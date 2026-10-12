using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsIntegrityGuard.UI.Models.Ipc;

namespace WindowsIntegrityGuard.UI.Services
{
    public interface IIpcClient
    {
        Task<IpcResponse> SendAsync(string command, CancellationToken cancellationToken = default);
    }
}
