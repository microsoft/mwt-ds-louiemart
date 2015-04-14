using System.Web.Mvc;
using Nop.Web.Framework.Security;
using MultiWorldTesting;
using ClientDecisionService;
using System;

namespace Nop.Web.Controllers
{
    public partial class MwtController : BasePublicController
    {
        public void IndexAsync()
        {
            AsyncManager.OutstandingOperations.Increment();

            var explorer = new EpsilonGreedyExplorer<string>(new MartPolicy(), epsilon: .2f, numActions: 10);

            var serviceConfig = new DecisionServiceConfiguration<string>(
                authorizationToken: "c01ff675-5710-4814-a961-d03d2d6bce65",
                explorer: explorer);

            var service = new DecisionService<string>(serviceConfig);

            var rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                int context = rand.Next(100);
                string uniqueKey = i.ToString();
                service.ChooseAction(uniqueKey, context.ToString());
                service.ReportReward((float)(context % 2), uniqueKey);
            }
            service.Flush();

            AsyncManager.OutstandingOperations.Decrement();
        }

        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult IndexCompleted()
        {
            return Content("Success");
        }
    }

    class MartPolicy : IPolicy<string>
    {
        public uint ChooseAction(string context)
        {
            return 1;
        }
    }
}
