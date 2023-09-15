using System.Threading.Tasks;
using TestTemplate6.Common.Interfaces;

namespace TestTemplate6.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TestTemplate6DbContext _dbContext;

        public UnitOfWork(TestTemplate6DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveAsync()
        {
            if (_dbContext.ChangeTracker.HasChanges())
            {
                return await _dbContext.SaveChangesAsync();
            }
            return 0;
        }
    }
}