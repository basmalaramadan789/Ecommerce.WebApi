using AutoMapper;
using Ecommerce.Core.Identity;
using Ecommerce.Core.Interfaces;
using Ecommerce.Web.Dtos;
using Ecommerce.Web.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UserManagementController : BaseApiController
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public UserManagementController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        RoleManager<ApplicationRole> roleManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;  // Make sure this is properly injected
        _mapper = mapper;
        _roleManager= roleManager;

    }


    [HttpPost("assign-role")]
    public async Task<ActionResult> AssignRoleToUser(string email, string role)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return NotFound(new ApiResponse(404, "User not found"));

        var roleExist = await _roleManager.RoleExistsAsync(role);
        if (!roleExist) return BadRequest(new ApiResponse(400, "Role does not exist"));

        var result = await _userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded) return BadRequest(new ApiResponse(400, "Failed to assign role"));

        return Ok(new ApiResponse(200, "Role assigned successfully"));
    }

    [HttpPost("remove-role")]
    public async Task<ActionResult> RemoveRoleFromUser(string email, string role)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return NotFound(new ApiResponse(404, "User not found"));

        var result = await _userManager.RemoveFromRoleAsync(user, role);
        if (!result.Succeeded) return BadRequest(new ApiResponse(400, "Failed to remove role"));

        return Ok(new ApiResponse(200, "Role removed successfully"));
    }


    [HttpPost("lock-user")]
    public async Task<ActionResult> LockUser(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return NotFound(new ApiResponse(404, "User not found"));

        user.LockoutEnd = DateTimeOffset.MaxValue; // Lock the account
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) return BadRequest(new ApiResponse(400, "Failed to lock user"));

        return Ok(new ApiResponse(200, "User account locked"));
    }

    [HttpPost("UnLock-user")]
    public async Task<ActionResult> ActivateUser(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return NotFound(new ApiResponse(404, "User not found"));

        user.LockoutEnd = null; // Unlock the account
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) return BadRequest(new ApiResponse(400, "Failed to activate user"));

        return Ok(new ApiResponse(200, "User account activated"));
    }

    [HttpDelete("delete-user")]
    public async Task<ActionResult> DeleteUser(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return NotFound(new ApiResponse(404, "User not found"));

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded) return BadRequest(new ApiResponse(400, "Failed to delete user"));

        return Ok(new ApiResponse(200, "User deleted successfully"));
    }


    
    [HttpGet("getallusers")]
    public async Task<ActionResult<IEnumerable<UserDetailsDto>>> GetAllUsers()
    {
        var users = await _userManager.Users
            .Include(u => u.Address) // Eagerly load the Address
            .ToListAsync(); // Fetch users with address

        if (users == null || !users.Any())
        {
            return NotFound(new ApiResponse(404, "No users found"));
        }

        var userDetailsDtos = _mapper.Map<List<UserDetailsDto>>(users); // Map users to UserDetailsDto

        return Ok(userDetailsDtos);
    }



    [HttpGet("user-details/{userId}")]
    public async Task<ActionResult> GetUserDetails(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound(new ApiResponse(404, "User not found"));

        return Ok(user);
    }




}
