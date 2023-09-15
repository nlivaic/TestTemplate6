using TestTemplate6.Core.Entities;
using TestTemplate6.Core.Interfaces;

namespace TestTemplate6.Data.Repositories
{
    public class FooRepository : Repository<Foo>, IFooRepository
    {
        public FooRepository(TestTemplate6DbContext context)
            : base(context)
        {
        }
    }
}
