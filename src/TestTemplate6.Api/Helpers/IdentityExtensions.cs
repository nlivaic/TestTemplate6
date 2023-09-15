using System;
using System.Linq;
using System.Security.Claims;

namespace TestTemplate6.Api.Helpers
{
    public static class IdentityExtensions
    {
        public static string Username(this ClaimsPrincipal user) =>
            user.Claims.SingleOrDefault(c => c.Type == "nickname").Value;

        public static Guid? UserId(this ClaimsPrincipal user)
        {
            var subClaim = user.Claims.SingleOrDefault(c => c.Type == "sub");
            return subClaim == null
                ? (Guid?)null
                : new Guid(subClaim.Value);
        }
    }
}
