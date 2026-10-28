using System.Threading.Tasks;
using System.Windows.Input;
using WindowsIntegrityGuard.UI.Commands;
using WindowsIntegrityGuard.UI.Models.Ipc;
using WindowsIntegrityGuard.UI.Services;

namespace WindowsIntegrityGuard.UI.ViewModels
{
    public sealed class DashboardViewModel : BaseViewModel
    {
        private string _serviceStatus = "Verificando...";
        private string _integrityStatus = "Aguardando verificação";
        private string _integrityMessage = "Nenhum resultado disponível.";
        private string _lastScan = "--";
        private string _currentOperation = "Em espera";
        private double? _scanProgress;
        private int? _filesVerified;
        private int? _problemsFound;
        private int? _problemsRepaired;
        private string _lastScanDuration = "--";
        private readonly IIpcClient _ipcClient;

        public DashboardViewModel(IIpcClient ipcClient)
        {
            _ipcClient = ipcClient;
            StartScanCommand = new AsyncRelayCommand(StartScanAsync, CanStartScan);
        }

        public string ServiceStatus
        {
            get => _serviceStatus;
            set
            {
                if (SetProperty(ref _serviceStatus, value))
                {
                    RaiseStartScanCanExecuteChanged();
                }
            }
        }

        public string IntegrityStatus
        {
            get => _integrityStatus;
            set => SetProperty(ref _integrityStatus, value);
        }

        public string IntegrityMessage
        {
            get => _integrityMessage;
            set => SetProperty(ref _integrityMessage, value);
        }

        public string LastScan
        {
            get => _lastScan;
            set => SetProperty(ref _lastScan, value);
        }

        public string CurrentOperation
        {
            get => _currentOperation;
            set
            {
                if (SetProperty(ref _currentOperation, value))
                {
                    RaiseStartScanCanExecuteChanged();
                }
            }
        }

        public double? ScanProgress
        {
            get => _scanProgress;
            set => SetProperty(ref _scanProgress, value);
        }

        public int? FilesVerified
        {
            get => _filesVerified;
            set => SetProperty(ref _filesVerified, value);
        }

        public int? ProblemsFound
        {
            get => _problemsFound;
            set => SetProperty(ref _problemsFound, value);
        }

        public int? ProblemsRepaired
        {
            get => _problemsRepaired;
            set => SetProperty(ref _problemsRepaired, value);
        }

        public string LastScanDuration
        {
            get => _lastScanDuration;
            set => SetProperty(ref _lastScanDuration, value);
        }

        public ICommand StartScanCommand { get; }

        #region Metodos Privados
        private bool CanStartScan()
        {
            return CurrentOperation != "Verificação em andamento" &&
                   CurrentOperation != "Reparação em andamento" &&
                   ServiceStatus != "Service indisponível";
        }

        private async Task StartScanAsync()
        {
            IpcResponse response = await _ipcClient.SendAsync("start-scan");

            if (!response.Success)
            {
                IntegrityMessage = response.Message;
                return;
            }

            CurrentOperation = "Verificação em andamento";
            IntegrityStatus = "Verificando";
            IntegrityMessage = "A verificação foi solicitada ao Service.";
        }

        private void RaiseStartScanCanExecuteChanged()
        {
            if (StartScanCommand is AsyncRelayCommand command)
            {
                command.RaiseCanExecuteChanged();
            }
        }

        #endregion
    }
}
