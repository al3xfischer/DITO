using Client.Command;
using Client.Models;
using Client.Services.Interfaces;
using Client.Services.Provider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
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
                try
                {
                    foreach (var file in this.SelectedFiles)
                    {
                        var servers = file.Clients.Select(host => new Host() { Name = host.Ip, Port = (int)host.Port });
                        var fileEntry = new FileEntry() { Name = file.FileName, Hash = file.FileHash, Length = file.FileSize };
                        var queries = this.clientToClientService.QueryFile(servers, fileEntry);
                        downloadService.AddDownload(queries, fileEntry);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to connect to the server.", "Server Problem", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                }
            });

            this.RequestFilesCommand = new RelayCommand(async (arg) =>
            {
                try
                {
                    var files = await registerFilesService.RequestFiles();
                    this.Files = new ObservableCollection<RequestedTorrentFile>(files);
                    this.FirePropertyChanged(nameof(this.Files));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to connect to the server.", "Server Problem", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                }
            });

            this.RequestFilesCommand.Execute(null);
        }

        public ICommand DownloadCommand { get; set; }
        public ICommand RequestFilesCommand { get; set; }

        public IList<RequestedTorrentFile> SelectedFiles { get; set; }
        public ObservableCollection<RequestedTorrentFile> Files { get; private set; }
    }
}
