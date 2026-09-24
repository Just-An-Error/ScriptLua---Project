import { ChangeDetectorRef, Component, inject, signal } from "@angular/core";
import { CreateRule } from "../../models/rule-create";
import { FormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { RuleService } from "../../services/rules-service";
import { RuleRead } from "../../models/rule-read";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { LuaCodeEditorComponent } from "./wrapper-code-editor/lua-code-editor";
import { RuleExecution } from "../../models/rule-execution";

@Component({
  selector: "app-rules",
  templateUrl: "./rules.html",
  styleUrls: ["./rules.css", "../../../styles.css"],
  imports: [CommonModule, FormsModule, MatProgressSpinnerModule, LuaCodeEditorComponent],
  standalone: true
})
export class RulesComponent {
  private readonly _ruleService = inject(RuleService);
  protected rule = signal<RuleRead | null>(null);
  protected isLoading = signal(false);
  protected createRule: CreateRule  = {
    RulesName: '',
    RulesDescription: '',
    rulesCodiceLua: '',
    RulesIsActive: false,
    RulesTriggerType: ''
  };
  protected testInputJson = signal<string>('');
  protected testResult = signal<RuleExecution | null>(null);

  createNewRule(newRule: CreateRule | null) {
    this.isLoading.set(true);
    console.log("Creating new rule:", newRule);

    if (!newRule || !newRule.RulesName || !newRule.RulesDescription) {
      console.error("All fields are required to create a new rule.");
      this.isLoading.set(false);
      return;
    }

    this._ruleService.createRule(newRule).subscribe({
      next: (response) => {
        console.log("Rule created successfully", response);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error("Error creating rule", error);
        this.isLoading.set(false);
      }
    });
  }

  testRule() {
    if (!this.createRule.rulesCodiceLua) {
      window.alert("Lua code is required to test the rule.");
      return;
    }

    if (!this.testInputJson() || this.testInputJson().trim() === "") {
      window.alert("Test input is required to test the rule.");
      return;
    }

    let testInputs: Record<string, unknown>;
    try {
      testInputs = JSON.parse(this.testInputJson());
    } catch {
      window.alert("Test input must be a valid JSON string.");
      return;
    }

    this._ruleService.testRule(this.createRule.rulesCodiceLua, testInputs).subscribe({
      next: (result) => {
        this.testResult.set(result);
      },
      error: () => {
        window.alert("Errore durante il test della regola.");
      }
    });
  }

  goToList() {
    window.location.href = "/rules-list";
  }
}
