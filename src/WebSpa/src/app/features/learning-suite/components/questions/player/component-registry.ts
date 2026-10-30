import { Provider } from '@angular/core';
import { COMPONENT_REGISTRY, ComponentRegistry } from './component-registry.token';
import { PlNumberInputComponent } from './pl-number-input.component';
import { PlMultipleChoiceComponent } from './pl-multiple-choice.component';
import { PlOrderBlocksComponent } from './pl-order-blocks.component';
import { PlGraphComponent } from './pl-graph.component';
import { PlBstComponent } from './pl-bst.component';
import { PlGraphSelectComponent } from './pl-graph-select.component';

// Maps pl-* tag names to the Angular components that render them.
const registry: ComponentRegistry = {
  'pl-number-input': PlNumberInputComponent,
  'pl-multiple-choice': PlMultipleChoiceComponent,
  'pl-order-blocks': PlOrderBlocksComponent,
  'pl-graph': PlGraphComponent,
  'pl-bst': PlBstComponent,
  'pl-graph-select': PlGraphSelectComponent,
};

export const COMPONENT_REGISTRY_PROVIDER: Provider = {
  provide: COMPONENT_REGISTRY,
  useValue: registry,
};
