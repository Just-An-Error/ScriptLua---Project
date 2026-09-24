namespace LuaProject.Application.Models
{
    public class Execution
    {
        public int Id { get; set; }
        public int RuleId { get; set; }
        public DateTime ExecutedAt { get; set; }
        public string InputJson { get; set; } = "{}";
        public string? OutputJson { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public long DurationMs { get; set; }
        public bool IsTest { get; set; }
    }
}
