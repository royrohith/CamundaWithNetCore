namespace CamundaInstance.Api.Camunda
{
    public class CamundaSettings
    {
        public string Url { get; set; }
        public int ExternalTaskLockDuration { get; set; }
        public int FetchTaskCount { get; set; }
        public int PollingIntervalInMilliseconds { get; set; }
    }
}
