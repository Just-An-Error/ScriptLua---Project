using LuaProject.Infrastracture.Data;
using LuaProject.Application.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using LuaProject.Application.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LuaProject.Infrastracture.Repositories
{
    public class RulesRepository : IRulesRepository
    {
        private readonly AppDbContext _context;

        public RulesRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all rules from the database.
        /// </summary>
        /// <returns>A list of all rules.</returns>
        public async Task<IEnumerable<Rule>> GetAllRules()
        {
            return await _context.Rules.ToListAsync();
        }

        /// <summary>
        /// Get a rule by its ID.
        /// </summary>
        /// <param name="id">The ID of the rule to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The rule if found, otherwise null.</returns>
        public async Task<Rule?> GetRuleById(int id, CancellationToken cancellationToken = default)
        {
            var rule = await _context.Rules.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (rule == null)
            {
                return null;
            }

            return rule;
        }

        /// <summary>
        /// Create a new rule in the database.
        /// </summary>
        /// <param name="rule">The rule to create.</param>
        /// <returns>The created rule.</returns>
        public async Task<Rule> CreateRule(Rule rule)
        {
            _context.Rules.Add(rule);
            await _context.SaveChangesAsync();
            return rule;
        }

        /// <summary>
        /// Update an existing rule in the database.
        /// </summary>
        /// <param name="rule">The rule to update.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated rule.</returns>
        public async Task UpdateRule(Rule rule, CancellationToken cancellationToken = default)
        {
            _context.Rules.Update(rule);
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Delete a rule from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the rule to delete.</param>
        /// <returns>The deleted boolean value.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the rule is not found.</exception>
        public async Task<bool> DeleteRule(int id)
        {
            var rule = await _context.Rules.FindAsync(id);
            if (rule == null)
            {
                return false;
            }
            _context.Rules.Remove(rule);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Get all active rules by trigger type.
        /// </summary>
        /// <param name="triggerType">The type of trigger to filter by.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>IReadOnlyList of active rules.</returns>
        public async Task<IReadOnlyList<Rule>> GetActiveByTriggerAsync(string triggerType, CancellationToken cancellationToken = default)
        {
            var activeRules = await _context.Rules
                .Where(r => r.isActive && r.TriggerType == triggerType)
                .ToListAsync(cancellationToken);
            return activeRules;
        }
    }
}
