using System;

namespace Ptg.Common.Dtos
{
    public class PlayerDto
    {
        public string Name { get; set; }

        public Guid SessionId { get; set; }

        public string SignalRConnectionId { get; set; }

        public LocationDto Location { get; set; }
    }
}
