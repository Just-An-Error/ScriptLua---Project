namespace LuaProject.Application.DTOs
{
    public class RulesCreate
    {
        public required string RulesName { get; set; }
        public required string RulesDescription { get; set; }
        public string? RulesCodiceLua { get; set; }
        public bool RulesIsActive { get; set; }
        public required string RulesTriggerType { get; set; }
    }
}
