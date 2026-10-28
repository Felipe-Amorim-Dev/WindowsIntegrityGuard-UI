using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsIntegrityGuard.UI.Models;

namespace WindowsIntegrityGuard.UI.Services
{
    public interface ISettingsService
    {
        Task<AppSettings> LoadAsync();
        Task SaveAsync(AppSettings settings);
    }
}
