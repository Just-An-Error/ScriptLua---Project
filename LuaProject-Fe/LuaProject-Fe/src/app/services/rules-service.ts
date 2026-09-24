import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { ConfigService } from "./app-config-key";
import { Observable } from "rxjs/internal/Observable";
import { RuleRead } from "../models/rule-read";
import { RuleExecution } from "../models/rule-execution";
import { CreateRule } from "../models/rule-create";

@Injectable({
  providedIn: 'root'
})
export class RuleService {
  private readonly _http = inject(HttpClient);
  private readonly _configService = inject(ConfigService);
  private get baseUrl(): string {
    return `${this._configService.settings.apiUrl}/Rules`;
  }

  // Get all rules
  getAllRules(): Observable<RuleRead[]> {
    return this._http.get<RuleRead[]>(`${this.baseUrl}`);
  }

  // Get a rule by its ID
  getRuleById(id: number) {
    return this._http.get<RuleRead>(`${this.baseUrl}/${id}`);
  }

  // Create a new rule
  createRule(rule: any) {
    return this._http.post(`${this.baseUrl}`, rule);
  }

  // Update a rule by its ID
  updateRule(id: number, rule: CreateRule) {
    return this._http.put(`${this.baseUrl}/${id}`, rule);
  }

  // Delete a rule by its ID
  deleteRule(id: number) {
    return this._http.delete(`${this.baseUrl}/${id}`);
  }

  runRule(id: number, inputs: Record<string, unknown>) {
    return this._http.post(`${this.baseUrl}/${id}/run`, inputs);
  }

  testRule(luaCode: string, testInputs: Record<string, unknown>) {
    return this._http.post<RuleExecution>(`${this.baseUrl}/test`, { luaCode, testInputs });
  }
}
