using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WindowsIntegrityGuard.UI.Models.Ipc
{
    public sealed class IpcResponse
    {
        public bool Success { get; init; }
        public string Message { get; init; } = string.Empty;
        public JsonElement? Data { get; init; }
        public DateTimeOffset Timestamp { get; init; }
    }
}
