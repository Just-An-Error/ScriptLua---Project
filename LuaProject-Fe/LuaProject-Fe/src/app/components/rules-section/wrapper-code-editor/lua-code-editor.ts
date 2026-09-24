import { AfterViewInit, Component, ElementRef, EventEmitter, Input, OnDestroy, Output, ViewChild, viewChild } from "@angular/core";

@Component({
  selector: "app-lua-code-editor",
  standalone: true,
  template: `<div #editorContainer style="height: 300px; border: 1px solid #444;"></div>`
})
export class LuaCodeEditorComponent implements AfterViewInit, OnDestroy {
  @ViewChild("editorContainer", { static: true }) editorContainer!: ElementRef;
  @Input() code: string | undefined = "";
  @Output() codeChange = new EventEmitter<string>();

  private _editor: any;

  async ngAfterViewInit(): Promise<void> {
    const monaco = await import("monaco-editor");
    this._editor = monaco.editor.create(this.editorContainer.nativeElement, {
      value: this.code,
      language: "lua",
      theme: "vs-dark",
      automaticLayout: true,
      minimap: { enabled: false },
    });

    this._editor.onDidChangeModelContent(() => {
      this.codeChange.emit(this._editor.getValue());
    });
  }

  ngOnDestroy(): void {
    if (this._editor) {
      this._editor.dispose();
    }
  }
}
