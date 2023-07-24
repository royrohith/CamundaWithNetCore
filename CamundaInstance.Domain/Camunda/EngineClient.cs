using Camunda.Api.Client;
using Camunda.Api.Client.ProcessDefinition;
using Newtonsoft.Json;

namespace CamundaInstance.Domain.Camunda
{
    public class EngineClient
    {
        private static CamundaClient _camundaClient;
        public EngineClient(CamundaSettings camundaSettings)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(camundaSettings.Url);
            _camundaClient = CamundaClient.Create(httpClient);

        }
        public async Task StartProcessInstance(bool isDBEntryRequired)
        {
            var variables = new Dictionary<string, object>();
            variables.Add("IsDBEntryRequired", isDBEntryRequired);
            var startProcessInstance = new StartProcessInstance()
            {
                BusinessKey = Guid.NewGuid().ToString(),
                Variables = ToVariableDictionary(variables)
            };
            await _camundaClient.ProcessDefinitions.ByKey("CamundaExample").StartProcessInstance(startProcessInstance);
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
