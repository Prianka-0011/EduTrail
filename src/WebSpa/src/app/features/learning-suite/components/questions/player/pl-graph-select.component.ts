import { Component, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FieldFeedback } from '../interfaces/IPlayer';

interface GraphNode { label: string; x: number; y: number; }
interface SelectableEdge { from: string; to: string; weight: number; mx: number; my: number; selected: boolean; }

// Clickable graph for selecting edges (pl-graph-select), e.g. picking MST edges.
// Emits the selected edges as [from, to] pairs.
@Component({
  selector: 'pl-graph-select',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="graph-wrap">
      <p class="hint">Click edges to select them. Selected edges turn green.</p>
      <svg [attr.width]="W" [attr.height]="H">
        <g *ngFor="let e of parsedEdges">
          <line [attr.x1]="nodeMap[e.from].x" [attr.y1]="nodeMap[e.from].y"
                [attr.x2]="nodeMap[e.to].x"   [attr.y2]="nodeMap[e.to].y"
                [attr.stroke]="e.selected ? '#198754' : '#9aa4b2'"
                [attr.stroke-width]="e.selected ? 4 : 2" stroke-linecap="round"/>
          <line [attr.x1]="nodeMap[e.from].x" [attr.y1]="nodeMap[e.from].y"
                [attr.x2]="nodeMap[e.to].x"   [attr.y2]="nodeMap[e.to].y"
                stroke="transparent" stroke-width="18" style="cursor:pointer"
                (click)="toggleEdge(e)"/>
          <circle [attr.cx]="e.mx" [attr.cy]="e.my" r="12"
                  [attr.fill]="e.selected ? '#198754' : 'white'"
                  [attr.stroke]="e.selected ? '#198754' : '#dee2e6'" stroke-width="1.5"
                  style="cursor:pointer" (click)="toggleEdge(e)"/>
          <text [attr.x]="e.mx" [attr.y]="e.my" dy="0.35em"
                text-anchor="middle" font-size="11" font-weight="600"
                [attr.fill]="e.selected ? 'white' : '#343a40'"
                style="cursor:pointer; pointer-events:none">{{ e.weight }}</text>
        </g>
        <g *ngFor="let n of parsedNodes">
          <circle [attr.cx]="n.x" [attr.cy]="n.y" r="22" fill="#0d6efd" stroke="white" stroke-width="2"/>
          <text [attr.x]="n.x" [attr.y]="n.y" dy="0.35em"
                text-anchor="middle" fill="white" font-size="14" font-weight="700">{{ n.label }}</text>
        </g>
      </svg>
      <div *ngIf="_feedback" class="feedback" [style.color]="_feedback.isCorrect ? '#198754' : '#dc3545'">
        {{ _feedback.message }}
      </div>
    </div>
  `,
  styles: [`
    .graph-wrap { margin: 1rem 0; }
    svg { display: block; }
    .hint { font-size: 0.85rem; color: #6c757d; margin-bottom: 0.25rem; }
    .feedback { margin-top: 0.5rem; font-size: 0.9rem; font-weight: 500; }
  `],
})
export class PlGraphSelectComponent implements OnChanges {
  @Input() answersName = '';
  @Input() nodes = '';
  @Input() edges = '';
  @Input() set feedback(val: FieldFeedback | null) { this._feedback = val; }
  @Output() answerChange = new EventEmitter<string[][]>();

  readonly W = 340;
  readonly H = 320;
  readonly CX = 170;
  readonly CY = 155;
  readonly RADIUS = 110;

  parsedNodes: GraphNode[] = [];
  parsedEdges: SelectableEdge[] = [];
  nodeMap: Record<string, GraphNode> = {};
  _feedback: FieldFeedback | null = null;

  ngOnChanges(): void {
    try {
      const labels: string[] = JSON.parse(this.nodes);
      const edgeData: [string, string, number][] = JSON.parse(this.edges);
      const n = labels.length;

      this.parsedNodes = labels.map((label, i) => {
        const angle = (2 * Math.PI * i / n) - Math.PI / 2;
        return { label, x: Math.round(this.CX + this.RADIUS * Math.cos(angle)), y: Math.round(this.CY + this.RADIUS * Math.sin(angle)) };
      });
      this.nodeMap = Object.fromEntries(this.parsedNodes.map(n => [n.label, n]));

      this.parsedEdges = edgeData.map(([from, to, weight]) => ({
        from, to, weight,
        mx: Math.round((this.nodeMap[from].x + this.nodeMap[to].x) / 2),
        my: Math.round((this.nodeMap[from].y + this.nodeMap[to].y) / 2),
        selected: false,
      }));
    } catch {
      this.parsedNodes = [];
      this.parsedEdges = [];
      this.nodeMap = {};
    }
  }

  toggleEdge(edge: SelectableEdge): void {
    edge.selected = !edge.selected;
    this.answerChange.emit(
      this.parsedEdges.filter(e => e.selected).map(e => [e.from, e.to])
    );
  }
}
