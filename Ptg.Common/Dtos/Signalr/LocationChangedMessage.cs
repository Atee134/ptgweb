namespace Ptg.Common.Dtos.Signalr
{
    public class LocationChangedMessage
    {
        public string SessionId { get; set; }

        public LocationDto Location { get; set; }
    }
}
