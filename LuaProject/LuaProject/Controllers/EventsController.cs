using LuaProject.Application.ServicesInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LuaProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventDispatchService _eventDispatchService;
        private readonly ILogger<EventsController> _logger;

        public EventsController(IEventDispatchService eventDispatchService, ILogger<EventsController> logger)
        {
            _eventDispatchService = eventDispatchService;
            _logger = logger;
        }

        /// <summary>
        /// Represents the request payload for dispatching an event.
        /// </summary>
        /// <param name="EventType">The type of the event to dispatch.</param>
        /// <param name="EventData">The data associated with the event.</param>
        public record EventRequest(string EventType, IDictionary<string, object?> EventData);

        /// <summary>
        /// Dispatches an event based on the provided request payload.
        /// </summary>
        /// <param name="request">The request payload containing the event type and data.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Standard HTTP response.</returns>
        [HttpPost]
        public async Task<IActionResult> DispatchEvent([FromBody] EventRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.EventType) || request.EventData == null)
                {
                    return BadRequest("Invalid request payload.");
                }
                var results = await _eventDispatchService.DispacthAsync(request.EventType, request.EventData, cancellationToken);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while dispatching event.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
