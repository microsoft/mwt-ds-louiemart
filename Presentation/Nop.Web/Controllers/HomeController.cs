using Nop.Web.Extensions;
using Nop.Web.Framework.Security;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Nop.Web.Controllers
{
    public partial class HomeController : BasePublicController
    {
        const string appId = "louiemart";
        const string appToken = "c7b77291-f267-43da-8cc3-7df7ec2aeb06";

        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult Index(int? page)
        {
            if (page.HasValue && page.Value == 99) // Reset
            {
                DecisionServiceWrapper<string>.Reset(appToken);
                return RedirectToAction("Index");
            }
            HostingEnvironment.QueueBackgroundWorkItem(token =>
            {
                DecisionServiceWrapper<string>.Create(
                    appId: appId,
                    appToken: appToken,
                    //appId: "rcvtest",
                    //appToken: "c01ff675-5710-4814-a961-d03d2d6bce65",
                    epsilon: .2f, 
                    numActions: 10, 
                    modelOutputDir: HostingEnvironment.MapPath("~/VWModel/")
                );
            });
            return View(page);
        }
    }
}
