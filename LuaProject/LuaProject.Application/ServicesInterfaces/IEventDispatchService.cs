using LuaProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuaProject.Application.ServicesInterfaces
{
    public interface IEventDispatchService
    {
        Task<IReadOnlyList<RuleExecutionDto>> DispacthAsync(string eventType, IDictionary<string, object?> eventData, CancellationToken cancellationToken = default);
    }
}
