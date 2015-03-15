using ClientDecisionService;
using Microsoft.AspNet.SignalR;
using MultiWorldTesting;
using Nop.Core.Caching;
using Nop.Web.Controllers;
using Nop.Web.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Nop.Web.Extensions
{
    public static class DecisionServiceWrapper<TContext>
    {
        static readonly string appId = "louiemart";
        static readonly string appToken = "c7b77291-f267-43da-8cc3-7df7ec2aeb06";

        public static EpsilonGreedyExplorer<TContext> Explorer { get; set; }
        public static DecisionServiceConfiguration<TContext> Configuration { get; set; }
        public static DecisionService<TContext> Service { get; set; }

        public static void Create(float epsilon, uint numActions, string modelOutputDir)
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
                        MaxDuration = TimeSpan.FromSeconds(2),
                        MaxBufferSizeInBytes = 1024,
                        MaxEventCount = 100,
                        MaxUploadQueueCapacity = 4,
                        UploadRetryPolicy = BatchUploadRetryPolicy.Retry
                    }
                };
            }

            if (Service == null)
            {
                Service = new DecisionService<TContext>(Configuration);
            }
        }

        public static void Reset()
        {
            // Clear trace messages
            DecisionServiceTrace.Clear();

            // Reset DecisionService objects
            Explorer = null;
            Configuration = null;
            Service = null;

            // Reset all settings via the command center (storage, metadata, etc...)
            using (var client = new System.Net.Http.HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "token", appToken }
                };
                var content = new System.Net.Http.FormUrlEncodedContent(values);

                var responseTask = client.PostAsync(
                    // TODO: use https
                    "http://mwtds.azurewebsites.net/Application/Reset",
                    content
                );
                responseTask.Wait();

                var response = responseTask.Result;
                if (!response.IsSuccessStatusCode)
                {
                    var t2 = response.Content.ReadAsStringAsync();
                    t2.Wait();
                    System.Diagnostics.Trace.TraceError("Failed to reset application. Result : {0}, Reason: {1}, Details: {2}", 
                        t2.Result, response.ReasonPhrase, response.Headers.ToString());
                }
            }
        }

        public static void ReportRewardForCachedProducts(ICacheManager cacheManager, int explorationJoinKeyIndex = -1)
        {
            if (cacheManager.IsSet(ProductController.JoinKeyCacheKey))
            {
                var explorationKeys = cacheManager.Get<List<string>>(ProductController.JoinKeyCacheKey);
                for (int i = 0; i < explorationKeys.Count; i++)
                {
                    if (i != explorationJoinKeyIndex)
                    {
                        DecisionServiceWrapper<string>.Service.ReportReward(0f, explorationKeys[i]);
                    }
                    else
                    {
                        DecisionServiceWrapper<string>.Service.ReportReward(1f, explorationKeys[i]);
                    }
                }
                Trace.WriteLine("Reported 0 reward for unclicked products.");

                // Clears cache once rewards have been determined.
                cacheManager.Remove(ProductController.JoinKeyCacheKey);
            }
        }
    }

    public static class DecisionServiceTrace
    {
        public static readonly int MaxTraceCount = 1000;

        static List<TraceMessage> traceMessageList = new List<TraceMessage>();

        public static List<TraceMessage> TraceMessageList
        {
            get { return DecisionServiceTrace.traceMessageList; }
        }

        public static void Add(TraceMessage trm) 
        {
            if (traceMessageList.Count >= DecisionServiceTrace.MaxTraceCount)
            {
                traceMessageList.Clear();
                DecisionServiceTrace.Add(new TraceMessage {
                    Message = string.Format("Max # trace messages received : {0}, resetting.", DecisionServiceTrace.MaxTraceCount)
                });
            }
            traceMessageList.Add(trm);

            IHubContext hub = GlobalHost.ConnectionManager.GetHubContext<TraceHub>();
            hub.Clients.All.addNewMessageToPage(trm.Message, trm.TimeStampInMillisecSinceUnixEpoch);
        }

        public static void Clear()
        {
            traceMessageList.Clear();
        }
    }

    public class TraceMessage
    {
        public string Message { get; set; }
        public double TimeStampInMillisecSinceUnixEpoch { get; set; }

        public TraceMessage()
        {
            TimeStampInMillisecSinceUnixEpoch = DateTime.UtcNow
                .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .TotalMilliseconds;
        }
    }

    class MartPolicy<TContext> : IPolicy<TContext>
    {
        public uint ChooseAction(TContext context)
        {
            return 5;
        }
    }
}