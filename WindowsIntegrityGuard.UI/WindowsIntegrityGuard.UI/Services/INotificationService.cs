using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsIntegrityGuard.UI.Services
{
    public interface INotificationService
    {
        void ShowInformation(string title, string message);
        void ShowWarning(string title, string message);
        void ShowError(string title, string message);
    }
}
