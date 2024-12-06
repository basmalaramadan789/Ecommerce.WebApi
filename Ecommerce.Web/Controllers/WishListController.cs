using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WishListController : ControllerBase
{
    private readonly IWishListItemService _wishlistService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public WishListController(IWishListItemService wishlistService, IHttpContextAccessor httpContextAccessor)
    {
        _wishlistService = wishlistService;
        _httpContextAccessor = httpContextAccessor;
    }


    [HttpPost("add-to-wishlist")]
    public async Task<IActionResult> AddToWishlist(int productId)
    {
        try
        {
            // 1. Call the service method to add the item to the wishlist
            await _wishlistService.AddItemToWishlistAsync(productId);

            return Ok("Item added to wishlist.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message); // If the product or user doesn't exist
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message); // If the product is already in the wishlist
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("User is not authenticated.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    

    [HttpGet("GetWishlist")]
    public async Task<IActionResult> GetWishlist()
    {
        try
        {
            var wishlistItems = await _wishlistService.GetWishlistItemsAsync();
            return Ok(wishlistItems);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    [HttpDelete("RemoveFromWishlist/{productId}")]
    public async Task<IActionResult> RemoveFromWishlist(int productId)
    {
        try
        {
            await _wishlistService.RemoveItemFromWishlistAsync(productId);
            return Ok(new { Message = "Item removed from wishlist successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpDelete("ClearWishlist")]
    public async Task<IActionResult> ClearWishlist()
    {
        try
        {
            await _wishlistService.ClearWishlistAsync();
            return Ok(new { Message = "Wishlist cleared successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}