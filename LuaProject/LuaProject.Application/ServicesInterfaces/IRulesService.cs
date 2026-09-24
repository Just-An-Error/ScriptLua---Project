using LuaProject.Application.DTOs;

namespace LuaProject.Application.ServicesInterfaces
{
    public interface IRulesService
    {
        /// <summary>
        /// Gets all rules from the database.
        /// </summary>
        /// <returns>A list of all rules.</returns>
        Task<IEnumerable<RulesRead>> GetAllRules();

        /// <summary>
        /// Gets a rule by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the rule to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The rule if found, otherwise null.</returns>
        Task<RulesRead?> GetRuleById(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new rule in the database.
        /// </summary>
        /// <param name="rule">The rule to create.</param>
        /// <returns>The created rule.</returns>
        Task<RulesCreate> CreateRule(RulesCreate rule);

        /// <summary>
        /// Updates an existing rule in the database with versioning.
        /// </summary>
        /// <param name="ruleId">The ID of the rule to update.</param>
        /// <param name="request">The updated rule data.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated rule if successful, otherwise null.</returns>
        Task<RulesRead?> UpdateRuleWithVersioningAsync(int ruleId, RulesCreate request, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a rule from the database.
        /// </summary>
        /// <param name="id">The ID of the rule to delete.</param>
        /// <returns>The result of the deletion operation.</returns>
        Task<bool> DeleteRule(int id);

        /// <summary>
        /// Runs a rule with the specified inputs.
        /// </summary>
        /// <param name="ruleId">The ID of the rule to run.</param>
        /// <param name="inputs">The inputs to pass to the rule.</param>
        /// <param name="isTest">Indicates whether the rule is being run in test mode.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>The result of the rule execution.</returns>
        Task<RuleExecutionDto?> RunRuleAsync(
        int ruleId,
        IDictionary<string, object?> inputs,
        bool isTest = false,
        CancellationToken cancellationToken = default);

    }
}
