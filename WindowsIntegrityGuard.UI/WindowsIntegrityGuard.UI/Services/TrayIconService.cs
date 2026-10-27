using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Windows;

namespace WindowsIntegrityGuard.UI.Services
{
    public sealed class TrayIconService : IDisposable
    {
        private readonly NotifyIcon _notifyIcon;
        private readonly Window _mainWindow;
        private readonly Action _exitAction;

        public TrayIconService(Window mainWindow, Action exitAction)
        {
            _mainWindow = mainWindow;
            _exitAction = exitAction;

            var contextMenu = new ContextMenuStrip();

            contextMenu.Items.Add("Abrir", null, (_, _) => ShowWindow());
            contextMenu.Items.Add("Verificar agora", null, (_, _) => { });
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add("Histórico", null, (_, _) => { });
            contextMenu.Items.Add("Configurações", null, (_, _) => { });
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add("Sair", null, (_, _) => ExitApplication());

            _notifyIcon = new NotifyIcon
            {
                Text = "Windows Integrity Guard",
                Icon = SystemIcons.Shield,
                Visible = true,
                ContextMenuStrip = contextMenu
            };

            _notifyIcon.DoubleClick += (_, _) => ShowWindow();
        }

        public void ShowWindow()
        {
            _mainWindow.Show();

            if (_mainWindow.WindowState == WindowState.Minimized)
            {
                _mainWindow.WindowState = WindowState.Normal;
            }

            _mainWindow.Activate();
            _mainWindow.Topmost = true;
            _mainWindow.Topmost = false;
            _mainWindow.Focus();
        }

        public void HideWindow()
        {
            _mainWindow.Hide();
        }

        #region Metodos privados

        private void ExitApplication()
        {
            _exitAction();
        }
        #endregion

        public void Dispose()
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
        }        
    }
}
