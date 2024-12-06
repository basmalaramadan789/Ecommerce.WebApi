using AutoMapper;
using Ecommerce.Core.Identity;
using Ecommerce.Core.Interfaces;
using Ecommerce.Web.Dtos;
using Ecommerce.Web.Errors;
using Ecommerce.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountController : BaseApiController
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null) return Unauthorized(new ApiResponse(401));

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

        return new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            DisplayName = user.DisplayName
        };
    }


    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Regitser(RegisterDto registerDto)
    {

        var user = new ApplicationUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded) return BadRequest(new ApiResponse(400));

        return new UserDto
        {
            DisplayName = user.DisplayName,
            Token = _tokenService.CreateToken(user),
            Email = user.Email
        };
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {

        var user = await _userManager.FindUserByClaimsPrincipleWithAddress(User);

        return new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            DisplayName = user.DisplayName
        };
    }

    [HttpGet("emailexists")]
    public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    [Authorize]
    [HttpGet("address")]
    public async Task<ActionResult<AddressDto>> GetUserAddress()
    {

        var user = await _userManager.FindUserByClaimsPrincipleWithAddress(User);

        return _mapper.Map<Address, AddressDto>(user.Address);

    }
    [Authorize]
    [HttpPut("address")]
    public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
    {
        var user = await _userManager.FindUserByClaimsPrincipleWithAddress(User);

        user.Address = _mapper.Map<AddressDto, Address>(address);

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded) return Ok(_mapper.Map<AddressDto>(user.Address));
        return BadRequest("Problem updating the user");
    }

}
