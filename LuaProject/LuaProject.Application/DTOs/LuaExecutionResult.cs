namespace LuaProject.Application.DTOs
{
    public class LuaExecutionResult
    {
        public bool Success { get; set; }
        public object? Output { get; set; }
        public string? ErrorMessage { get; set; }
        public long DurationMs { get; set; }

        public static LuaExecutionResult Ok(object? output, long durationMs) => new()
        {
            Success = true,
            Output = output,
            DurationMs = durationMs
        };

        public static LuaExecutionResult Fail(string errorMessage, long durationMs) => new()
        {
            Success = false,
            ErrorMessage = errorMessage,
            DurationMs = durationMs
        };
    }
}
