import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FieldFeedback } from '../interfaces/IPlayer';

// Drag-to-order code blocks (pl-order-blocks). Blocks come from the "options"
// attribute (JSON array string, already shuffled by server.py); emits the
// ordered list the student builds.
@Component({
  selector: 'pl-order-blocks',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="ob-wrapper">
      <div class="ob-col">
        <div class="ob-label">Available blocks</div>
        <div class="ob-zone"
             (dragover)="$event.preventDefault()"
             (drop)="onZoneDrop($event, 'source')">
          <div class="ob-empty" *ngIf="sourceBlocks.length === 0">All blocks placed</div>
          <div *ngFor="let block of sourceBlocks; let i = index"
               class="ob-block"
               draggable="true"
               (dragstart)="onDragStart($event, 'source', i)"
               (dragover)="$event.preventDefault(); $event.stopPropagation()"
               (drop)="onBlockDrop($event, 'source', i)">
            <pre><code>{{ block }}</code></pre>
          </div>
        </div>
      </div>

      <div class="ob-col">
        <div class="ob-label">Your sequence</div>
        <div class="ob-zone ob-target"
             [class.ob-correct]="_feedback?.isCorrect === true"
             [class.ob-incorrect]="_feedback?.isCorrect === false"
             (dragover)="$event.preventDefault()"
             (drop)="onZoneDrop($event, 'target')">
          <div *ngFor="let block of targetBlocks; let i = index"
               class="ob-block ob-placed"
               draggable="true"
               (dragstart)="onDragStart($event, 'target', i)"
               (dragover)="$event.preventDefault(); $event.stopPropagation()"
               (drop)="onBlockDrop($event, 'target', i)">
            <span class="ob-num">{{ i + 1 }}</span>
            <pre><code>{{ block }}</code></pre>
          </div>
          <div *ngFor="let slot of emptySlots; let i = index"
               class="ob-slot"
               (dragover)="$event.preventDefault(); $event.stopPropagation()"
               (drop)="onBlockDrop($event, 'target', targetBlocks.length + i)">
            <span class="ob-num">{{ targetBlocks.length + i + 1 }}</span>
          </div>
        </div>
        <div *ngIf="_feedback" class="ob-msg"
             [style.color]="_feedback.isCorrect ? '#198754' : '#dc3545'">
          {{ _feedback.message }}
        </div>
      </div>
    </div>
  `,
  styles: [`
    .ob-wrapper { display: flex; gap: 1.2rem; margin: 1rem 0; }
    .ob-col { flex: 1; display: flex; flex-direction: column; }
    .ob-label { font-weight: 600; font-size: 0.82rem; color: #6c757d; margin-bottom: 6px; text-transform: uppercase; letter-spacing: 0.05em; }
    .ob-zone { flex: 1; border: 2px dashed #ced4da; border-radius: 8px; padding: 6px; background: #f8f9fa; min-height: 60px; }
    .ob-target { border-color: #adb5bd; background: #f1f3f5; }
    .ob-correct { border-color: #198754 !important; background: #d1e7dd !important; }
    .ob-incorrect { border-color: #dc3545 !important; background: #f8d7da !important; }
    .ob-block { display: flex; align-items: center; background: #1e1e2e; color: #cdd6f4; border-radius: 5px; margin: 3px 0; cursor: grab; padding: 4px 10px; }
    .ob-block:hover { opacity: 0.88; }
    .ob-block pre { margin: 0; flex: 1; }
    .ob-block pre code { font-family: 'Consolas', 'Courier New', monospace; font-size: 0.82rem; white-space: pre; }
    .ob-num { color: #6c7086; font-size: 0.72rem; min-width: 1.4rem; text-align: right; margin-right: 8px; flex-shrink: 0; }
    .ob-slot { display: flex; align-items: center; border: 1px dashed #adb5bd; border-radius: 5px; margin: 3px 0; padding: 4px 10px; min-height: 34px; background: transparent; }
    .ob-empty { color: #adb5bd; font-size: 0.85rem; text-align: center; padding: 1.5rem 1rem; }
    .ob-msg { margin-top: 6px; font-size: 0.9rem; font-weight: 500; }
  `]
})
export class PlOrderBlocksComponent {
  @Input() answersName = '';
  @Input() set options(val: string) {
    try {
      const parsed = JSON.parse(val);
      this.sourceBlocks = Array.isArray(parsed) ? parsed.map(String) : [];
    } catch { this.sourceBlocks = []; }
    this.totalBlocks = this.sourceBlocks.length;
    this.targetBlocks = [];
  }
  @Input() set feedback(val: FieldFeedback | null) { this._feedback = val; }
  @Output() answerChange = new EventEmitter<string[]>();

  sourceBlocks: string[] = [];
  targetBlocks: string[] = [];
  _feedback: FieldFeedback | null = null;
  private totalBlocks = 0;
  private drag: { zone: 'source' | 'target'; index: number } | null = null;

  get emptySlots(): number[] {
    return Array(Math.max(0, this.totalBlocks - this.targetBlocks.length)).fill(0).map((_, i) => i);
  }

  onDragStart(event: DragEvent, zone: 'source' | 'target', index: number): void {
    this.drag = { zone, index };
    event.dataTransfer?.setData('text/plain', '');
  }

  onBlockDrop(event: DragEvent, toZone: 'source' | 'target', toIndex: number): void {
    event.preventDefault();
    event.stopPropagation();
    this.performDrop(toZone, toIndex);
  }

  onZoneDrop(event: DragEvent, toZone: 'source' | 'target'): void {
    event.preventDefault();
    const len = toZone === 'source' ? this.sourceBlocks.length : this.targetBlocks.length;
    this.performDrop(toZone, len);
  }

  private performDrop(toZone: 'source' | 'target', toIndex: number): void {
    if (!this.drag) return;
    const { zone: fromZone, index: fromIndex } = this.drag;
    this.drag = null;

    const srcArr = [...(fromZone === 'source' ? this.sourceBlocks : this.targetBlocks)];
    const dstArr = fromZone === toZone ? srcArr : [...(toZone === 'source' ? this.sourceBlocks : this.targetBlocks)];

    const [item] = srcArr.splice(fromIndex, 1);
    const insertAt = fromZone === toZone && fromIndex < toIndex ? toIndex - 1 : toIndex;
    dstArr.splice(Math.min(insertAt, dstArr.length), 0, item);

    if (fromZone === 'source') this.sourceBlocks = srcArr;
    else this.targetBlocks = srcArr;
    if (toZone === 'source') this.sourceBlocks = dstArr;
    else this.targetBlocks = dstArr;

    this.answerChange.emit([...this.targetBlocks]);
  }
}
