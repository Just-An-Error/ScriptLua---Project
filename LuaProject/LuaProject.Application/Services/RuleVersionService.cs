using LuaProject.Application.DTOs;
using LuaProject.Application.Models;
using LuaProject.Application.RepositoryInterfaces;
using LuaProject.Application.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuaProject.Application.Services
{
    public class RuleVersionService : IRuleVersionService
    {
        private readonly IRuleVersionRepository _ruleVersionRepository;
        private readonly IRulesRepository _rulesRepository;

        public RuleVersionService(IRuleVersionRepository ruleVersionRepository, IRulesRepository rulesRepository)
        {
            _ruleVersionRepository = ruleVersionRepository;
            _rulesRepository = rulesRepository;
        }

        /// <summary>
        /// Gets all versions of a rule by its ID.
        /// </summary>
        /// <param name="ruleId">The ID of the rule.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of all versions of the rule.</returns>
        public Task<IReadOnlyList<RuleVersion>> GetAllVersionsAsync(int ruleId, CancellationToken cancellationToken = default)
        {
            return _ruleVersionRepository.GetByRuleIdAsync(ruleId, cancellationToken);
        }

        /// <summary>
        /// Restores a specific version of a rule by its ID.
        /// </summary>
        /// <param name="ruleId">The ID of the rule to restore.</param>
        /// <param name="versionId">The ID of the version to restore.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The restored rule, or null if not found.</returns>
        public async Task<RulesRead?> RestoreVersionAsync(int ruleId, int versionId, CancellationToken cancellationToken = default)
        {
            var rule = await _rulesRepository.GetRuleById(ruleId, cancellationToken);
            if (rule == null)
            {
                return null;
            }
            var versions = await _ruleVersionRepository.GetByRuleIdAsync(ruleId, cancellationToken);
            var versionToRestore = versions.FirstOrDefault(v => v.Id == versionId);
            if (versionToRestore == null)
            {
                return null;
            }

            var nextVersion = await _ruleVersionRepository.GetLastVersionNumberAsync(ruleId, cancellationToken);
            await _ruleVersionRepository.AddAsync(new RuleVersion
            {
                RuleId = ruleId,
                VersionNumber = nextVersion + 1,
                LuaCode = rule.CodiceLua ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            }, cancellationToken);

            rule.CodiceLua = versionToRestore.LuaCode;
            await _rulesRepository.UpdateRule(rule, cancellationToken);
            return new RulesRead
            {
                RulesId = rule.Id,
                RulesName = rule.Name,
                RulesDescription = rule.Description,
                RulesCodiceLua = rule.CodiceLua,
                RulesIsActive = rule.isActive,
                RulesCreatedAt = rule.CreatedAt,
                RulesTriggerType = rule.TriggerType
            };
        }

    }
}
