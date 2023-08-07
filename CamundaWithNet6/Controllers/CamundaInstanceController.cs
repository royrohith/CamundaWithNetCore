using CamundaInstance.Camunda.Camunda.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CamundaWithNet6.Controllers
{
    [Route("/api/CamundaInstance")]
    [ApiController]
    public class CamundaInstanceController : ControllerBase
    {
        private readonly IEngineClient _engineClient;
        private readonly ITaskPollingService _taskPollingService;

        public CamundaInstanceController(IEngineClient engineClient,
            ITaskPollingService taskPollingService)
        {
            _engineClient = engineClient;
            _taskPollingService = taskPollingService;
        }

        [HttpPost]
        [Route("StartProcess")]
        public async Task<ActionResult<bool>> Initialize(bool isDBEntryRequired)
        {
            await _engineClient.StartProcessInstance(isDBEntryRequired);
            return true;
        }

        [HttpPost]
        [Route("StartWorker")]
        public ActionResult<bool> StartWorker()
        {
            _taskPollingService.StartPolling();
            return true;
        }
    }
}
