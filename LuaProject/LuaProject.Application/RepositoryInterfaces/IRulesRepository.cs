using LuaProject.Application.Models;

namespace LuaProject.Application.RepositoryInterfaces
{
    public interface IRulesRepository
    {
        /// <summary>
        /// Gets all the rules from the database.
        /// </summary>
        /// <returns>A list of all rules.</returns>
        Task<IEnumerable<Rule>> GetAllRules();

        /// <summary>
        /// Gets a rule by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the rule to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The rule if found, otherwise null.</returns>
        Task<Rule?> GetRuleById(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new rule in the database.
        /// </summary>
        /// <param name="rule">The
        /// rule to create.</param>
        /// <returns>The created rule.</returns>
        Task<Rule> CreateRule(Rule rule);

        /// <summary>
        /// Updates an existing rule in the database.
        /// </summary>
        /// <param name="rule">The rule to update.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated rule.</returns>
        Task UpdateRule(Rule rule, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a rule from the database.
        /// </summary>
        /// <param name="id">The ID of the rule to delete.</param>
        /// <returns>The deleted boolean value.</returns>
        Task<bool> DeleteRule(int id);

        /// <summary>
        /// Gets all active rules by trigger type from the database.
        /// </summary>
        /// <param name="triggerType">The type of trigger to filter by.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>IReadOnlyList of active rules.</returns>
        Task<IReadOnlyList<Rule>> GetActiveByTriggerAsync(string triggerType, CancellationToken cancellationToken = default);
    }
}
