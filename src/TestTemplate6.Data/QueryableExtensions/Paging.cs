using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TestTemplate6.Common.Paging;

namespace TestTemplate6.Data.QueryableExtensions
{
    public static class Paging
    {
        /// <summary>
        /// Project list from TSource to TTarget using AutoMapper.
        /// </summary>
        /// <typeparam name="TSource">Source data structure.</typeparam>
        /// <typeparam name="TTarget">Target data structure, usually a Dto.</typeparam>
        /// <param name="query">Query to attach paging to.</param>
        /// <param name="mapper">Automapper instance.</param>
        /// <param name="pageNumber">Which page to project.</param>
        /// <param name="pageSize">Size of the projected page.</param>
        /// <returns>A paged projection of TTarget.</returns>
        public static async Task<PagedList<TTarget>> ApplyPagingAsync<TSource, TTarget>(
            this IQueryable<TSource> query,
            IMapper mapper,
            int pageNumber,
            int pageSize)
        {
            var totalItems = query.Count();
            var elements = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<TTarget>(mapper.ConfigurationProvider)
                .ToListAsync();
            return new PagedList<TTarget>(elements, pageNumber, totalItems, pageSize);
        }

        /// <summary>
        /// Project list from TSource to TTarget.
        /// Provide projection delegate.
        /// </summary>
        /// <typeparam name="TSource">Source data structure.</typeparam>
        /// <typeparam name="TTarget">Target data structure, usually a Dto.</typeparam>
        /// <param name="query">Query to attach paging to.</param>
        /// <param name="projection">A delegate describing the projection.</param>
        /// <param name="pageNumber">Which page to project.</param>
        /// <param name="pageSize">Size of the projected page.</param>
        /// <returns>A paged projection of TTarget.</returns>
        public static async Task<PagedList<TTarget>> ApplyPagingAsync<TSource, TTarget>(
            this IQueryable<TSource> query,
            Expression<Func<TSource, TTarget>> projection,
            int pageNumber,
            int pageSize)
        {
            var totalItems = query.Count();
            var elements = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(projection)
                .ToListAsync();
            return new PagedList<TTarget>(elements, pageNumber, totalItems, pageSize);
        }
    }
}
