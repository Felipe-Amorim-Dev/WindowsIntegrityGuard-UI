using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsIntegrityGuard.UI.Models;
using WindowsIntegrityGuard.UI.Services;

namespace WindowsIntegrityGuard.UI.ViewModels
{
    public sealed class SettingsViewModel : BaseViewModel
    {
        private readonly ISettingsService _settingsService;
        private bool _startWithWindows;
        private bool _startMinimized;
        private bool _minimizeToTray;
        private bool _showNotifications;
        private bool _isLoaded;
        private readonly IStartupService _startupService;

        public SettingsViewModel(ISettingsService settingsService, IStartupService startupService)
        {
            _settingsService = settingsService;
            _startupService = startupService;
        }

        public bool StartWithWindows
        {
            get => _startWithWindows;
            set
            {
                if (SetProperty(ref _startWithWindows, value))
                {
                    if (_isLoaded)
                    {
                        _startupService.SetEnabled(value);
                    }

                    _ = SaveAsync();
                }
            }
        }

        public bool StartMinimized
        {
            get => _startMinimized;
            set
            {
                if (SetProperty(ref _startMinimized, value))
                {
                    _ = SaveAsync();
                }
            }
        }

        public bool MinimizeToTray
        {
            get => _minimizeToTray;
            set
            {
                if (SetProperty(ref _minimizeToTray, value))
                {
                    _ = SaveAsync();
                }
            }
        }

        public bool ShowNotifications
        {
            get => _showNotifications;
            set
            {
                if (SetProperty(ref _showNotifications, value))
                {
                    _ = SaveAsync();
                }
            }
        }

        public async Task LoadAsync()
        {
            AppSettings settings = await _settingsService.LoadAsync();

            _isLoaded = false;

            StartWithWindows = _startupService.IsEnabled();
            StartMinimized = settings.StartMinimized;
            MinimizeToTray = settings.MinimizeToTray;
            ShowNotifications = settings.ShowNotifications;

            _isLoaded = true;
        }

        private async Task SaveAsync()
        {
            if (!_isLoaded)
            {
                return;
            }

            AppSettings settings = new()
            {
                StartWithWindows = StartWithWindows,
                StartMinimized = StartMinimized,
                MinimizeToTray = MinimizeToTray,
                ShowNotifications = ShowNotifications
            };

            await _settingsService.SaveAsync(settings);
        }
    }
}
