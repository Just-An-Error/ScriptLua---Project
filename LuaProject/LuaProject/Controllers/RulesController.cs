using LuaProject.Application.DTOs;
using LuaProject.Application.Models;
using LuaProject.Application.ServicesInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LuaProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RulesController : ControllerBase
    {
        private readonly IRulesService _rulesService;
        private readonly ILuaExecutionService _luaExecutionService;
        private readonly ILogger<RulesController> _logger;

        public RulesController(IRulesService rulesService, ILuaExecutionService luaExecutionService, ILogger<RulesController> logger)
        {
            _rulesService = rulesService;
            _luaExecutionService = luaExecutionService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all rules from the service and returns them as a response.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllRules()
        {
            try
            {
                var rules = await _rulesService.GetAllRules();
                return Ok(rules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the rules.");
                return StatusCode(500, "An error occurred while retrieving the rules.");
            }
        }

        /// <summary>
        /// Gets a specific rule by its ID from the service and returns it as a response.
        /// </summary>
        /// <param name="id">The ID of the rule to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Standard HTTP response with the rule data or an error message.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRuleById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var rule = await _rulesService.GetRuleById(id, cancellationToken);
                if (rule == null)
                {
                    return NotFound();
                }
                return Ok(rule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the rule with ID {id}.");
                return StatusCode(500, "An error occurred while retrieving the rule.");
            }
        }

        /// <summary>
        /// Creates a new rule using the provided data.
        /// </summary>
        /// <param name="rule">The data for the new rule.</param>
        /// <returns>Standard HTTP response with the created rule or an error message.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateRule([FromBody] RulesCreate rule)
        {
            try
            {
                if (rule == null)
                {
                    return BadRequest("The rule data is null.");
                }
                var createdRule = await _rulesService.CreateRule(rule);
                return StatusCode(201, createdRule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the rule.");
                return StatusCode(500, "An error occurred while creating the rule.");
            }
        }

        /// <summary>
        /// Updates an existing rule by its ID using the provided data and returns an appropriate response.
        /// </summary>
        /// <param name="id">The ID of the rule to update.</param>
        /// <param name="rule">The data for the updated rule.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated rule or an error message.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRule(int id, [FromBody] RulesCreate rule, CancellationToken cancellationToken)
        {
            try
            {
                if (rule == null)
                {
                    return BadRequest("The rule data is null.");
                }
                var updatedRule = await _rulesService.UpdateRuleWithVersioningAsync(id, rule, cancellationToken);
                if (updatedRule == null)
                {
                    return NotFound();
                }
                return Ok(updatedRule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the rule with ID {id}.");
                return StatusCode(500, "An error occurred while updating the rule.");
            }
        }

        /// <summary>
        /// Deletes a specific rule by its ID using the service and returns an appropriate response.
        /// </summary>
        /// <param name="id">The ID of the rule to delete.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRule(int id)
        {
            try
            {
                var deleted = await _rulesService.DeleteRule(id);
                if (!deleted)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the rule with ID {id}.");
                return StatusCode(500, "An error occurred while deleting the rule.");
            }
        }

        /// <summary>
        /// Executes a specific rule by its ID with the provided input variables and returns the result.
        /// </summary>
        /// <param name="id">The ID of the rule to execute.</param>
        /// <param name="inputVariables">The input variables to use in the rule execution.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result of the rule execution.</returns>
        [HttpPost("{id}/run")]
        public async Task<IActionResult> RunRule(int id, [FromBody] IDictionary<string, object?> inputVariables, CancellationToken cancellationToken)
        {
            var result = await _rulesService.RunRuleAsync(id, inputVariables, isTest: false, cancellationToken);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost("test")]
        public IActionResult TestLuaScript([FromBody] LuaTestRequestDto request)
        {
            var result = _luaExecutionService.Execute(request.LuaCode, request.TestInputs);
            return Ok(result);
        }
    }
}