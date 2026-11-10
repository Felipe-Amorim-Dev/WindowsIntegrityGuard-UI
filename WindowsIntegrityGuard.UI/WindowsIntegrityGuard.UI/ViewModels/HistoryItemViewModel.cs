using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsIntegrityGuard.UI.Models.Ipc;

namespace WindowsIntegrityGuard.UI.ViewModels
{
    public sealed class HistoryItemViewModel
    {
        public DateTimeOffset StartedAt { get; }
        public DateTimeOffset FinishedAt { get; }
        public TimeSpan Duration { get; }
        public string Status { get; }
        public string Message { get; }
        public int? ExitCode { get; }
        public bool RequiresRepairReview { get; }

        public string Date => StartedAt.ToLocalTime().ToString("dd/MM/yyyy");
        public string Time => StartedAt.ToLocalTime().ToString("HH:mm");
        public string DurationText => Duration.ToString(@"hh\:mm\:ss");

        public HistoryItemViewModel(ScanHistoryEntry entry)
        {
            StartedAt = entry.StartedAt;
            FinishedAt = entry.FinishedAt;
            Duration = entry.Duration;
            Status = GetStatusName(entry.Status);
            Message = entry.Message;
            ExitCode = entry.ExitCode;
            RequiresRepairReview = entry.RequiresRepairReview;
        }

        #region Metodos Privados

        private static string GetStatusName(int status)
        {
            return status switch
            {
                0 => "Não iniciado",
                1 => "Verificando",
                2 => "Íntegro",
                3 => "Corrupção detectada",
                4 => "Inconclusivo",
                5 => "Falha",
                _ => "Desconhecido"
            };
        }

        #endregion
    }
}