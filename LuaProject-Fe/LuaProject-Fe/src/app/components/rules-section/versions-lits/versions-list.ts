import { Component, EventEmitter, inject, Input, Output } from "@angular/core";
import { RuleVersion } from "../../../models/rule-versions";
import { MatIconModule } from "@angular/material/icon";
import { CommonModule } from "@angular/common";
import { VersionsService } from "../../../services/versions-service";

@Component({
  selector: "app-versions-list",
  templateUrl: "./versions-list.html",
  styleUrls: ["./versions-list.css", "../list-rules/list-rules.css"],
  imports: [MatIconModule, CommonModule]
})
export class VersionsListComponent {
  private readonly _versionsService = inject(VersionsService);
  @Input({required: true}) versions: RuleVersion[] = [];
  @Output() closeVersions = new EventEmitter<void>();

  close() {
    this.closeVersions.emit();
  }

  changeToCyrrentVersion(ruleId: number, versionId: number) {
    this._versionsService.changeVersionForRuleId(ruleId, versionId).subscribe({
      next: () => {
        window.alert("Versione impostata con successo");
        window.location.reload();
      },
      error: () => {
        window.alert("Errore durante l'impostazione della versione");
      }
    });
  }

}
