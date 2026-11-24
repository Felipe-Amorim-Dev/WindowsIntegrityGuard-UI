using System.Windows.Input;
using WindowsIntegrityGuard.UI.Commands;
using WindowsIntegrityGuard.UI.Models.Ipc;
using WindowsIntegrityGuard.UI.Services;

namespace WindowsIntegrityGuard.UI.ViewModels
{
    public sealed class DashboardViewModel : BaseViewModel
    {
        private readonly IIpcClient _ipcClient;
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
        private string _heroTitle = "Integridade indisponível";
        private string _heroSubtitle = "Não foi possível conectar ao Windows Integrity Guard Service.";
        private string _heroLabel = "ATENÇÃO NECESSÁRIA";
        private string _heroIcon = "\uE814";
        private string _heroAccent = "#FF707C";
        private string _heroBackground = "#251722";
        private string _serviceBadgeBackground = "#2B1821";
        private string _serviceBadgeBorder = "#65303D";
        private string _serviceBadgeForeground = "#FF7C87";
        private string _primaryActionText = "Verificar agora";
        private string _reconnectText = "Reconectar ao serviço";
        private bool _serviceAvailable;
        private bool _isScanning;
        private bool _isReconnecting;

        public DashboardViewModel(IIpcClient ipcClient)
        {
            _ipcClient = ipcClient;
            StartScanCommand = new AsyncRelayCommand(StartScanAsync, CanStartScan);
            ReconnectCommand = new AsyncRelayCommand(ReconnectAsync);
        }

        public string ServiceStatus { get => _serviceStatus; set { if (SetProperty(ref _serviceStatus, value)) { RaiseStartScanCanExecuteChanged(); } } }
        public string IntegrityStatus { get => _integrityStatus; set => SetProperty(ref _integrityStatus, value); }
        public string IntegrityMessage { get => _integrityMessage; set => SetProperty(ref _integrityMessage, value); }
        public string LastScan { get => _lastScan; set => SetProperty(ref _lastScan, value); }
        public string CurrentOperation { get => _currentOperation; set { if (SetProperty(ref _currentOperation, value)) { RaiseStartScanCanExecuteChanged(); } } }
        public double? ScanProgress { get => _scanProgress; set => SetProperty(ref _scanProgress, value); }
        public int? FilesVerified { get => _filesVerified; set => SetProperty(ref _filesVerified, value); }
        public int? ProblemsFound { get => _problemsFound; set => SetProperty(ref _problemsFound, value); }
        public int? ProblemsRepaired { get => _problemsRepaired; set => SetProperty(ref _problemsRepaired, value); }
        public string LastScanDuration { get => _lastScanDuration; set => SetProperty(ref _lastScanDuration, value); }
        public string HeroTitle { get => _heroTitle; set => SetProperty(ref _heroTitle, value); }
        public string HeroSubtitle { get => _heroSubtitle; set => SetProperty(ref _heroSubtitle, value); }
        public string HeroLabel { get => _heroLabel; set => SetProperty(ref _heroLabel, value); }
        public string HeroIcon { get => _heroIcon; set => SetProperty(ref _heroIcon, value); }
        public string HeroAccent { get => _heroAccent; set => SetProperty(ref _heroAccent, value); }
        public string HeroBackground { get => _heroBackground; set => SetProperty(ref _heroBackground, value); }
        public string ServiceBadgeBackground { get => _serviceBadgeBackground; set => SetProperty(ref _serviceBadgeBackground, value); }
        public string ServiceBadgeBorder { get => _serviceBadgeBorder; set => SetProperty(ref _serviceBadgeBorder, value); }
        public string ServiceBadgeForeground { get => _serviceBadgeForeground; set => SetProperty(ref _serviceBadgeForeground, value); }
        public string PrimaryActionText { get => _primaryActionText; set => SetProperty(ref _primaryActionText, value); }
        public string ReconnectText { get => _reconnectText; set => SetProperty(ref _reconnectText, value); }
        public bool ServiceAvailable { get => _serviceAvailable; set { if (SetProperty(ref _serviceAvailable, value)) { OnPropertyChanged(nameof(ServiceUnavailable)); RaiseStartScanCanExecuteChanged(); } } }
        public bool ServiceUnavailable => !ServiceAvailable;
        public bool IsScanning { get => _isScanning; set { if (SetProperty(ref _isScanning, value)) { RaiseStartScanCanExecuteChanged(); } } }
        public bool IsReconnecting { get => _isReconnecting; set => SetProperty(ref _isReconnecting, value); }

        public ICommand StartScanCommand { get; }
        public ICommand ReconnectCommand { get; }

