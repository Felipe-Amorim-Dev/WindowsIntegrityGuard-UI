using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsIntegrityGuard.UI.Models.Ipc
{
    public sealed class DashboardSnapshot
    {
        public int ServiceStatus { get; init; }
        public int IntegrityStatus { get; init; }
        public string CurrentOperation { get; init; } = string.Empty;
        public DateTimeOffset? LastScanAt { get; init; }
        public double? ScanProgress { get; init; }
        public int? FilesVerified { get; init; }
        public int? ProblemsFound { get; init; }
        public int? ProblemsRepaired { get; init; }
        public TimeSpan? LastScanDuration { get; init; }
    }
}
