using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Web.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Ecommerce.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WishListController : ControllerBase
{
    private readonly IWishListItemService _wishlistService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _appContext;
    

    public WishListController(IWishListItemService wishlistService, IHttpContextAccessor httpContextAccessor, ApplicationDbContext appContext)
    {
        _wishlistService = wishlistService;
        _httpContextAccessor = httpContextAccessor;
        _appContext = appContext;
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
        //try
        //{
        //    var wishlistItems = await _wishlistService.GetWishlistItemsAsync();
        //    return Ok(wishlistItems);
        //}
        //catch (Exception ex)
        //{
        //    return BadRequest(new { Message = ex.Message });
        //}
        try
        {
            // Get the current logged-in user's ID from IHttpContextAccessor
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User is not authenticated." });
            }

            // Get the wishlist items for the user, including the product details
            var wishlistItems = await _appContext.WishListItems
                .Where(w => w.UserId == userId)
                .Include(w => w.Product)  
                .ToListAsync();
            
            var wishlistItemsDto = wishlistItems.Select(w => new WishListItemDto
            {
                Id = w.Id,
                ProductName = w.Product.Name,  
                ProductImage = w.Product.PictureUrl,  
                ProductPrice = w.Product.Price  
            }).ToList();

            return Ok(wishlistItemsDto);  // Return the wishlist items with product details (name, image, price)
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