        public void ApplyVisualState(int serviceStatus, int integrityStatus, bool serviceAvailable)
        {
            ServiceAvailable = serviceAvailable;
            IsScanning = serviceAvailable && integrityStatus == 1;

            if (!serviceAvailable)
            {
                ReconnectText = "Reconectar ao serviço";
                HeroLabel = "ATENÇÃO NECESSÁRIA";
                HeroTitle = "Integridade indisponível";
                HeroSubtitle = "Não foi possível conectar ao Windows Integrity Guard Service. As verificações ficam pausadas até a conexão ser restabelecida.";
                HeroIcon = "\uE814";
                HeroAccent = "#FF707C";
                HeroBackground = "#251722";
                ServiceBadgeBackground = "#2B1821";
                ServiceBadgeBorder = "#65303D";
                ServiceBadgeForeground = "#FF7C87";
                PrimaryActionText = "Verificar agora";
                return;
            }

            ServiceBadgeBackground = "#102D2C";
            ServiceBadgeBorder = "#24504F";
            ServiceBadgeForeground = "#31D6C5";

            switch (integrityStatus)
            {
                case 1:
                    HeroLabel = "VERIFICAÇÃO EM ANDAMENTO";
                    HeroTitle = "Verificando integridade";
                    HeroSubtitle = "O Windows Integrity Guard está analisando arquivos e componentes críticos do sistema.";
                    HeroIcon = "\uE895";
                    HeroAccent = "#31D6C5";
                    HeroBackground = "#101D32";
                    PrimaryActionText = "Verificação em andamento";
                    break;

                case 2:
                    HeroLabel = "SISTEMA PROTEGIDO";
                    HeroTitle = "Integridade preservada";
                    HeroSubtitle = "Nenhuma violação de integridade foi identificada na última verificação.";
                    HeroIcon = "\uE73E";
                    HeroAccent = "#47D6A0";
                    HeroBackground = "#102620";
                    PrimaryActionText = "Verificar novamente";
                    break;

                case 3:
                    HeroLabel = "ATENÇÃO NECESSÁRIA";
                    HeroTitle = "Problemas de integridade detectados";
                    HeroSubtitle = "Foram encontrados problemas que precisam de análise ou reparo.";
                    HeroIcon = "\uE814";
                    HeroAccent = "#FF707C";
                    HeroBackground = "#251722";
                    PrimaryActionText = "Verificar novamente";
                    break;

                case 4:
                    HeroLabel = "RESULTADO INCONCLUSIVO";
                    HeroTitle = "Integridade não determinada";
                    HeroSubtitle = "A verificação terminou, mas não foi possível determinar com segurança o estado do sistema.";
                    HeroIcon = "\uE7BA";
                    HeroAccent = "#F3C443";
                    HeroBackground = "#282516";
                    PrimaryActionText = "Executar novamente";
                    break;

                case 5:
                    HeroLabel = "FALHA NA VERIFICAÇÃO";
                    HeroTitle = "A verificação não foi concluída";
                    HeroSubtitle = "Ocorreu uma falha durante a análise de integridade do Windows.";
                    HeroIcon = "\uEA39";
                    HeroAccent = "#FF707C";
                    HeroBackground = "#251722";
                    PrimaryActionText = "Tentar novamente";
                    break;

                default:
                    HeroLabel = "PRONTO PARA VERIFICAR";
                    HeroTitle = "Integridade aguardando verificação";
                    HeroSubtitle = "Execute uma verificação para avaliar arquivos e componentes críticos do Windows.";
                    HeroIcon = "\uE946";
                    HeroAccent = "#70A1FF";
                    HeroBackground = "#111C35";
                    PrimaryActionText = "Verificar agora";
                    break;
            }
        }

        #region Metodos Privados

        private bool CanStartScan()
        {
            return ServiceAvailable && !IsScanning && CurrentOperation != "Reparação em andamento";
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
            IsScanning = true;
            PrimaryActionText = "Verificação em andamento";
        }

        private async Task ReconnectAsync()
        {
            if (IsReconnecting)
            {
                return;
            }

            IsReconnecting = true;
            ReconnectText = "Reconectando...";

            try
            {
                IpcResponse response = await _ipcClient.SendAsync("ping");

                if (response.Success)
                {
                    ServiceAvailable = true;
                    ReconnectText = "Reconectar ao serviço";
                    IntegrityMessage = "Conexão com o Windows Integrity Guard Service restabelecida.";
                }
                else
                {
                    ServiceAvailable = false;
                    ReconnectText = "Tentar novamente";
                }
            }
            catch
            {
                ServiceAvailable = false;
                ReconnectText = "Tentar novamente";
            }
            finally
            {
                IsReconnecting = false;

                if (!ServiceAvailable)
                {
                    ReconnectText = "Reconectar ao serviço";
                }
            }
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