using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithRoles.Controllers;

[Authorize(Roles = "User")]
[Route("api/[controller]")]
[ApiController]

public class UserController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("You have accessed the User controller");
    }
   
}
