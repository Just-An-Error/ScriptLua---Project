using System;
using System.Collections.Generic;
using System.Text;

namespace LuaProject.Application.DTOs
{
    public class LuaTestRequestDto
    {
        public string LuaCode { get; set; } = string.Empty;
        public Dictionary<string, object?> TestInputs { get; set; } = new Dictionary<string, object?>();
    }
}
