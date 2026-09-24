import { CommonModule } from "@angular/common";
import { Component, inject, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { RuleRead } from "../../../models/rule-read";
import { RuleService } from "../../../services/rules-service";
import { MatIconModule } from "@angular/material/icon";
import { ExecutionService } from "../../../services/execution-service";
import { RuleExecution } from "../../../models/rule-execution";
import { ExecutionListComponent } from "../executions-list/execution-list";
import { RuleVersion } from "../../../models/rule-versions";
import { VersionsListComponent } from "../versions-lits/versions-list";
import { VersionsService } from "../../../services/versions-service";
import { CreateRule } from "../../../models/rule-create";

@Component({
  selector: "app-list-rules",
  templateUrl: "./list-rules.html",
  styleUrls: ["./list-rules.css", "./../rules.css"],
  imports: [FormsModule, CommonModule, MatIconModule, ExecutionListComponent, VersionsListComponent],
})
export class ListRulesComponent {
  private readonly _ruleService = inject(RuleService);
  private readonly _executeService = inject(ExecutionService);
  private readonly _versionService = inject(VersionsService);

  protected allRules = signal<RuleRead[]>([]);
  protected isViewDescription = signal(false);
  protected isViewCode = signal<boolean>(false);
  protected isViewHistory = signal<boolean>(false);
  protected descriptionToShow = signal<string>("");
  protected executionsToShow = signal<RuleExecution[]>([]);
  protected isViewVersions = signal<boolean>(false);
  protected versionsToShow = signal<RuleVersion[]>([]);
  protected isEditing = signal<boolean>(false);
  protected ruleToEdit = signal<RuleRead | null>(null);

  private loadRules(): void {
    this._ruleService.getAllRules().subscribe({
      next: (rules) => {
        this.allRules.set(rules);
      },
      error: () => {
        window.alert("Errore durante il recupero delle regole");
        this.allRules.set([]);
      }
    });
  }

  ngOnInit() {
    this.loadRules();
  }

  toggleDescription(rulesDescription: string) {
    this.isViewDescription.set(true);
    this.descriptionToShow.set(rulesDescription);
  }

  closeDescription() {
    this.isViewDescription.set(false);
    this.descriptionToShow.set("");
  }

  toggleCode(rulesCodiceLua: string | undefined) {
    this.isViewCode.set(true);
    this.descriptionToShow.set(rulesCodiceLua || "Codice Lua non presente");
  }

  closeCode() {
    this.isViewCode.set(false);
    this.descriptionToShow.set("");
  }

  goToCreateRule() {
    window.location.href = "/create-rule";
  }

  deleteRule(ruleId: number) {
    this._ruleService.deleteRule(ruleId).subscribe({
      next: () => {
        this.loadRules();
      },
      error: () => {
        window.alert("Errore durante l'eliminazione");
      }
    });
  }

  goToRuleHistory(ruleId: number) {
    this.executionsToShow.set([]);

    this._executeService.getExecutionByRuleId(ruleId).subscribe({
      next: (executions) => {
        this.isViewHistory.set(true);
        this.executionsToShow.set(executions);
      },
      error: () => {
        window.alert("Errore durante il recupero delle esecuzioni");
      }
    });
  }

  goToRuleVersions(ruleId: number) {
    this.versionsToShow.set([]);

    this._versionService.getAllVersionsByRuleId(ruleId).subscribe({
      next: (versions) => {
        this.isViewVersions.set(true);
        this.versionsToShow.set(versions);
      },
      error: () => {
        window.alert("Errore durante il recupero delle versioni");
      }
    });
  }

  closeHistory() {
    this.isViewHistory.set(false);
    this.executionsToShow.set([]);
  }

  closeVersions() {
    this.isViewVersions.set(false);
    this.versionsToShow.set([]);
  }

  runRule(rule: RuleRead) {
    const inputs = window.prompt("Inserisci gli input in formato JSON");

    if (inputs) {
      let parsedInputs: Record<string, unknown>;
      try {
        parsedInputs = JSON.parse(inputs);
      } catch (e) {
        window.alert("Input non valido. Assicurati che sia un JSON valido.");
        return;
      }
      this._ruleService.runRule(rule.rulesId, parsedInputs).subscribe({
        next: (result) => {
          window.alert("Regola eseguita con successo. Risultato: " + JSON.stringify(result));
        },
        error: () => {
          window.alert("Errore durante l'esecuzione della regola");
        }
      });
    }
  }

  openEditRule(ruleId: number) {
    this._ruleService.getRuleById(ruleId).subscribe({
      next: (rule) => {
        this.isEditing.set(true);
        this.ruleToEdit.set(rule);
      },
      error: () => {
        window.alert("Errore durante il recupero della regola");
        this.isEditing.set(false);
      }
    });
  }

  closeEditRule() {
    this.isEditing.set(false);
    this.ruleToEdit.set(null);
  }

  cancelEdit() {
    this.closeEditRule();
  }

  saveEdit() {
    const rule = this.ruleToEdit();

    if (rule) {
      this._ruleService.updateRule(rule.rulesId, this.mapRuleReadToCreateRule(rule!)).subscribe({
        next: () => {
          this.closeEditRule();
          this.loadRules();
        },
        error: () => {
          window.alert("Errore durante il salvataggio della modifica");
        }
      });
    }
  }

  mapRuleReadToCreateRule(rule: RuleRead): CreateRule {
    return {
      RulesName: rule.rulesName,
      RulesDescription: rule.rulesDescription,
      rulesCodiceLua: rule.rulesCodiceLua,
      RulesIsActive: rule.rulesIsActive,
      RulesTriggerType: rule.rulesTriggerType
    };
  }
}
