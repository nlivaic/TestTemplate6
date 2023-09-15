using System.Collections.Generic;
using TestTemplate6.Core.Entities;
using TestTemplate6.Data;

namespace TestTemplate6.Api.Tests.Helpers
{
    public static class Seeder
    {
        public static void Seed(this TestTemplate6DbContext ctx)
        {
            ctx.Foos.AddRange(
                new List<Foo>
                {
                    new ("Text 1"),
                    new ("Text 2"),
                    new ("Text 3")
                });
            ctx.SaveChanges();
        }
    }
}
