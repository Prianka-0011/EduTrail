import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

// Renders a chunk of static question HTML (text, panels) with params already
// substituted server-side.
@Component({
  selector: 'app-html-block',
  standalone: true,
  imports: [CommonModule],
  template: `<span [innerHTML]="html"></span>`,
})
export class HtmlBlockComponent {
  @Input() html = '';
  @Input() answersName = '';
  @Input() feedback: unknown = null;
}
