using LuaProject.Application.DTOs;
using LuaProject.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuaProject.Application.ServicesInterfaces
{
    public interface IRuleVersionService
    {
        Task<IReadOnlyList<RuleVersion>> GetAllVersionsAsync(int ruleId, CancellationToken cancellationToken = default);
        Task<RulesRead?> RestoreVersionAsync(int ruleId, int versionid, CancellationToken cancellationToken = default);
    }
}
