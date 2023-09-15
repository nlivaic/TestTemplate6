using System;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace TestTemplate6.Api.Helpers
{
    public class UserIdLoggedInResolver<TSource, TDestination>
        : IValueResolver<TSource, TDestination, Guid>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserIdLoggedInResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid Resolve(TSource source, TDestination destination, Guid destMember, ResolutionContext context) =>
            _httpContextAccessor.HttpContext.User.UserId().Value;
    }
}
