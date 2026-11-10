using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WindowsIntegrityGuard.UI.Models.Ipc;
using WindowsIntegrityGuard.UI.Services;

namespace WindowsIntegrityGuard.UI.ViewModels
{
    public sealed class HistoryViewModel : BaseViewModel
    {
        private readonly IIpcClient _ipcClient;
        private string _statusMessage = "Carregando histórico...";

        public ObservableCollection<HistoryItemViewModel> Items { get; } = [];

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public HistoryViewModel(IIpcClient ipcClient)
        {
            _ipcClient = ipcClient;
        }

        public async Task LoadAsync()
        {
            IpcResponse response = await _ipcClient.SendAsync("scan-history");

            if (!response.Success || response.Data is null)
            {
                Items.Clear();
                StatusMessage = "Não foi possível carregar o histórico.";
                return;
            }

            List<ScanHistoryEntry>? history;

            try
            {
                history = response.Data.Value.Deserialize<List<ScanHistoryEntry>>();
            }
            catch (JsonException)
            {
                Items.Clear();
                StatusMessage = "O Service retornou dados inválidos.";
                return;
            }

            Items.Clear();

            if (history is null || history.Count == 0)
            {
                StatusMessage = "Nenhuma verificação registrada.";
                return;
            }

            foreach (ScanHistoryEntry entry in history.OrderByDescending(x => x.FinishedAt))
            {
                Items.Add(new HistoryItemViewModel(entry));
            }

            StatusMessage = $"{Items.Count} verificação(ões) encontrada(s).";
        }       
    }
}
