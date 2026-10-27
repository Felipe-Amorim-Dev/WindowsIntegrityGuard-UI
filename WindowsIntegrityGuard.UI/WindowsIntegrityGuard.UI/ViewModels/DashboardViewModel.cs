using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string ServiceStatus
        {
            get => _serviceStatus;
            set => SetProperty(ref _serviceStatus, value);
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
            set => SetProperty(ref _currentOperation, value);
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
    }
}
