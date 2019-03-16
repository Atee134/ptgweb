using System;

namespace PtgWeb.HubServices
{
    public interface ILocationChangedBroadcasterService
    {
        void StartBroadcasting(Guid sessionId);
    }
}
