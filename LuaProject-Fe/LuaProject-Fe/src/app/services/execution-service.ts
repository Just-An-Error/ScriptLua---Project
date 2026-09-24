import { inject, Injectable } from "@angular/core";
import { ConfigService } from "./app-config-key";
import { HttpClient, HttpParams } from "@angular/common/http";
import { RuleExecution } from "../models/rule-execution";

@Injectable({
  providedIn: 'root'
})
export class ExecutionService {
  private readonly _http = inject(HttpClient);
  private readonly _configService = inject(ConfigService);
  private get baseUrl(): string {
    return `${this._configService.settings.apiUrl}/Executions`;
  }

  getExecutionByRuleId(ruleId: number, take: number = 20) {
    const params = new HttpParams().set('take', take.toString());
    return this._http.get<RuleExecution[]>(`${this.baseUrl}/${ruleId}`, { params });
  }
}
