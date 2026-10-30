import {
  Directive, Input, inject, ViewContainerRef, ComponentRef, Type, OnDestroy
} from '@angular/core';
import { COMPONENT_REGISTRY } from './component-registry.token';
import { FieldFeedback } from '../interfaces/IPlayer';
import { HtmlBlockComponent } from './html-block.component';

function toCamelCase(attr: string): string {
  return attr.replace(/-(\w)/g, (_, c: string) => c.toUpperCase());
}

interface PlComponentInstance {
  answersName?: string;
  feedback?: FieldFeedback | null;
  answerChange?: { subscribe: (fn: (v: unknown) => void) => void };
  [key: string]: unknown;
}

// Parses rendered question HTML and builds live Angular components for each pl-*
// tag found in the component registry; everything else renders as static HTML.
@Directive({
  selector: '[appDynamicQuestion]',
  standalone: true,
})
export class DynamicQuestionRendererDirective implements OnDestroy {
  private vcr = inject(ViewContainerRef);
  private registry = inject(COMPONENT_REGISTRY);
  private answers: Record<string, unknown> = {};
  private componentRefs: ComponentRef<PlComponentInstance>[] = [];

  @Input('appDynamicQuestion')
  set html(value: string) {
    if (value) this.render(value);
  }

  getAnswers(): Record<string, unknown> {
    return { ...this.answers };
  }

  setFeedback(feedback: Record<string, FieldFeedback>): void {
    for (const ref of this.componentRefs) {
      const name = ref.instance.answersName;
      if (name && feedback[name] !== undefined) {
        ref.setInput('feedback', feedback[name]);
      }
    }
  }

  private render(html: string): void {
    this.vcr.clear();
    this.componentRefs = [];
    this.answers = {};

    const doc = new DOMParser().parseFromString(html, 'text/html');
    let rawBuffer = '';

    const flushRaw = () => {
      if (rawBuffer.trim()) {
        const ref = this.vcr.createComponent(HtmlBlockComponent as Type<unknown>) as ComponentRef<HtmlBlockComponent>;
        ref.setInput('html', rawBuffer);
        rawBuffer = '';
      }
    };

    doc.body.childNodes.forEach(node => {
      if (node.nodeType === Node.ELEMENT_NODE) {
        const el = node as Element;
        const tagName = el.tagName.toLowerCase();
        const ComponentClass = this.registry[tagName];

        if (tagName === 'pl-question-panel') {
          flushRaw();
          const panelHtml = `<div style="padding:1rem;border:1px solid #dee2e6;border-radius:.375rem;margin-bottom:1rem;background:#fff">${el.innerHTML}</div>`;
          const ref = this.vcr.createComponent(HtmlBlockComponent as Type<unknown>) as ComponentRef<HtmlBlockComponent>;
          ref.setInput('html', panelHtml);
          return;
        }

        if (ComponentClass) {
          flushRaw();
          const ref = this.vcr.createComponent(ComponentClass as Type<PlComponentInstance>) as ComponentRef<PlComponentInstance>;
          this.componentRefs.push(ref);

          el.getAttributeNames().forEach(attr => {
            ref.setInput(toCamelCase(attr), el.getAttribute(attr));
          });

          const answersName = el.getAttribute('answers-name') || tagName;
          ref.instance.answerChange?.subscribe((val: unknown) => {
            this.answers[answersName] = val;
          });
        } else {
          rawBuffer += el.outerHTML;
        }
      } else if (node.nodeType === Node.TEXT_NODE) {
        rawBuffer += node.textContent;
      }
    });

    flushRaw();
  }

  ngOnDestroy(): void {
    this.vcr.clear();
  }
}
