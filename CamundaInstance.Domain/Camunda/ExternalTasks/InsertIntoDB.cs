using Camunda.Api.Client.ExternalTask;
using CamundaInstance.Domain.Camunda.Contracts;

namespace CamundaInstance.Domain.Camunda.ExternalTasks
{
    [ExternalTaskTopic("insert-into-db")]
    public class InsertIntoDB : IExternalTaskExecutor
    {
        public async Task<bool> Execute(LockedExternalTask lockedExternalTask)
        {
            try
            {
                await Task.Run(new Action(() => Console.WriteLine("Success !!!")));
                return true;
            }
            catch (Exception ex)
            {
                ex.Data.Add("CustomMessage", "Failed External Task.");
                ex.Data.Add("ExternalTaskId", lockedExternalTask.Id);
            }
            return false;
        }
    }
}
