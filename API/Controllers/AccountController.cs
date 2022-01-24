using API.DTOs;
using API.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly TokenService _tokenService;
    private readonly IUserAccessor _userAccessor;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, TokenService tokenService, IUserAccessor userAccessor)
    {
        _userAccessor = userAccessor;
        _tokenService = tokenService;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null) return Unauthorized("Invalid email or password");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded) return Unauthorized("Invalid email or password");

        return CreateUserObject(user);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
        {
            ModelState.AddModelError("email", "Email taken");

            return ValidationProblem();
        }

        if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
        {
            ModelState.AddModelError("username", "Username taken");

            return ValidationProblem();
        }

        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Username
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) return BadRequest("Problem registering user");

        return Ok("User registered");
    }

    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await _userManager.FindByNameAsync(_userAccessor.GetUsername());

        return CreateUserObject(user);
    }

    private UserDto CreateUserObject(AppUser user)
    {
        var userDto = new UserDto
        {
            DisplayName = user.DisplayName,
            Username = user.UserName,
            Token = _tokenService.CreateToken(user),
            Avatar = new Avatar
            {
                AvatarName = user.AvatarName,
                Url = user.AvatarUrl,
            }
        };

        return userDto;
    }
}
