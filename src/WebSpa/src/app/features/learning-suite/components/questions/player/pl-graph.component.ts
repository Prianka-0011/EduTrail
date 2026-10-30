import { Component, Input, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';

interface GraphNode { label: string; x: number; y: number; }
interface GraphEdge { from: string; to: string; weight: number; mx: number; my: number; }

// Display-only weighted graph (pl-graph). Nodes/edges come as JSON array strings.
@Component({
  selector: 'pl-graph',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="graph-wrap">
      <svg [attr.width]="W" [attr.height]="H">
        <g *ngFor="let e of parsedEdges">
          <line [attr.x1]="nodeMap[e.from].x" [attr.y1]="nodeMap[e.from].y"
                [attr.x2]="nodeMap[e.to].x"   [attr.y2]="nodeMap[e.to].y"
                stroke="#9aa4b2" stroke-width="2"/>
          <circle [attr.cx]="e.mx" [attr.cy]="e.my" r="12" fill="white" stroke="#dee2e6" stroke-width="1.5"/>
          <text [attr.x]="e.mx" [attr.y]="e.my" dy="0.35em"
                text-anchor="middle" font-size="11" font-weight="600" fill="#343a40">{{ e.weight }}</text>
        </g>
        <g *ngFor="let n of parsedNodes">
          <circle [attr.cx]="n.x" [attr.cy]="n.y" r="22" fill="#0d6efd" stroke="white" stroke-width="2"/>
          <text [attr.x]="n.x" [attr.y]="n.y" dy="0.35em"
                text-anchor="middle" fill="white" font-size="14" font-weight="700">{{ n.label }}</text>
        </g>
      </svg>
    </div>
  `,
  styles: [`.graph-wrap { margin: 1rem 0; overflow-x: auto; } svg { display: block; }`],
})
export class PlGraphComponent implements OnChanges {
  @Input() nodes = '';
  @Input() edges = '';

  readonly W = 340;
  readonly H = 320;
  readonly CX = 170;
  readonly CY = 155;
  readonly RADIUS = 110;

  parsedNodes: GraphNode[] = [];
  parsedEdges: GraphEdge[] = [];
  nodeMap: Record<string, GraphNode> = {};

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
      }));
    } catch {
      this.parsedNodes = [];
      this.parsedEdges = [];
      this.nodeMap = {};
    }
  }
}
