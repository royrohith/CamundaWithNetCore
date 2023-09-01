using Camunda.Api.Client;
using Camunda.Api.Client.ExternalTask;
using CamundaInstance.Camunda.Camunda.Contracts;
using CamundaInstance.Camunda.Camunda.Core;
using CamundaInstance.Domain.Camunda.Core;
using CamundaInstance.Domain.Hasura.Contracts;
using CamundaInstance.Domain.Hasura.GraphQL.Variables;

namespace CamundaInstance.Camunda.Camunda.ExternalTasks
{
    [ExternalTaskTopic("insert-into-db")]
    public class InsertIntoDB : IExternalTaskExecutor
    {
        private readonly IHasuraService _hasuraService;

        public InsertIntoDB(IHasuraService hasuraService)
        {
            _hasuraService = hasuraService;
        }

        public async Task<bool> Execute(LockedExternalTask lockedExternalTask)
        {
            try
            {
                var adminUser = GetAdminUserDetails(lockedExternalTask.Variables);
                var result = await _hasuraService.InsertAdminUser(adminUser);

                await Task.Run(new Action(() => Console.WriteLine("Success !!!")));                
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        private AdminUser GetAdminUserDetails(IDictionary<string, VariableValue> variables)
        {
            var details = variables.ToObjectDictionary();
            return new AdminUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = details["Email"].ToString()
            };
        }
    }
}
