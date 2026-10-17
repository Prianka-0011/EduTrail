import { Component, Input, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';

interface BstNode {
  val: number;
  left: BstNode | null;
  right: BstNode | null;
  x: number;
  y: number;
}

// Display-only binary search tree (pl-bst). Values come as a JSON array string;
// an optional highlight value is drawn in a different colour.
@Component({
  selector: 'pl-bst',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="bst-wrap">
      <svg [attr.width]="svgW" [attr.height]="svgH">
        <line *ngFor="let e of edges"
              [attr.x1]="e.x1" [attr.y1]="e.y1"
              [attr.x2]="e.x2" [attr.y2]="e.y2"
              stroke="#6c757d" stroke-width="2"/>
        <g *ngFor="let n of nodeList">
          <circle [attr.cx]="n.x" [attr.cy]="n.y" [attr.r]="R"
                  [attr.fill]="n.val === highlightVal ? '#e67e22' : '#0d6efd'"
                  stroke="white" stroke-width="2"/>
          <text [attr.x]="n.x" [attr.y]="n.y" dy="0.35em"
                text-anchor="middle" fill="white"
                font-size="13" font-weight="600" font-family="monospace">{{ n.val }}</text>
        </g>
      </svg>
    </div>
  `,
  styles: [`
    .bst-wrap { margin: 1rem 0; overflow-x: auto; }
    svg { display: block; }
  `],
})
export class PlBstComponent implements OnChanges {
  @Input() nodes = '';
  @Input() highlight = '';

  readonly R = 22;
  readonly H_GAP = 56;
  readonly V_GAP = 72;
  readonly MARGIN = 36;

  nodeList: BstNode[] = [];
  edges: { x1: number; y1: number; x2: number; y2: number }[] = [];
  svgW = 300;
  svgH = 200;
  highlightVal: number | null = null;

  ngOnChanges(): void {
    this.highlightVal = this.highlight ? +this.highlight : null;
    try {
      const vals: number[] = JSON.parse(this.nodes);
      if (!Array.isArray(vals) || vals.length === 0) { this.nodeList = []; this.edges = []; return; }
      const root = this.buildBst(vals);
      this.layout(root);
    } catch {
      this.nodeList = [];
      this.edges = [];
    }
  }

  private buildBst(vals: number[]): BstNode {
    let root: BstNode | null = null;
    for (const v of vals) root = this.insert(root, v);
    return root!;
  }

  private insert(node: BstNode | null, val: number): BstNode {
    if (!node) return { val, left: null, right: null, x: 0, y: 0 };
    if (val < node.val) node.left = this.insert(node.left, val);
    else node.right = this.insert(node.right, val);
    return node;
  }

  private layout(root: BstNode): void {
    let rank = 0;
    const assignX = (n: BstNode | null): void => {
      if (!n) return;
      assignX(n.left);
      n.x = rank++ * this.H_GAP + this.MARGIN + this.R;
      assignX(n.right);
    };

    const assignY = (n: BstNode | null, depth: number): number => {
      if (!n) return depth - 1;
      n.y = depth * this.V_GAP + this.MARGIN + this.R;
      return Math.max(assignY(n.left, depth + 1), assignY(n.right, depth + 1));
    };

    assignX(root);
    const maxDepth = assignY(root, 0);

    this.nodeList = [];
    this.edges = [];
    const collect = (n: BstNode | null, parent: BstNode | null): void => {
      if (!n) return;
      this.nodeList.push(n);
      if (parent) this.edges.push({ x1: parent.x, y1: parent.y, x2: n.x, y2: n.y });
      collect(n.left, n);
      collect(n.right, n);
    };
    collect(root, null);

    this.svgW = (this.nodeList.length - 1) * this.H_GAP + 2 * (this.MARGIN + this.R);
    this.svgH = (maxDepth + 1) * this.V_GAP + 2 * this.MARGIN;
  }
}
