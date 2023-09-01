using Camunda.Api.Client.ExternalTask;
using CamundaInstance.Domain.Hasura.GraphQL.Variables;

namespace CamundaInstance.Camunda.Camunda.Contracts
{
    public interface IEngineClient
    {
        Task StartProcessInstance(AdminUser adminUser);
        Task ProcessExternalTasks(FetchExternalTasks fetchExternalTasks);
        Task UnlockExternalTasks(IEnumerable<LockedExternalTask> lockedExternalTasks);

    }
}
