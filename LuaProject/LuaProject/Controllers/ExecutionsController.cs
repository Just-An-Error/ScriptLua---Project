using LuaProject.Application.ServicesInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LuaProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutionsController : ControllerBase
    {
        private readonly IExecutionService _executionService;
        private readonly ILogger<ExecutionsController> _logger;

        public ExecutionsController(IExecutionService executionService, ILogger<ExecutionsController> logger)
        {
            _executionService = executionService;
            _logger = logger;
        }

        [HttpGet("{ruleId}")]
        public async Task<IActionResult> GetExecutionsByRule(int ruleId, int take = 20, CancellationToken cancellationToken = default)
        {
            try
            {
                var executions = await _executionService.GetExecutionsByRuleAsync(ruleId, take, cancellationToken);
                return Ok(executions);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Rule with ID {ruleId} not found.");
                return NotFound("Rule not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving executions for rule ID {ruleId}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
