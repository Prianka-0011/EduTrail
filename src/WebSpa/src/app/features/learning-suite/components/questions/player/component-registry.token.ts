import { InjectionToken, Type } from '@angular/core';

export type ComponentRegistry = Record<string, Type<unknown>>;

export const COMPONENT_REGISTRY = new InjectionToken<ComponentRegistry>('COMPONENT_REGISTRY');
