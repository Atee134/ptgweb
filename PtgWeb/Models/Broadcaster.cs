using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ptg.Services.Models
{
    public class Broadcaster
    {
        public Guid SessionId { get; set; }
        public Task Task { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
    }
}
