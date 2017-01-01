using System.Web.Http;

namespace MIDIator.Web.Controllers
{
    [RoutePrefix("admin")]
    public class AdminController : ApiController
    {
        [HttpPost]
        public void Shutdown()
        {
            AdminManager.Instance.OnShutdown.OnNext(string.Empty);
        }
    }
}
