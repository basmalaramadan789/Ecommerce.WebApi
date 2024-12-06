using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Interfaces;
public interface IWishListItemService
{
    Task AddItemToWishlistAsync(int productId);
    Task<IReadOnlyList<WishListItem>> GetWishlistItemsAsync();
    Task RemoveItemFromWishlistAsync(int productId);
    Task ClearWishlistAsync();
}
