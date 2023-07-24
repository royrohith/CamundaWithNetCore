using CamundaInstance.Domain.Camunda;
using Microsoft.AspNetCore.Mvc;

namespace CamundaWithNet6.Controllers
{
    [Route("/api/CamundaInstance")]
    [ApiController]
    public class CamundaInstanceController : ControllerBase
    {
        private static EngineClient _engineClient;
        public CamundaInstanceController()
        {
            _engineClient = new EngineClient();
        }

        [HttpPost]
        [Route("Initialize")]
        public async Task<ActionResult<bool>> Initialize(bool isDBEntryRequired)
        {
            await _engineClient.StartProcessInstance(isDBEntryRequired);
            return true;
        }
    }
}
