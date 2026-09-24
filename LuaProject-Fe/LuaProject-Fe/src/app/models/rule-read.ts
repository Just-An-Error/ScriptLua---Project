export interface RuleRead {
  rulesId: number;
  rulesName: string;
  rulesDescription: string;
  rulesCodiceLua?: string;
  rulesIsActive: boolean;
  rulesCreatedAt: Date;
  rulesTriggerType: string;
}
