using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using WindowsIntegrityGuard.UI.Models.Ipc;
using WindowsIntegrityGuard.UI.Services;
using WindowsIntegrityGuard.UI.Commands;
using System.Windows.Input;

namespace WindowsIntegrityGuard.UI.ViewModels
{
    public sealed class MainViewModel : BaseViewModel, IDisposable
    {
        private readonly IIpcClient _ipcClient;
        private readonly CancellationTokenSource _monitoringCancellationTokenSource = new();
        private bool _monitoringStarted;
        private object _currentViewModel;

        public DashboardViewModel Dashboard { get; }

        public string ServiceStatus => Dashboard.ServiceStatus;

        public MainViewModel(IIpcClient ipcClient)
        {
            _ipcClient = ipcClient;
            Dashboard = new DashboardViewModel(ipcClient);
            History = new HistoryViewModel(ipcClient);
            _currentViewModel = Dashboard;
            ShowDashboardCommand = new RelayCommand(ShowDashboard);
            ShowHistoryCommand = new AsyncRelayCommand(ShowHistoryAsync);
        }

        public async Task InitializeAsync()
        {
            await RefreshDashboardAsync();

            if (!_monitoringStarted)
            {
                _monitoringStarted = true;
                _ = MonitorDashboardAsync(_monitoringCancellationTokenSource.Token);
            }
        }

        public HistoryViewModel History { get; }

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowHistoryCommand { get; }

        #region Metodos Privados
        private async Task MonitorDashboardAsync(CancellationToken cancellationToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(2));

            try
            {
                while (await timer.WaitForNextTickAsync(cancellationToken))
                {
                    await RefreshDashboardAsync(cancellationToken);
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
            }
        }

        private async Task RefreshDashboardAsync(CancellationToken cancellationToken = default)
        {
            IpcResponse response;

            try
            {
                response = await _ipcClient.SendAsync("dashboard", cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch
            {
                SetServiceUnavailable();
                return;
            }

            if (!response.Success || response.Data is null)
            {
                SetServiceUnavailable();
                return;
            }

            DashboardSnapshot? snapshot;

            try
            {
                snapshot = response.Data.Value.Deserialize<DashboardSnapshot>();
            }
            catch (JsonException)
            {
                SetInvalidData();
                return;
            }

            if (snapshot is null)
            {
                SetInvalidData();
                return;
            }

            ApplyDashboardSnapshot(snapshot);
        }

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
            Dashboard.ScanProgress = null;

            OnPropertyChanged(nameof(ServiceStatus));
        }

        private void SetInvalidData()
        {
            Dashboard.ServiceStatus = "Resposta inválida";
            Dashboard.CurrentOperation = "Não foi possível interpretar os dados do Service.";

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

        private void ShowDashboard()
        {
            CurrentViewModel = Dashboard;
        }

        private async Task ShowHistoryAsync()
        {
            CurrentViewModel = History;
            await History.LoadAsync();
        }
        #endregion

        public void Dispose()
        {
            _monitoringCancellationTokenSource.Cancel();
            _monitoringCancellationTokenSource.Dispose();
        }
    }
}