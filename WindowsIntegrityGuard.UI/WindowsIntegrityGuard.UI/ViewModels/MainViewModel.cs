using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using WindowsIntegrityGuard.UI.Models.Ipc;
using WindowsIntegrityGuard.UI.Services;

namespace WindowsIntegrityGuard.UI.ViewModels
{
    public sealed class MainViewModel : BaseViewModel
    {
        private readonly IIpcClient _ipcClient;

        public DashboardViewModel Dashboard { get; } = new();

        public string ServiceStatus => Dashboard.ServiceStatus;

        public MainViewModel(IIpcClient ipcClient)
        {
            _ipcClient = ipcClient;
        }

        public async Task InitializeAsync()
        {
            IpcResponse pingResponse = await _ipcClient.SendAsync("ping");

            if (!pingResponse.Success)
            {
                Dashboard.ServiceStatus = "Service indisponível";
                Dashboard.CurrentOperation = "Indisponível";
                OnPropertyChanged(nameof(ServiceStatus));
                return;
            }

            IpcResponse dashboardResponse = await _ipcClient.SendAsync("dashboard");

            if (!dashboardResponse.Success || dashboardResponse.Data is null)
            {
                Dashboard.ServiceStatus = "Status indisponível";
                OnPropertyChanged(nameof(ServiceStatus));
                return;
            }

            DashboardSnapshot? snapshot = dashboardResponse.Data.Value.Deserialize<DashboardSnapshot>();

            if (snapshot is null)
            {
                Dashboard.ServiceStatus = "Dados inválidos";
                OnPropertyChanged(nameof(ServiceStatus));
                return;
            }

            Dashboard.ServiceStatus = GetStatusName(snapshot.ServiceStatus);
            Dashboard.CurrentOperation = snapshot.CurrentOperation;
            Dashboard.IntegrityStatus = GetIntegrityStatusName(snapshot.IntegrityStatus);
            Dashboard.IntegrityMessage = GetIntegrityMessage(snapshot.IntegrityStatus);
            Dashboard.LastScan = snapshot.LastScanAt?.ToLocalTime().ToString("dd/MM/yyyy HH:mm") ?? "--";
            Dashboard.ScanProgress = snapshot.ScanProgress;
            Dashboard.FilesVerified = snapshot.FilesVerified;
            Dashboard.ProblemsFound = snapshot.ProblemsFound;
            Dashboard.ProblemsRepaired = snapshot.ProblemsRepaired;
            Dashboard.LastScanDuration = snapshot.LastScanDuration?.ToString(@"hh\:mm\:ss") ?? "--";

            OnPropertyChanged(nameof(ServiceStatus));
        }

        #region Metodos Privados
        private static string GetStatusName(int status)
        {
            return status switch
            {
                0 => "Em execução",
                1 => "Verificando",
                2 => "Reparando",
                3 => "Em execução",
                4 => "Falha",
                _ => "Desconhecido"
            };
        }

        private static string GetIntegrityStatusName(int status)
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

        private static string GetIntegrityMessage(int status)
        {
            return status switch
            {
                2 => "Nenhuma violação de integridade identificada.",
                3 => "Foram identificados problemas de integridade.",
                4 => "Não foi possível determinar a integridade do sistema.",
                5 => "A verificação apresentou uma falha.",
                _ => "Aguardando uma verificação de integridade."
            };
        }
        #endregion
    }
}
