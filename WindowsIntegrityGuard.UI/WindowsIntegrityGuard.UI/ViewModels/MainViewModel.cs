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
        private string _serviceStatus = "Verificando serviço...";

        public string ServiceStatus
        {
            get => _serviceStatus;
            set => SetProperty(ref _serviceStatus, value);
        }

        public MainViewModel(IIpcClient ipcClient)
        {
            _ipcClient = ipcClient;
        }

        public async Task InitializeAsync()
        {
            IpcResponse pingResponse = await _ipcClient.SendAsync("ping");

            if (!pingResponse.Success)
            {
                ServiceStatus = "Service indisponível";
                return;
            }

            IpcResponse statusResponse = await _ipcClient.SendAsync("status");

            if (!statusResponse.Success || statusResponse.Data is null)
            {
                ServiceStatus = "Não foi possível obter o status";
                return;
            }

            ServiceStatusSnapshot? snapshot = statusResponse.Data.Value.Deserialize<ServiceStatusSnapshot>();

            if (snapshot is null)
            {
                ServiceStatus = "Status inválido";
                return;
            }

            ServiceStatus = GetStatusName(snapshot.Status);
        }

        private static string GetStatusName(int status)
        {
            return status switch
            {
                0 => "Idle",
                1 => "Scanning",
                2 => "Repairing",
                3 => "Completed",
                4 => "Failed",
                _ => "Unknown"
            };
        }
    }
}
