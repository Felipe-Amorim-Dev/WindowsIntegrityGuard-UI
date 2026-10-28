using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsIntegrityGuard.UI.Models
{
    public sealed class AppSettings
    {
        public bool StartWithWindows { get; set; }
        public bool StartMinimized { get; set; }
        public bool MinimizeToTray { get; set; } = true;
        public bool ShowNotifications { get; set; } = true;
    }
}
