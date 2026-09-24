using LuaProject.Application.Models;

namespace LuaProject.Application.RepositoryInterfaces
{
    public interface IExecutionRepository
    {
        Task AddAsync(Execution execution, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Execution>> GetExecutionsByRuleAsync(int id, int take = 20, CancellationToken cancellationToken = default);
    }
}
