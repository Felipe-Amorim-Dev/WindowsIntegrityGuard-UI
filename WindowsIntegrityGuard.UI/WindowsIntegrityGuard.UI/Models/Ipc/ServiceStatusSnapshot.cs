using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsIntegrityGuard.UI.Models.Ipc
{
    public sealed class ServiceStatusSnapshot
    {
        public int Status { get; init; }
        public DateTimeOffset Timestamp { get; init; }
    }
}
