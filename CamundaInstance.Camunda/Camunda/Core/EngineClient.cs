using Camunda.Api.Client.ProcessDefinition;
using Camunda.Api.Client;
using CamundaInstance.Camunda.Camunda.Contracts;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Camunda.Api.Client.ExternalTask;
using CamundaInstance.Domain.Hasura.GraphQL.Variables;

namespace CamundaInstance.Camunda.Camunda.Core
{
    public class EngineClient : IEngineClient
    {
        private static CamundaClient _camundaClient;
        private readonly CamundaSettings _camundaSettings;
        private readonly IEnumerable<IExternalTaskExecutor> _externalTaskExecutors;

        public EngineClient(IOptions<CamundaSettings> camundaSettings, 
            IEnumerable<IExternalTaskExecutor> externalTaskExecutors)
        {
            _camundaSettings = camundaSettings.Value;
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_camundaSettings.Url);
            _camundaClient = CamundaClient.Create(httpClient);
            _externalTaskExecutors = externalTaskExecutors;
        }

        public async Task StartProcessInstance(AdminUser adminUser)
        {
            var variables = new Dictionary<string, object>();

            variables.Add("IsDBEntryRequired", adminUser.IsHasuraCallRequired);
            variables.Add("Email", adminUser.Email);

            var startProcessInstance = new StartProcessInstance()
            {
                BusinessKey = Guid.NewGuid().ToString(),
                Variables = ToVariableDictionary(variables)
            };
            await _camundaClient.ProcessDefinitions.ByKey("CamundaExample").StartProcessInstance(startProcessInstance);
        }

        public async Task ProcessExternalTasks(FetchExternalTasks fetchExternalTasks)
        {
            var lockedExternalTasks = new List<LockedExternalTask>();
            try
            {
                lockedExternalTasks = await _camundaClient.ExternalTasks.FetchAndLock(fetchExternalTasks);

                lockedExternalTasks.ForEach(async lockedExternalTask => {
                    if(!await ProcessLockedTask(fetchExternalTasks.WorkerId, lockedExternalTask))
                        await UnlockExternalTasks(lockedExternalTasks);
                });
            }
            catch (Exception ex)
            {
                await UnlockExternalTasks(lockedExternalTasks);
                Console.WriteLine(ex.Message);
            }

        }

        public async Task UnlockExternalTasks(IEnumerable<LockedExternalTask> lockedExternalTasks)
        {
            foreach (var lockedExternalTask in lockedExternalTasks)
            {
                await _camundaClient.ExternalTasks[lockedExternalTask.Id].Unlock();
            }
        }

        private async Task<bool> ProcessLockedTask(string workerId, LockedExternalTask lockedExternalTask)
        {
            var executor = GetExecutor(lockedExternalTask.TopicName);
            var isSuccess = await executor.Execute(lockedExternalTask);

            if (isSuccess)
                await _camundaClient.ExternalTasks[lockedExternalTask.Id].Complete(new CompleteExternalTask() { WorkerId = workerId });
            
            return isSuccess;
        }

        private IExternalTaskExecutor GetExecutor(string topic) 
        {
            return _externalTaskExecutors.First(executor => (executor.GetType().GetCustomAttributes(typeof(ExternalTaskTopicAttribute), true)
                                        .FirstOrDefault() as ExternalTaskTopicAttribute)?.Topic == topic);
        }
        private static Dictionary<string, VariableValue> ToVariableDictionary(IDictionary<string, object> source)
        {
            var result = new Dictionary<string, VariableValue>();
            foreach (KeyValuePair<string, object> item in source)
            {
                var variable = VariableValue.FromObject(item.Value);
                if (variable.Type == VariableType.Object)
                {
                    variable.Value = JsonConvert.SerializeObject(item.Value);
                    variable.Type = VariableType.Json;
                }

                result.Add(item.Key, variable);
            }
            return result;
        }
    }
}
