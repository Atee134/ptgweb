using System;

namespace Ptg.Common.Dtos.Request
{
    public class StartGameSesionRequestDto
    {
        public Guid SessionId { get; set; }

        public Guid TerrainDataId { get; set; }
    }
}
