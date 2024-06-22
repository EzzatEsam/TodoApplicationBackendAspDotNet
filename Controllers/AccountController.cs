using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoProj.Auth;

namespace TodoProj.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserAccountController : ControllerBase
{
    private readonly UserManager<CustomUser> _userManager;

    public UserAccountController(UserManager<CustomUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult> Register(RegisterDTO register)
    {
        var user = new CustomUser
        {
            UserName = register.UserName,
            Email = register.Email,
            FirstName = register.FirstName,
            LastName = register.LastName
        };

        var result = await _userManager.CreateAsync(user, register.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();

    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<LoginTokenDTO>> Login(LoginDTO login)
    {
        var user = await _userManager.FindByEmailAsync(login.Email);

        if (user == null)
        {
            return NotFound("User not found");
        }

        var result = await _userManager.CheckPasswordAsync(user, login.Password);

        if (!result)
        {
            return Unauthorized("Invalid password");
        }

        var jwtSecret = Config.JwtSecret;

        var tokenStr = JwtGenerator.GenerateJwt(user, jwtSecret);

        var token = new LoginTokenDTO
        {
            Token = tokenStr
        };

        return token;

    }

    [Authorize]
    [HttpGet]
    [Route("me")]
    public async Task<ActionResult<UserDTO>> Me()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound("User not found");
        }

        var userDTO = new UserDTO
        {
            UserName = user.UserName!,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName
        };

        return userDTO;
    }

}