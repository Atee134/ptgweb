using System;

namespace Ptg.Common.Dtos.Request
{
    public class JoinGameSessionRequestDto
    {
        public Guid SessionId { get; set; }

        public string PlayerName { get; set; }
    }
}
