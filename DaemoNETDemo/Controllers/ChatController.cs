using Daemo.SDK;
using DaemoNETDemo.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DaemoNETDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken] // Add this to allow Postman/External POST requests
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DaemoClient _agentClient;

        public ChatController(IHttpContextAccessor httpContextAccessor, DaemoClient agentClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _agentClient = agentClient;
                
        }


        [HttpPost]
        [Route("getchatresponse")]
        public async Task<IActionResult> GetChatResponse([FromBody] ChatRequest request)
        {

            // Here, implement the logic for generating a response.

            var user = _httpContextAccessor.HttpContext?.User;
            var role = user?.FindFirst(ClaimTypes.Role)?.Value;
            var applicationUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            var context = new Dictionary<string, string>
            {

                ["username"] = user?.FindFirst(ClaimTypes.Name)?.Value,
                ["applicationUserId"] = applicationUserId ?? "Unknown"

            };

            var result = await _agentClient.ProcessQueryAsync(request.Message, request.ThreadId, role: role, context: context, analysisMode: false);

            if (!result.Success)
            {
                return StatusCode(500, new { error = "Agent failed to process query.", details = result.ErrorMessage });
            }

            return Ok(result);

        }



    }
}
