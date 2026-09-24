import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { RulesComponent } from '../rules-section/rules';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css',
  standalone: true
})
export class App {
  protected readonly title = signal('LuaProject-Fe');
}
