using LuaProject.Application.RepositoryInterfaces;
using LuaProject.Infrastracture.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuaProject.Infrastracture.Repositories
{
    public class ExecutionRepository : IExecutionRepository
    {
        private readonly AppDbContext _context;

        public ExecutionRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new execution record to the database.
        /// </summary>
        /// <param name="execution">The execution record to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public async Task AddAsync(Application.Models.Execution execution, CancellationToken cancellationToken = default)
        {
            _context.Executions.Add(execution);
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves a list of execution records associated with a specific rule ID.
        /// </summary>
        /// <param name="id">The ID of the rule for which to retrieve executions.</param>
        /// <param name="take">The number of executions to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of execution records.</returns>
        public async Task<IReadOnlyList<Application.Models.Execution>> GetExecutionsByRuleAsync(int id, int take = 20, CancellationToken cancellationToken = default)
        {
            return await _context.Executions
                .Where(e => e.RuleId == id)
                .OrderByDescending(e => e.ExecutedAt)
                .Take(take)
                .ToListAsync(cancellationToken);
        }
    }
}
