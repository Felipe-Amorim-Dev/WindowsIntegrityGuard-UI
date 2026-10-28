using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WindowsIntegrityGuard.UI.Commands;

namespace WindowsIntegrityGuard.UI.ViewModels
{
    public sealed class AboutViewModel : BaseViewModel
    {
        private string _serviceStatus = "Verificando...";

        public string ApplicationName => "Windows Integrity Guard";
        public string Version => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Desconhecida";
        public string Description => "Ferramenta para monitoramento, verificação e recuperação da integridade do Windows.";
        public string ServiceName => "Windows Integrity Guard";
        public string License => "MIT";
        public string RepositoryUrl => "https://github.com/Felipe-Amorim-Dev/WindowsIntegrityGuard";
        public ICommand OpenRepositoryCommand { get; }

        public string ServiceStatus
        {
            get => _serviceStatus;
            set => SetProperty(ref _serviceStatus, value);
        }

        public AboutViewModel()
        {
            OpenRepositoryCommand = new RelayCommand(OpenRepository);
        }

        private void OpenRepository()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = RepositoryUrl,
                UseShellExecute = true
            });
        }
    }
}
