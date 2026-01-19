using DaemoNETDemo.Data;
using DaemoNETDemo.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DaemoNETDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken] // Add this to allow Postman/External POST requests
    public class RegistrationViaHttpController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        

        public RegistrationViaHttpController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            //_roleManager = roleManager;
            //_db = db;
        }

        [HttpPost]
        [Route("registrationViaHttp")]
        public async Task<IActionResult> RegistrationViaAspApp([FromBody] RegistrationViaHttpModel model)
        {
            // 1. API Key Security Check
            if (!Request.Headers.TryGetValue("x-api-key", out var apiKey) || apiKey != "my-x-api-key")
            {
                return Unauthorized("Invalid API key");
            }



            // 2. Lookup existing user by LinkSysID
            var existingUser = _userManager.Users.FirstOrDefault(u => u.Email == model.Email);

            // CASE 1: USER DOES NOT EXISTS
            if (existingUser == null)
            {

                var newUser = new ApplicationUser
                {
                    UserName = model.Email,                    
                    Email = model.Email,
                    
                };

                var createResult = await _userManager.CreateAsync(newUser, model.Password);


                //  Role Assignment
              
                    await _userManager.AddToRoleAsync(newUser, model.Role);
                
            }
            return Ok("User created");
        }
    }
}
