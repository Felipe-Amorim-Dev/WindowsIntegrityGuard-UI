using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsIntegrityGuard.UI.Services
{
    public sealed class StartupService : IStartupService
    {
        private const string RegistryPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string ApplicationName = "WindowsIntegrityGuard";

        public bool IsEnabled()
        {
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryPath, false);

            if (key is null)
            {
                return false;
            }

            return key.GetValue(ApplicationName) is not null;
        }

        public void SetEnabled(bool enabled)
        {
            using RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryPath, true);

            if (enabled)
            {
                string? executablePath = Environment.ProcessPath;

                if (string.IsNullOrWhiteSpace(executablePath))
                {
                    throw new InvalidOperationException("Não foi possível localizar o executável da aplicação.");
                }

                key.SetValue(ApplicationName, $"\"{executablePath}\"");
                return;
            }

            key.DeleteValue(ApplicationName, false);
        }
    }
}
