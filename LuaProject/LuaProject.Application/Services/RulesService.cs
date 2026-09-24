using LuaProject.Application.DTOs;
using LuaProject.Application.Models;
using LuaProject.Application.RepositoryInterfaces;
using LuaProject.Application.ServicesInterfaces;
using System.Text.Json;

namespace LuaProject.Application.Services
{
    public class RulesService : IRulesService
    {
        private readonly IRulesRepository _rulesRepository;
        private readonly ILuaExecutionService _luaExecutionService;
        private readonly IExecutionRepository _executionRepository;
        private readonly IRuleVersionRepository _ruleVersionRepository;

        public RulesService(IRulesRepository rulesRepository, ILuaExecutionService luaExecutionService, IExecutionRepository executionRepository, IRuleVersionRepository ruleVersionRepository)
        {
            _rulesRepository = rulesRepository;
            _luaExecutionService = luaExecutionService;
            _executionRepository = executionRepository;
            _ruleVersionRepository = ruleVersionRepository;
        }

        /// <summary>
        /// Get all rules from the repository and map them to RulesRead DTOs.
        /// </summary>
        /// <returns>A collection of RulesRead DTOs.</returns>
        public async Task<IEnumerable<RulesRead>> GetAllRules()
        {
            var rules = await _rulesRepository.GetAllRules();

            return rules.Select(r => new RulesRead
            {
                RulesId = r.Id,
                RulesName = r.Name,
                RulesDescription = r.Description,
                RulesCodiceLua = r.CodiceLua,
                RulesIsActive = r.isActive,
                RulesCreatedAt = r.CreatedAt,
                RulesTriggerType = r.TriggerType
            });
        }

        /// <summary>
        /// Get a rule by its ID from the repository and map it to a RulesRead DTO.
        /// </summary>
        /// <param name="id">The ID of the rule to retrieve.</param>
        /// <returns>The mapped RulesRead DTO if found, otherwise null.</returns>
        public async Task<RulesRead?> GetRuleById(int id, CancellationToken cancellationToken)
        {
            var rule = await _rulesRepository.GetRuleById(id, cancellationToken);
            if (rule == null)
            {
                return null;
            }
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

        /// <summary>
        /// Creates a new rule in the repository based on the provided RulesCreate DTO and returns the created rule as a RulesCreate DTO.
        /// </summary>
        /// <param name="rule">The RulesCreate DTO containing the rule data to create.</param>
        /// <returns>The created rule as a RulesCreate DTO.</returns>
        public async Task<RulesCreate> CreateRule(RulesCreate rule)
        {
            var newRule = new Application.Models.Rule
            {
                Name = rule.RulesName,
                Description = rule.RulesDescription,
                CodiceLua = rule.RulesCodiceLua,
                isActive = rule.RulesIsActive,
                CreatedAt = DateTime.UtcNow,
                TriggerType = rule.RulesTriggerType
            };

            var createdRule = await _rulesRepository.CreateRule(newRule);
            return new RulesCreate
            {
                RulesName = createdRule.Name,
                RulesDescription = createdRule.Description,
                RulesCodiceLua = createdRule.CodiceLua,
                RulesIsActive = createdRule.isActive,
                RulesTriggerType = createdRule.TriggerType
            };
        }

        /// <summary>
        /// Updates an existing rule in the database with versioning.
        /// </summary>
        /// <param name="ruleId">The ID of the rule to update.</param>
        /// <param name="request">The updated rule data.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated rule if successful, otherwise null.</returns>
        public async Task<RulesRead?> UpdateRuleWithVersioningAsync(int ruleId, RulesCreate request, CancellationToken cancellationToken)
        {
            var existingRule = await _rulesRepository.GetRuleById(ruleId, cancellationToken);
            if (existingRule == null)
            {
                return null;
            }

            if (existingRule.CodiceLua != request.RulesCodiceLua)
            {
                var nextVersion = await _ruleVersionRepository.GetLastVersionNumberAsync(ruleId, cancellationToken);

                await _ruleVersionRepository.AddAsync(new RuleVersion
                {
                    RuleId = ruleId,
                    VersionNumber = nextVersion + 1,
                    LuaCode = existingRule.CodiceLua ?? string.Empty,
                    CreatedAt = DateTime.UtcNow
                }, cancellationToken);
            }

            existingRule.Name = request.RulesName;
            existingRule.Description = request.RulesDescription;
            existingRule.CodiceLua = request.RulesCodiceLua;
            existingRule.isActive = request.RulesIsActive;
            existingRule.TriggerType = request.RulesTriggerType;

            await _rulesRepository.UpdateRule(existingRule, cancellationToken);

            return new RulesRead
            {
                RulesId = existingRule.Id,
                RulesName = existingRule.Name,
                RulesDescription = existingRule.Description,
                RulesCodiceLua = existingRule.CodiceLua,
                RulesIsActive = existingRule.isActive,
                RulesCreatedAt = existingRule.CreatedAt,
                RulesTriggerType = existingRule.TriggerType
            };
        }

        /// <summary>
        /// Deletes a rule by its ID from the repository.
        /// </summary>
        /// <param name="id">The ID of the rule to delete.</param>
        /// <returns>The result of the deletion operation.</returns>
        public async Task<bool> DeleteRule(int id)
        {
            var existingRule = await _rulesRepository.GetRuleById(id);
            if (existingRule == null)
            {
                return false;
            }
            return await _rulesRepository.DeleteRule(id);
        }

        /// <summary>
        /// Executes a rule by its ID with the provided inputs, records the execution details, and returns a RuleExecutionDto containing the execution results.
        /// </summary>
        /// <param name="ruleId">The ID of the rule to execute.</param>
        /// <param name="inputs">The inputs to pass to the rule.</param>
        /// <param name="isTest">Indicates if the execution is a test.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<RuleExecutionDto?> RunRuleAsync(int ruleId, IDictionary<string, object?> inputs, bool isTest = false, CancellationToken cancellationToken = default)
        {
            var rule = await _rulesRepository.GetRuleById(ruleId, cancellationToken);
            if (rule == null || !rule.isActive || rule.CodiceLua == null)
            {
                return null;
            }
            var result = _luaExecutionService.Execute(rule.CodiceLua, inputs);

            var executionRecord = new Application.Models.Execution
            {
                RuleId = rule.Id,
                ExecutedAt = DateTime.UtcNow,
                InputJson = JsonSerializer.Serialize(inputs),
                OutputJson = result.Success ? JsonSerializer.Serialize(result.Output) : null,
                Success = result.Success,
                ErrorMessage = result.ErrorMessage,
                DurationMs = result.DurationMs,
                IsTest = isTest
            };

            await _executionRepository.AddAsync(executionRecord, cancellationToken);

            return new RuleExecutionDto
            {
                ExecutionId = executionRecord.Id,
                RuleId = rule.Id,
                RuleName = rule.Name,
                Success = result.Success,
                Output = result.Output,
                ErrorMessage = result.ErrorMessage,
                DurationMs = result.DurationMs
            };
        }
    }
}
