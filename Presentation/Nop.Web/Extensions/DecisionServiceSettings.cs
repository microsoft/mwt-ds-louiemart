namespace Nop.Web.Extensions
{
    public class DecisionServiceSettings
    {
        readonly float defaultEpsilon = 0.2f;
        readonly int defaultServerObserveDelay = 1000;
        readonly int defaultModelRetrainPeriodicDelay = 5000;

        readonly bool defaultAutoRetrainModel = true;
        readonly bool defaultUseAfxForModelRetrain = true;
        readonly bool defaultUseLatestModel = true;

        // Changes effective on Reset
        public float Epsilon { get; set; }

        // Changes effective at the end of "ModelRetrainPeriodicDelay" period
        public int ServerObserveDelay { get; set; }

        // Changes effective at the end of "ModelRetrainPeriodicDelay" period
        public int ModelRetrainPeriodicDelay { get; set; }

        // Changes effective immediately
        public bool AutoRetrainModel { get; set; }

        // Changes effective immediately
        public bool UseAfxForModelRetrain { get; set; }

        // Changes effective on Reset
        public bool UseLatestModel { get; set; }

        public DecisionServiceSettings()
        {
            Epsilon = defaultEpsilon;
            ServerObserveDelay = defaultServerObserveDelay;
            ModelRetrainPeriodicDelay = defaultModelRetrainPeriodicDelay;
            AutoRetrainModel = defaultAutoRetrainModel;
            UseAfxForModelRetrain = defaultUseAfxForModelRetrain;
            UseLatestModel = defaultUseLatestModel;
        }
    }
}