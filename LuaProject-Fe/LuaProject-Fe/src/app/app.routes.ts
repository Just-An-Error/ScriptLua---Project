import { Routes } from '@angular/router';
import { ListRulesComponent } from './components/rules-section/list-rules/list-rules';
import { RulesComponent } from './components/rules-section/rules';

export const routes: Routes = [
  {
    path: 'rules-list',
    component: ListRulesComponent,
  },
  {
    path: 'create-rule',
    component: RulesComponent,
  },
  {
    path: '',
    redirectTo: 'rules-list',
    pathMatch: 'full',
  },
  {
    path: '**',
    redirectTo: 'rules-list',
  }
];
