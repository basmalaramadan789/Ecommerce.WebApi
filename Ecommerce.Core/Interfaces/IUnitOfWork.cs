using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Interfaces;
public interface IUnitOfWork:IDisposable
{
    //IWishlistRepository WishlistRepository();

    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;

    Task<int> Complete();
}
