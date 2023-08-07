using Camunda.Api.Client.ExternalTask;
using CamundaInstance.Camunda.Camunda.Contracts;
using CamundaInstance.Camunda.Camunda.Core;

namespace CamundaInstance.Camunda.Camunda.ExternalTasks
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
                Console.WriteLine(ex.ToString());
            }
            return false;
        }
    }
}
