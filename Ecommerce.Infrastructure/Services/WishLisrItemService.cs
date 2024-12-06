using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Services;
public class WishLisrItemService:IWishListItemService
{
    private readonly ApplicationDbContext _appContext;
    private readonly IdentityAppDbContext _identityContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WishLisrItemService(ApplicationDbContext appContext, IdentityAppDbContext identityContext, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _appContext = appContext;
        _identityContext = identityContext;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task AddItemToWishlistAsync(int productId)
    {
        // 1. Get the current logged-in user's ID from IHttpContextAccessor
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the user ID from the claims

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        // 2. Ensure the user exists in the IdentityDbContext
        var user = await _identityContext.Users.FindAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("User does not exist.");
        }

        // 3. Ensure the product exists in the ApplicationDbContext
        var product = await _appContext.Products.FindAsync(productId);
        if (product == null)
        {
            throw new ArgumentException("Product does not exist.");
        }

        // 4. Check if the item already exists in the wishlist
        var existingItem = await _appContext.WishListItems
            .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);
        if (existingItem != null)
        {
            throw new InvalidOperationException("This product is already in the wishlist.");
        }

        // 5. Add the new wishlist item
        var wishlistItem = new WishListItem
        {
            UserId = userId,
            ProductId = productId
        };

        // Use the Unit of Work to add the item
        _unitOfWork.Repository<WishListItem>().Add(wishlistItem);

        // Commit the changes
        await _unitOfWork.Complete();
    }

    public async Task<IReadOnlyList<WishListItem>> GetWishlistItemsAsync()
    {
        //.Get the current logged -in user's ID from IHttpContextAccessor
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

       // 2.Get the wishlist items for the user

       var wishlistItems = await _appContext.WishListItems
           .Where(w => w.UserId == userId)
           .ToListAsync();

        return wishlistItems;

    }

    public async Task RemoveItemFromWishlistAsync(int productId)
    {
        // 1. Get the current logged-in user's ID from IHttpContextAccessor
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        // 2. Find the wishlist item based on UserId and ProductId
        var wishlistItem = await _appContext.WishListItems
            .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

        if (wishlistItem == null)
        {
            throw new ArgumentException("Item not found in the wishlist.");
        }

        // 3. Remove the item
        _appContext.WishListItems.Remove(wishlistItem);

        // 4. Commit the changes to the database
        await _unitOfWork.Complete();
    }

    public async Task ClearWishlistAsync()
    {
        // 1. Get the current logged-in user's ID from IHttpContextAccessor
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        // 2. Fetch all items in the user's wishlist
        var wishlistItems = _appContext.WishListItems
            .Where(w => w.UserId == userId);

        if (!wishlistItems.Any())
        {
            throw new InvalidOperationException("Wishlist is already empty.");
        }

        // 3. Remove all items from the user's wishlist
        _appContext.WishListItems.RemoveRange(wishlistItems);

        // 4. Save changes to the database
        await _unitOfWork.Complete();
    }
}

