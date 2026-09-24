using LuaProject.Application.DTOs;

namespace LuaProject.Application.ServicesInterfaces
{
    public interface ILuaExecutionService
    {
        LuaExecutionResult Execute(string luaCode, IDictionary<string, object?> inputVariables);
    }
}
