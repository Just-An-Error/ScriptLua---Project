using System;
using System.Collections.Generic;
using System.Text;

namespace LuaProject.Application.Models
{
    public class RuleVersion
    {
        public int Id { get; set; }
        public int RuleId { get; set; }
        public Rule Rule { get; set; } = null!;
        public string LuaCode { get; set; } = string.Empty;
        public int VersionNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
