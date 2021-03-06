﻿using System.ComponentModel.DataAnnotations;

namespace Ptg.Common.Dtos.Request
{
    public class DiamondSquareHeightmapRequestDto
    {
        public int Size { get; set; }

        public float OffsetRange { get; set; }

        [Range(0,1)]
        public float OffsetReductionRate { get; set; }

        public int? Seed { get; set; }
    }
}
