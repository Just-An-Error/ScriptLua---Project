using LuaProject.Application.DTOs;
using LuaProject.Application.RepositoryInterfaces;
using LuaProject.Application.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuaProject.Application.Services
{
    public class EventDispatchService : IEventDispatchService
    {
        private readonly IRulesService _rulesService;
        private readonly IRulesRepository _rulesRepository;

        public EventDispatchService(IRulesService rulesService, IRulesRepository rulesRepository)
        {
            _rulesService = rulesService;
            _rulesRepository = rulesRepository;
        }

        /// <summary>
        /// Dispatches an event to all matching rules based on the event type and data.
        /// </summary>
        /// <param name="eventType">The type of the event.</param>
        /// <param name="eventData">The data associated with the event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of rule execution results.</returns>
        public async Task<IReadOnlyList<RuleExecutionDto>> DispacthAsync(string eventType, IDictionary<string, object?> eventData, CancellationToken cancellationToken = default)
        {
            var matchingRules = await _rulesRepository.GetActiveByTriggerAsync(eventType, cancellationToken);

            var results = new List<RuleExecutionDto>();

            foreach (var rule in matchingRules)
            {
                var result = await _rulesService.RunRuleAsync(rule.Id, eventData, isTest: false, cancellationToken);
                if(result != null)
                {
                    results.Add(result);
                }
            }
            return results;
        }
    }
}
