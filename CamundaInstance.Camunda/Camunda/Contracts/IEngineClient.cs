using Camunda.Api.Client.ExternalTask;

namespace CamundaInstance.Camunda.Camunda.Contracts
{
    public interface IEngineClient
    {
        Task StartProcessInstance(bool isDBEntryRequired);
        Task ProcessExternalTasks(FetchExternalTasks fetchExternalTasks);
        Task UnlockExternalTasks(IEnumerable<LockedExternalTask> lockedExternalTasks);

    }
}
