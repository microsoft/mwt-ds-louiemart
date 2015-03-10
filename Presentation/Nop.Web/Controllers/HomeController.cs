﻿using Nop.Web.Extensions;
using Nop.Web.Framework.Security;
using System;
using System.Diagnostics;
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
                // Clear trace messages
                DecisionServiceTrace.Clear();

                // TODO: Reset DecisionService objects (policy, recorder, etc...)
                // TODO: Reset Storage account (delete incomplete storage, complete storage, model store)
                // TODO: Reset web settings if in selected-model-update mode

                return RedirectToAction("Index");
            }
            HostingEnvironment.QueueBackgroundWorkItem(token =>
            {
                DecisionServiceWrapper<string>.Create(
                    appId: "louiemart",
                    appToken: "c7b77291-f267-43da-8cc3-7df7ec2aeb06",
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
