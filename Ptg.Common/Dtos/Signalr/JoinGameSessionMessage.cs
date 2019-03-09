using System;

namespace Ptg.Common.Dtos.Signalr
{
    public class JoinGameSessionMessage
    {
        public string SessionId { get; set; }

        public string PlayerName { get; set; }
    }
}
