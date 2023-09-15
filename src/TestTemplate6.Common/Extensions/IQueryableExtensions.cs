using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace TestTemplate6.Common.Extensions
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Provides an opportunity to project to different types.
        /// If T is an entity (inheriting from BaseEntity of Guid) then pass through.
        /// If T is not an entity (e.g. a view model) then map to T using AutoMapper.
        /// </summary>
        /// <typeparam name="T">Resulting type.</typeparam>
        /// <param name="query">Query to project from.</param>
        /// <param name="provider">Definition of class to project into.</param>
        /// <returns>Query with projection.</returns>
        public static IQueryable<T> Projector<T>(this IQueryable query, IConfigurationProvider provider)
        {
            var type = typeof(T);
            while (type != typeof(object))
            {
                if (type == typeof(Base.BaseEntity<Guid>))
                {
                    return query.Cast<T>();
                }
                type = type.BaseType;
            }
            return query.ProjectTo<T>(provider);
        }
    }
}
