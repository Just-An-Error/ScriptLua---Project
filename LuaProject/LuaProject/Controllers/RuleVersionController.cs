using LuaProject.Application.ServicesInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LuaProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RuleVersionController : ControllerBase
    {
        private readonly IRuleVersionService _ruleVersionService;
        private readonly ILogger<RuleVersionController> _logger;

        public RuleVersionController(IRuleVersionService ruleVersionService, ILogger<RuleVersionController> logger)
        {
            _ruleVersionService = ruleVersionService;
            _logger = logger;
        }

        [HttpGet("{ruleId}/versions")]
        public async Task<IActionResult> GetAllVersions(int ruleId, CancellationToken cancellationToken)
        {
            try
            {
                var versions = await _ruleVersionService.GetAllVersionsAsync(ruleId, cancellationToken);
                
                if(versions == null || !versions.Any())
                {
                    return NotFound($"No versions found for rule with ID {ruleId}.");
                }

                return Ok(versions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the rule versions.");
                return StatusCode(500, "An error occurred while retrieving the rule versions.");
            }
        }

        [HttpPost("{ruleId}/versions/{versionId}/restore")]
        public async Task<IActionResult> RestoreVersion(int ruleId, int versionId, CancellationToken cancellationToken)
        {
            try
            {
                var restoredRule = await _ruleVersionService.RestoreVersionAsync(ruleId, versionId, cancellationToken);
                if (restoredRule == null)
                {
                    return NotFound($"No rule found with ID {ruleId} or no version found with ID {versionId}.");
                }

                return Ok(restoredRule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while restoring the rule version.");
                return StatusCode(500, "An error occurred while restoring the rule version.");
            }
        }
    }
}
