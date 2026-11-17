using System.ComponentModel;
using System.Windows;
using WindowsIntegrityGuard.UI.Communication;
using WindowsIntegrityGuard.UI.Services;
using WindowsIntegrityGuard.UI.ViewModels;

namespace WindowsIntegrityGuard.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;
        private readonly TrayIconService _trayIconService;      
        private bool _isExiting;

        public MainWindow()
        {
            InitializeComponent();

            _trayIconService = new TrayIconService(this, ShutdownApplication);
            _viewModel = new MainViewModel(new NamedPipeClient(), _trayIconService);

            DataContext = _viewModel;

            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        public void ShutdownApplication()
        {
            _isExiting = true;
            _viewModel.Dispose();
            _trayIconService.Dispose();
            System.Windows.Application.Current.Shutdown();
        }

        #region Metodos Privados

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.InitializeAsync();

            bool startedWithWindows = Environment.GetCommandLineArgs().Any(x => string.Equals(x, "--startup", StringComparison.OrdinalIgnoreCase));

            if (startedWithWindows && _viewModel.Settings.StartMinimized)
            {
                Hide();
            }
        }

        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            if (_isExiting)
            {
                return;
            }

            if (_viewModel.Settings.MinimizeToTray)
            {
                e.Cancel = true;
                Hide();
                return;
            }

            ShutdownApplication();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }  

        #endregion
    }
}