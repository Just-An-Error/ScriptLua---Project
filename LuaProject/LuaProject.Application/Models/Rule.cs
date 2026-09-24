using System;
using System.Collections.Generic;
using System.Text;

namespace LuaProject.Application.Models
{
    public class Rule
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? CodiceLua { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string TriggerType { get; set; }
    }
}
