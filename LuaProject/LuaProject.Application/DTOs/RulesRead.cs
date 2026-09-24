namespace LuaProject.Application.DTOs
{
    public class RulesRead
    {
        public int RulesId { get; set; }
        public required string RulesName { get; set; }
        public required string RulesDescription { get; set; }
        public string? RulesCodiceLua { get; set; }
        public bool RulesIsActive { get; set; }
        public DateTime RulesCreatedAt { get; set; }
        public required string RulesTriggerType { get; set; }
    }
}
