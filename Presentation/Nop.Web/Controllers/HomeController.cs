using Nop.Web.Extensions;
using Nop.Web.Framework.Security;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Nop.Web.Controllers
{
    public partial class HomeController : BasePublicController
    {
        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult Index(int? page)
        {
            if (page.HasValue && page.Value == 99) // Reset
            {
                DecisionServiceWrapper<object>.Reset();
                return RedirectToAction("Index");
            }
            return View(page);
        }
    }
}
