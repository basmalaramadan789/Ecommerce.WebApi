﻿
using System.Linq.Expressions;


namespace Ecommerce.Core.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; } // Correct spelling

    List<Expression<Func<T, object>>> Includes { get; }

    Expression<Func<T, object>> OrderBy { get; }
    Expression<Func<T, object>> OrderByDescending { get; }

    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
}

