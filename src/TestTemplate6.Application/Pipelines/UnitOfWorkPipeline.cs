using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TestTemplate6.Common.Interfaces;

namespace TestTemplate6.Application.Pipelines
{
    public class UnitOfWorkPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IUnitOfWork _uow;

        public UnitOfWorkPipeline(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();
            await _uow.SaveAsync();
            return response;
        }
    }
}
