using Camunda.Api.Client.ExternalTask;

namespace CamundaInstance.Camunda.Camunda.Contracts
{
    public interface IExternalTaskExecutor
    {
        Task<bool> Execute(LockedExternalTask lockedExternalTask);

    }
}
