using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsIntegrityGuard.UI.Services
{
    public interface IStartupService
    {
        bool IsEnabled();
        void SetEnabled(bool enabled);
    }
}
