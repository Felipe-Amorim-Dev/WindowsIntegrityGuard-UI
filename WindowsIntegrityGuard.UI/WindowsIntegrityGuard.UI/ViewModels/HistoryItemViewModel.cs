using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsIntegrityGuard.UI.ViewModels
{
    public sealed class HistoryItemViewModel
    {
        public DateTimeOffset FinishedAt { get; init; }
        public string Date => FinishedAt.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
        public string Status { get; init; } = string.Empty;
        public string Duration { get; init; } = "--";
        public string Message { get; init; } = string.Empty;
        public string ExitCode { get; init; } = "--";
        public string RepairReview { get; init; } = "Não";
    }
}
