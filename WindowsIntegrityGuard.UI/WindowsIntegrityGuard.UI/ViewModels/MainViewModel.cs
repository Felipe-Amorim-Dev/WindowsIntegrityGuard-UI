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

            IpcResponse statusResponse = await _ipcClient.SendAsync("status");

            if (!statusResponse.Success || statusResponse.Data is null)
            {
                Dashboard.ServiceStatus = "Status indisponível";
                OnPropertyChanged(nameof(ServiceStatus));
                return;
            }

            ServiceStatusSnapshot? snapshot = statusResponse.Data.Value.Deserialize<ServiceStatusSnapshot>();

            if (snapshot is null)
            {
                Dashboard.ServiceStatus = "Status inválido";
                OnPropertyChanged(nameof(ServiceStatus));
                return;
            }

            Dashboard.ServiceStatus = GetStatusName(snapshot.Status);
            Dashboard.CurrentOperation = GetOperationName(snapshot.Status);

            OnPropertyChanged(nameof(ServiceStatus));
        }

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

        private static string GetOperationName(int status)
        {
            return status switch
            {
                0 => "Em espera",
                1 => "Verificação em andamento",
                2 => "Reparação em andamento",
                3 => "Operação concluída",
                4 => "Falha na operação",
                _ => "Estado desconhecido"
            };
        }
    }
}
