namespace LuaProject.Application.DTOs
{
    public class RuleExecutionDto
    {
        public int ExecutionId { get; set; }
        public int RuleId { get; set; }
        public string RuleName { get; set; } = string.Empty;
        public bool Success { get; set; }
        public object? Output { get; set; }
        public string? ErrorMessage { get; set; }
        public long DurationMs { get; set; }
    }
}
