using Camunda.Api.Client.ExternalTask;

namespace CamundaInstance.Domain.Camunda.Contracts
{
    public interface IExternalTaskExecutor
    {
        Task<bool> Execute(LockedExternalTask lockedExternalTask);
    }
}
