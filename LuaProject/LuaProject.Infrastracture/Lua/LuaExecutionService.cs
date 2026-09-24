using LuaProject.Application.DTOs;
using LuaProject.Application.ServicesInterfaces;
using NLua.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace LuaProject.Infrastracture.Lua
{
    public class LuaExecutionService : ILuaExecutionService
    {
        public LuaExecutionResult Execute(string luaCode, IDictionary<string, object?> inputVariables)
        {
            var sw = Stopwatch.StartNew();
            using var lua = new global::NLua.Lua();
            try
            {
                RestrictLuaEnvironment(lua);

                foreach (var kvp in inputVariables)
                {
                    lua[kvp.Key] = UnwrapJsonElement(kvp.Value);
                }

                var executionTask = Task.Run(() => lua.DoString(luaCode));

                if(!executionTask.Wait(TimeSpan.FromSeconds(3)))
                {
                    sw.Stop();
                    return LuaExecutionResult.Fail("Timeout: lo script Lua ha impiegato troppo tempo per l'esecuzione.", sw.ElapsedMilliseconds);
                }

                sw.Stop();
                var results = executionTask.Result;

                var output = (results is { Length: > 0 }) ? results[0] : null;
                return LuaExecutionResult.Ok(output, sw.ElapsedMilliseconds);
            }
            catch (LuaScriptException ex)
            {
                sw.Stop();
                return LuaExecutionResult.Fail($"Errore nello script Lua: {ex.Message}", sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                return LuaExecutionResult.Fail($"Errore imprevisto: {ex.Message}", sw.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// Unwraps a JsonElement to its corresponding .NET type.
        /// </summary>
        /// <param name="value">The JsonElement to unwrap.</param>
        /// <returns>The unwrapped .NET object.</returns>
        private static object? UnwrapJsonElement(object? value)
        {
            if (value is not JsonElement element)
                return value;

            return element.ValueKind switch
            {
                JsonValueKind.Number => element.GetDouble(),
                JsonValueKind.String => element.GetString(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => element.ToString()
            };
        }

        /// <summary>
        /// Restricts the Lua environment by removing potentially dangerous functions and libraries.
        /// </summary>
        /// <param name="lua">The Lua instance to restrict.</param>
        private static void RestrictLuaEnvironment(global::NLua.Lua lua)
        {
            lua.DoString(@"
                os = nil
                io = nil
                require = nil
                dofile = nil
                loadfile = nil
                load = nil
            ");
        }
    }
}
