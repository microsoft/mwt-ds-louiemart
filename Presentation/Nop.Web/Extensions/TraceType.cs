namespace Nop.Web.Extensions
{
    public enum TraceType
    {
        None = 0,
        ClientToServerInteraction,
        ClientToServerReward,
        ServerToStorage,
        AzureMLToStorage,
        StorageToClient
    }

    public static class CurrentTraceType
    {
        public static TraceType Value { get; set; }
    }
}