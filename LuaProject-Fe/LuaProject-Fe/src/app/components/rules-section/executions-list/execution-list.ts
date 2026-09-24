import { Component, EventEmitter, Input, Output } from "@angular/core";
import { MatIconModule } from "@angular/material/icon";
import { RuleExecution } from "../../../models/rule-execution";

@Component({
  selector: "app-execution-list",
  templateUrl: "./execution-list.html",
  styleUrls: ["./execution-list.css", "../list-rules/list-rules.css"],
  imports: [MatIconModule]
})
export class ExecutionListComponent {
  @Input({required: true}) executions: RuleExecution[] = [];
  @Output() closeHistory = new EventEmitter<void>();

  close() {
    this.closeHistory.emit();
  }
}
