import { RuleRead } from "./rule-read";

export interface RuleVersion {
  id: number;
  ruleId: number;
  rule: RuleRead;
  luaCode: string;
  versionNumber: number;
  createdAt: Date;
}
