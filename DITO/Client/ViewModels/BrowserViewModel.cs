using Client.Command;
using Client.Models;
using Client.Services.Interfaces;
using Client.Services.Provider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Torrent;

namespace Client.ViewModels
{
    public class BrowserViewModel : BaseViewModel
    {
        private readonly FileRequestServiceImpl fileRequestService;

        private readonly IDownloadService downloadService;

        private readonly ClientToClientService clientToClientService;

        public BrowserViewModel(FileRequestServiceImpl registerFilesService, IDownloadService downloadService, ClientToClientService clientToClientService)
        {
            this.fileRequestService = registerFilesService ?? throw new ArgumentNullException(nameof(registerFilesService));
            this.downloadService = downloadService ?? throw new ArgumentNullException(nameof(downloadService));
            this.clientToClientService = clientToClientService ?? throw new ArgumentNullException(nameof(clientToClientService));

            this.DownloadCommand = new RelayCommand((arg) =>
            {
                foreach (var file in this.SelectedFiles)
                {
                    var servers = file.Clients.Select(host => new Host() { Name = host.Ip, Port = (int)host.Port });
                    var queries = this.clientToClientService.QueryFile(servers, new FileEntry() { Name = file.FileName, Hash = file.FileHash, Length = file.FileSize });
                    downloadService.AddDownload(queries);
                }
            });

            this.RequestFilesCommand = new RelayCommand(async (arg) =>
            {
                var files = await registerFilesService.RequestFiles();
                this.Files = new ObservableCollection<RequestedTorrentFile>(files);
                this.FirePropertyChanged(nameof(this.Files));
            });

            this.RequestFilesCommand.Execute(null);
        }

        public ICommand DownloadCommand { get; set; }
        public ICommand RequestFilesCommand { get; set; }

        public IList<RequestedTorrentFile> SelectedFiles { get; set; }
        public ObservableCollection<RequestedTorrentFile> Files { get; private set; }
    }
}
