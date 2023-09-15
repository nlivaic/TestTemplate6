using System.Threading.Tasks;

namespace TestTemplate6.Common.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveAsync();
    }
}