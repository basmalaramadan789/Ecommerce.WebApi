using Ecommerce.Core.Entities;
using Ecommerce.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Interfaces;
public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T> GetEntityWithSpec(ISpecification<T> spec); // Match this exactly with implementation
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    Task<int> CountAsync(ISpecification<T> spec);

    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);

}
