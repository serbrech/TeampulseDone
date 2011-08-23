using System.Web.Mvc;

namespace TeampulseReporting.Controllers
{
    public class ControllerBase : Controller
    {
        public Data.TeamPulseEntities Context
        {
            get { return MvcApplication.CurrentContext; }
        }
    }
}