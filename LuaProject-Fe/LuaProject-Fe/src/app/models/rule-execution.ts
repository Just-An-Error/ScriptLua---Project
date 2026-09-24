export interface RuleExecution {
  executionId: number;
  ruleId: number;
  ruleName: string;
  success: boolean;
  output: unknown;
  errorMessage: string | null;
  durationMs: number;
}
