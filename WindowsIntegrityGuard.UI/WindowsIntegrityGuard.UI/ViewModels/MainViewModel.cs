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
                SetServiceUnavailable();
                return;
            }

            IpcResponse dashboardResponse = await _ipcClient.SendAsync("dashboard");

            if (!dashboardResponse.Success || dashboardResponse.Data is null)
            {
                Dashboard.ServiceStatus = "Status indisponível";
                Dashboard.CurrentOperation = "Não foi possível obter os dados";
                OnPropertyChanged(nameof(ServiceStatus));
                return;
            }

            DashboardSnapshot? snapshot = dashboardResponse.Data.Value.Deserialize<DashboardSnapshot>();

            if (snapshot is null)
            {
                Dashboard.ServiceStatus = "Dados inválidos";
                Dashboard.CurrentOperation = "Resposta inválida do Service";
                OnPropertyChanged(nameof(ServiceStatus));
                return;
            }

            ApplyDashboardSnapshot(snapshot);
        }

        #region Metodos Privados
        private void ApplyDashboardSnapshot(DashboardSnapshot snapshot)
        {
            Dashboard.ServiceStatus = GetServiceStatusName(snapshot.ServiceStatus);
            Dashboard.IntegrityStatus = GetIntegrityStatusName(snapshot.IntegrityStatus);
            Dashboard.IntegrityMessage = GetIntegrityMessage(snapshot.IntegrityStatus);
            Dashboard.CurrentOperation = string.IsNullOrWhiteSpace(snapshot.CurrentOperation) ? "Em espera" : snapshot.CurrentOperation;
            Dashboard.LastScan = snapshot.LastScanAt?.ToLocalTime().ToString("dd/MM/yyyy HH:mm") ?? "--";
            Dashboard.ScanProgress = snapshot.ScanProgress;
            Dashboard.FilesVerified = snapshot.FilesVerified;
            Dashboard.ProblemsFound = snapshot.ProblemsFound;
            Dashboard.ProblemsRepaired = snapshot.ProblemsRepaired;
            Dashboard.LastScanDuration = snapshot.LastScanDuration?.ToString(@"hh\:mm\:ss") ?? "--";

            OnPropertyChanged(nameof(ServiceStatus));
        }

        private void SetServiceUnavailable()
        {
            Dashboard.ServiceStatus = "Service indisponível";
            Dashboard.IntegrityStatus = "Indisponível";
            Dashboard.IntegrityMessage = "Não foi possível conectar ao Windows Integrity Guard Service.";
            Dashboard.CurrentOperation = "Service indisponível";
            Dashboard.LastScan = "--";
            Dashboard.ScanProgress = null;
            Dashboard.FilesVerified = null;
            Dashboard.ProblemsFound = null;
            Dashboard.ProblemsRepaired = null;
            Dashboard.LastScanDuration = "--";

            OnPropertyChanged(nameof(ServiceStatus));
        }

        private static string GetServiceStatusName(int status)
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
                0 => "Aguardando uma verificação de integridade.",
                1 => "Verificação de integridade em andamento.",
                2 => "Nenhuma violação de integridade identificada.",
                3 => "Foram identificados problemas de integridade.",
                4 => "Não foi possível determinar a integridade do sistema.",
                5 => "A verificação apresentou uma falha.",
                _ => "Estado de integridade desconhecido."
            };
        }
        #endregion
    }
}
