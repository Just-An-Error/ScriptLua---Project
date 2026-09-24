export interface CreateRule {
  RulesName: string;
  RulesDescription: string;
  rulesCodiceLua?: string;
  RulesIsActive: boolean;
  RulesTriggerType: string;
}
