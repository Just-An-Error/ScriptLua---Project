import { inject, Injectable } from "@angular/core";
import { ConfigService } from "./app-config-key";
import { HttpClient } from "@angular/common/http";
import { RuleVersion } from "../models/rule-versions";

@Injectable({
  providedIn: "root",
})
export class VersionsService {
  private readonly _http = inject(HttpClient);
  private readonly _configService = inject(ConfigService);
  private get baseUrl(): string {
    return `${this._configService.settings.apiUrl}/RuleVersion`;
  }

  getAllVersionsByRuleId(ruleId: number) {
    return this._http.get<RuleVersion[]>(`${this.baseUrl}/${ruleId}/versions`);
  }

  changeVersionForRuleId(ruleId: number, versionId: number) {
    return this._http.post(`${this.baseUrl}/${ruleId}/versions/${versionId}/restore`, {});
  }
}
