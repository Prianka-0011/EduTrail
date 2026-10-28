import {
  Component, Input, Output, EventEmitter, ElementRef, ViewChild,
  AfterViewInit, OnDestroy, Inject, PLATFORM_ID
} from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { basicSetup } from 'codemirror';
import { EditorView } from '@codemirror/view';
import { EditorState } from '@codemirror/state';
import { cpp } from '@codemirror/lang-cpp';

import { QuestionCatalogService } from '../services/question-catalog.service';
import { FieldFeedback, RunCodeResult } from '../interfaces/IPlayer';

// Code-editor question component (pl-code-editor): a CodeMirror editor plus an
// editable stdin box, a Run button that compiles+runs in the isolated runner,
// and an output panel. The current code is emitted as the answer for grading.
@Component({
  selector: 'pl-code-editor',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="code-editor">
      <div class="editor-host" #editor></div>

      <div class="io-row">
        <div class="io-col">
          <label>Input (stdin)</label>
          <textarea class="stdin" [(ngModel)]="stdin" rows="2" spellcheck="false"></textarea>
        </div>
        <button class="run-btn" (click)="run()" [disabled]="running">
          {{ running ? 'Running…' : '▶ Run' }}
        </button>
      </div>

      <div class="output" *ngIf="output || runError">
        <label>Output</label>
        <pre *ngIf="runError" class="err">{{ runError }}</pre>
        <ng-container *ngIf="output">
          <pre *ngIf="!output.compiled" class="err">{{ output.compileOutput }}</pre>
          <pre *ngIf="output.compiled && output.timedOut" class="err">Timed out.</pre>
          <pre *ngIf="output.compiled && !output.timedOut">{{ output.stdout }}</pre>
          <pre *ngIf="output.compiled && output.stderr" class="warn">{{ output.stderr }}</pre>
        </ng-container>
      </div>

      <div *ngIf="_feedback" class="feedback" [class.ok]="_feedback.isCorrect" [class.bad]="!_feedback.isCorrect">
        {{ _feedback.message }}
      </div>
    </div>
  `,
  styles: [`
    .code-editor { margin: 0.75rem 0; }
    .editor-host { border: 1px solid #ced4da; border-radius: 0.375rem; overflow: hidden; }
    ::ng-deep .editor-host .cm-scroller { max-height: 360px; }
    .io-row { display: flex; gap: 0.75rem; align-items: flex-end; margin-top: 0.6rem; }
    .io-col { flex: 1; display: flex; flex-direction: column; gap: 3px; }
    label { font-size: 0.8rem; font-weight: 600; color: #6c757d; }
    .stdin { font-family: 'Consolas', monospace; font-size: 0.85rem; padding: 0.35rem 0.5rem; border: 1px solid #ced4da; border-radius: 0.375rem; resize: vertical; }
    .run-btn { padding: 0.45rem 1.1rem; background: #198754; color: #fff; border: none; border-radius: 0.375rem; cursor: pointer; font-size: 0.95rem; white-space: nowrap; }
    .run-btn:disabled { background: #6c757d; cursor: not-allowed; }
    .output { margin-top: 0.7rem; }
    .output pre { margin: 0.2rem 0 0; padding: 0.6rem 0.75rem; background: #1e1e2e; color: #cdd6f4; border-radius: 0.375rem; font-family: 'Consolas', monospace; font-size: 0.82rem; white-space: pre-wrap; max-height: 220px; overflow: auto; }
    .output pre.err { background: #2b1416; color: #f1aeb5; }
    .output pre.warn { background: #2b2616; color: #ffe08a; }
    .feedback { margin-top: 0.6rem; font-size: 0.9rem; font-weight: 500; }
    .feedback.ok { color: #198754; }
    .feedback.bad { color: #dc3545; }
  `],
})
export class PlCodeEditorComponent implements AfterViewInit, OnDestroy {
  @ViewChild('editor') editorRef!: ElementRef<HTMLElement>;

  @Input() answersName = 'code';
  @Input() language = 'cpp';
  @Input() set starter(v: string) { this._starter = v || ''; }
  @Input() set sampleInput(v: string) { if (v) this.stdin = v; }
  @Input() set feedback(v: FieldFeedback | null) { this._feedback = v; }
  @Output() answerChange = new EventEmitter<string>();

  stdin = '';
  code = '';
  running = false;
  output: RunCodeResult | null = null;
  runError = '';
  _feedback: FieldFeedback | null = null;
  private _starter = '';
  private view?: EditorView;

  constructor(
    private qs: QuestionCatalogService,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  ngAfterViewInit(): void {
    if (!isPlatformBrowser(this.platformId)) return;

    this.code = this._starter;
    this.answerChange.emit(this.code); // unedited submissions still carry the starter

    this.view = new EditorView({
      state: EditorState.create({
        doc: this._starter,
        extensions: [
          basicSetup,
          cpp(),
          EditorView.updateListener.of(u => {
            if (u.docChanged) {
              this.code = u.state.doc.toString();
              this.answerChange.emit(this.code);
            }
          }),
        ],
      }),
      parent: this.editorRef.nativeElement,
    });
  }

  run(): void {
    this.running = true;
    this.output = null;
    this.runError = '';
    this.qs.runCode(this.language, this.code, this.stdin).subscribe({
      next: res => { this.output = res; this.running = false; },
      error: err => {
        this.runError = err?.status === 429
          ? 'You are running code too frequently. Please wait a moment.'
          : 'Could not run your code. The code runner may be unavailable.';
        this.running = false;
      },
    });
  }

  ngOnDestroy(): void {
    this.view?.destroy();
  }
}
