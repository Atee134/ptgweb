namespace Ptg.Common.Dtos.Signalr
{
    public class MapLoadedMessage
    {
        public string SessionId { get; set; }

        public LocationDto Location { get; set; }
    }
}
