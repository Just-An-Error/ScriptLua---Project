using LuaProject.Application.Models;
using LuaProject.Application.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuaProject.Infrastracture.Repositories
{
    public class RuleVersionRepository : IRuleVersionRepository
    {
        private readonly Data.AppDbContext _context;

        public RuleVersionRepository(Data.AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new rule version to the database and saves changes asynchronously.
        /// </summary>
        /// <param name="version">The rule version to add.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Nothing.</returns>
        public async Task AddAsync(RuleVersion version, CancellationToken cancellationToken = default)
        {
            _context.RuleVersions.Add(version);
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Gets all versions of a rule by its ID, ordered by version number in descending order.
        /// </summary>
        /// <param name="ruleId">The ID of the rule for which to get versions.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of all versions of the rule, ordered by version number in descending order.</returns>
        public async Task<IReadOnlyList<RuleVersion>> GetByRuleIdAsync(int ruleId, CancellationToken cancellationToken = default)
        {
            return await _context.RuleVersions
                .Where(rv => rv.RuleId == ruleId)
                .OrderByDescending(rv => rv.VersionNumber)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets the last version number for a given rule ID. If no versions exist, returns 0.
        /// </summary>
        /// <param name="ruleId">The ID of the rule for which to get the last version number.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The last version number for the given rule ID, or 0 if no versions exist.</returns>
        public async Task<int> GetLastVersionNumberAsync(int ruleId, CancellationToken cancellationToken = default)
        {
            var lastVersion = await _context.RuleVersions
                .Where(rv => rv.RuleId == ruleId)
                .OrderByDescending(rv => rv.VersionNumber)
                .Select(v => (int?)v.VersionNumber)
                .FirstOrDefaultAsync(cancellationToken);

            return lastVersion ?? 0;
        }
    }
}
