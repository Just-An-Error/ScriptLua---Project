using LuaProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuaProject.Application.ServicesInterfaces
{
    public interface IExecutionService
    {
        Task<IReadOnlyList<RuleExecutionDto>> GetExecutionsByRuleAsync(int ruleId, int take = 20, CancellationToken cancellationToken = default);
    }
}
