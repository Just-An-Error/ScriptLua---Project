using LuaProject.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuaProject.Application.RepositoryInterfaces
{
    public interface IRuleVersionRepository
    {
        Task AddAsync(RuleVersion verison, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<RuleVersion>> GetByRuleIdAsync(int ruleId, CancellationToken cancellationToken = default);
        Task<int> GetLastVersionNumberAsync(int ruleId, CancellationToken cancellationToken = default);
    }
}
