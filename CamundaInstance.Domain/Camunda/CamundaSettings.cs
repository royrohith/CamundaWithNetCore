using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamundaInstance.Domain.Camunda
{
    public class CamundaSettings
    {
        public string Url { get; set; }
        public int ExternalTaskLockDuration { get; set; }
        public int FetchTaskCount { get; set; }
        public int PollingIntervalInMilliseconds { get; set; }
    }
}
