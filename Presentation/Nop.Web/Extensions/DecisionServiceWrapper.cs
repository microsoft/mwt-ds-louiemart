using ClientDecisionService;
using Microsoft.AspNet.SignalR;
using MultiWorldTesting;
using Nop.Web.Hubs;
using System;
using System.Collections.Generic;

namespace Nop.Web.Extensions
{
    public static class DecisionServiceWrapper<TContext>
    {
        public static EpsilonGreedyExplorer<TContext> Explorer { get; set; }
        public static DecisionServiceConfiguration<TContext> Configuration { get; set; }
        public static DecisionService<TContext> Service { get; set; }

        public static void Create(string appId, string appToken, float epsilon, uint numActions, string modelOutputDir)
        {
            if (Explorer == null)
            {
                Explorer = new EpsilonGreedyExplorer<TContext>(new MartPolicy<TContext>(), epsilon, numActions);
            }

            if (Configuration == null)
            {
                Configuration = new DecisionServiceConfiguration<TContext>(appId, appToken, Explorer)
                {
                    PolicyModelOutputDir = modelOutputDir,
                    BatchConfig = new BatchingConfiguration 
                    {
                        MaxDuration = TimeSpan.FromSeconds(5),
                        MaxBufferSizeInBytes = 10,
                        MaxEventCount = 1,
                        MaxUploadQueueCapacity = 1,
                        UploadRetryPolicy = BatchUploadRetryPolicy.Retry
                    }
                };
            }

            if (Service == null)
            {
                Service = new DecisionService<TContext>(Configuration);
            }
        }
    }

    public static class DecisionServiceTrace
    {
        static List<TraceMessage> traceMessageList = new List<TraceMessage>();

        public static List<TraceMessage> TraceMessageList
        {
            get { return DecisionServiceTrace.traceMessageList; }
        }

        public static void Add(TraceMessage trm) 
        {
            traceMessageList.Add(trm);

            IHubContext hub = GlobalHost.ConnectionManager.GetHubContext<TraceHub>();
            hub.Clients.All.addNewMessageToPage(trm.Message);
        }

        public static void Clear()
        {
            traceMessageList.Clear();
        }
    }

    public class TraceMessage
    {
        public string Message { get; set; }
    }

    class MartPolicy<TContext> : IPolicy<TContext>
    {
        public uint ChooseAction(TContext context)
        {
            return 5;
        }
    }
}