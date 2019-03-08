using System;
using System.ComponentModel.DataAnnotations;

namespace Ptg.DataAccess.Models
{
    public class Player
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Guid SessionId { get; set; }

        public string SignalRConnectionId { get; set; }

        public bool Loaded { get; set; }

        public Location Location { get; set; }
    }
}
