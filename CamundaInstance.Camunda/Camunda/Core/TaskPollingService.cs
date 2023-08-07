using Camunda.Api.Client.ExternalTask;
using CamundaInstance.Camunda.Camunda.Contracts;
using Microsoft.Extensions.Options;

namespace CamundaInstance.Camunda.Camunda.Core
{
    public class TaskPollingService : ITaskPollingService
    {
        private Timer _timer;
        private static CamundaSettings _camundaSettings;
        private static IEnumerable<IExternalTaskExecutor> _taskExecutors;
        private readonly IEngineClient _engineClient;
        private readonly IEnumerable<FetchExternalTaskTopic> _topics;


        public TaskPollingService(IOptions<CamundaSettings> camundaSettings,
            IEnumerable<IExternalTaskExecutor> taskExecutors,
            IEngineClient engineClient)
        {
            _camundaSettings = camundaSettings.Value;
            _taskExecutors = taskExecutors;
            _engineClient = engineClient;
            _topics = GetTopicsFromExecutors();
        }

        public void StartPolling()
        {
            _timer = new Timer(_ => DoPolling(), null, _camundaSettings.PollingIntervalInMilliseconds, Timeout.Infinite);
        }


        private async void DoPolling()
        {
            try
            {
                foreach (var topic in _topics)
                {
                    var workerId = Guid.NewGuid().ToString();
                    var fetchExternalTasks = new FetchExternalTasks()
                    {
                        WorkerId = workerId,
                        MaxTasks = _camundaSettings.FetchTaskCount,
                        UsePriority = false,
                        Topics = new List<FetchExternalTaskTopic> { topic }
                    };
                    await _engineClient.ProcessExternalTasks(fetchExternalTasks);
                }

            }
            catch (Exception ex)
            {                
                Console.WriteLine(ex.Message);
            }
            _timer.Change(_camundaSettings.PollingIntervalInMilliseconds, Timeout.Infinite);
        }
        private static IEnumerable<FetchExternalTaskTopic> GetTopicsFromExecutors()
        {
            var t = _taskExecutors.Select(worker => worker.GetType().GetCustomAttributes(typeof(ExternalTaskTopicAttribute), true).FirstOrDefault() as ExternalTaskTopicAttribute);  
            return _taskExecutors.Select(worker => new FetchExternalTaskTopic(
                                        (worker.GetType().GetCustomAttributes(typeof(ExternalTaskTopicAttribute), true).FirstOrDefault() as ExternalTaskTopicAttribute)?.Topic,
                                        _camundaSettings.ExternalTaskLockDuration)
            {}).ToList();
        }

    }
}
