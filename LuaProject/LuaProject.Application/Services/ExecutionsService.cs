using LuaProject.Application.DTOs;
using LuaProject.Application.RepositoryInterfaces;
using LuaProject.Application.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LuaProject.Application.Services
{
    public class ExecutionsService : IExecutionService
    {
        private readonly IExecutionRepository _executionsRepository;
        private readonly IRulesRepository _rulesRepository;

        public ExecutionsService(IExecutionRepository executionsRepository, IRulesRepository rulesRepository)
        {
            _executionsRepository = executionsRepository;
            _rulesRepository = rulesRepository;
        }

        public async Task<IReadOnlyList<RuleExecutionDto>> GetExecutionsByRuleAsync(int ruleId, int take = 20, CancellationToken cancellationToken = default)
        {
            var existingRule = await _rulesRepository.GetRuleById(ruleId, cancellationToken);
            if(existingRule == null)
            {
                throw new KeyNotFoundException($"Rule with ID {ruleId} not found.");
            }
            var execution = await _executionsRepository.GetExecutionsByRuleAsync(ruleId, take, cancellationToken);

            var executionDtos = new List<RuleExecutionDto>();

            foreach (var exec in execution)
            {
                executionDtos.Add(new RuleExecutionDto
                {
                    ExecutionId = exec.Id,
                    RuleId = exec.RuleId,
                    RuleName = existingRule.Name,
                    Success = exec.Success,
                    Output = exec.OutputJson,
                    ErrorMessage = exec.ErrorMessage,
                    DurationMs = exec.DurationMs
                });
            }

            return executionDtos;
        }
    }
}
