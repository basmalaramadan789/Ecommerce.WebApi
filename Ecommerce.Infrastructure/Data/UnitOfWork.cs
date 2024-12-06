using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private Hashtable _repositories;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }
        //public IWishlistRepository WishlistRepository()
        //{
        //    return _repositories.ContainsKey(typeof(IWishlistRepository).Name)
        //        ? (IWishlistRepository)_repositories[typeof(IWishlistRepository).Name]
        //        : new WishlistRepository(_context);
        //}
        public async Task<int> Complete()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the detailed error
                throw new Exception($"Database update error: {ex.InnerException?.Message ?? ex.Message}", ex);
            }
        }



        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<TEntity>)_repositories[type];
        }
    }
}

