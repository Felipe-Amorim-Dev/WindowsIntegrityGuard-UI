using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WindowsIntegrityGuard.UI.Models.Ipc;
using WindowsIntegrityGuard.UI.Services;

namespace WindowsIntegrityGuard.UI.Communication
{
    public sealed class NamedPipeClient : IIpcClient
    {
        private const string PipeName = "WindowsIntegrityGuard";
        private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

        public async Task<IpcResponse> SendAsync(string command, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(command);

            using var pipe = new NamedPipeClientStream(".", PipeName, PipeDirection.InOut, PipeOptions.Asynchronous);

            using var timeoutSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutSource.CancelAfter(TimeSpan.FromSeconds(3));

            try
            {
                await pipe.ConnectAsync(timeoutSource.Token);

                using var reader = new StreamReader(pipe, Encoding.UTF8, false, 1024, true);
                using var writer = new StreamWriter(pipe, new UTF8Encoding(false), 1024, true) { AutoFlush = true };

                var request = new IpcRequest
                {
                    Command = command
                };

                string requestJson = JsonSerializer.Serialize(request, _jsonOptions);

                await writer.WriteLineAsync(requestJson);

                string? responseJson = await reader.ReadLineAsync(timeoutSource.Token);

                if (string.IsNullOrWhiteSpace(responseJson))
                {
                    return CreateErrorResponse("O Service não retornou uma resposta.");
                }

                IpcResponse? response = JsonSerializer.Deserialize<IpcResponse>(responseJson, _jsonOptions);

                return response ?? CreateErrorResponse("A resposta recebida é inválida.");
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                return CreateErrorResponse("Tempo limite ao tentar conectar ao Service.");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (IOException)
            {
                return CreateErrorResponse("Não foi possível comunicar com o Windows Integrity Guard Service.");
            }
            catch (JsonException)
            {
                return CreateErrorResponse("O Service retornou uma resposta inválida.");
            }
        }

        private static IpcResponse CreateErrorResponse(string message)
        {
            return new IpcResponse
            {
                Success = false,
                Message = message,
                Timestamp = DateTimeOffset.UtcNow
            };
        }
    }
}
