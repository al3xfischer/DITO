using Client.Command;
using Client.Models;
using Client.Services.Interfaces;
using Client.Services.Provider;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Client.ViewModels


{
    public class MainViewModel : BaseViewModel
    {
        private readonly string filesFolder;

        private readonly IFileService fileService;

        private readonly RegisterFilesServiceImpl registerFilesService;

        private readonly IClientServerService clientServerService;

        private readonly IDownloadService downloadService;

        public MainViewModel(IFileService fileService, RegisterFilesServiceImpl deleteServerFilesService, IClientServerService clientServerService, IDownloadService downloadService)
        {
            this.fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            this.registerFilesService = deleteServerFilesService ?? throw new ArgumentNullException(nameof(deleteServerFilesService));
            this.filesFolder = Path.Combine(Directory.GetCurrentDirectory(), "shared");
            this.RegisteredFiles = new ObservableCollection<FileInfo>();
            this.CurrentDownloads = new ObservableCollection<Download>();
            this.clientServerService = clientServerService;
            this.downloadService = downloadService ?? throw new ArgumentNullException(nameof(downloadService));

            this.downloadService.DownloadStarted += (sender, args) =>
            {
                var download = new Download
                {
                    FileName = args.FileName,
                    Hash = args.Hash,
                };

                this.CurrentDownloads.Add(download);
                this.FirePropertyChanged(nameof(this.CurrentDownloads));
            };

            this.downloadService.DownloadCompleted += (sender, args) =>
            {
                var download = this.CurrentDownloads.First(d => d.Hash == args.Hash);
                if (download is null) return;

                this.CurrentDownloads.Remove(download);
                download.Success = args.Success;
                download.Completed = true;
                download.CompletedTimeStamp = DateTime.Now;
                this.CurrentDownloads.Add(download);
                fileService.AddFileEntry(args.FileInfo);
                this.RegisteredFiles.Add(args.FileInfo);
                this.registerFilesService.RegisterFile(args.FileInfo);
                this.FirePropertyChanged(nameof(this.RegisteredFiles));
                this.FirePropertyChanged(nameof(this.CurrentDownloads));
            };


            this.RegisterFileCommand = new RelayCommand((arg) =>
            {
                if (!(arg is FileInfo)) throw new ArgumentException("The arg is not of type FileInfo");

                var file = arg as FileInfo;

                if (!file.Exists) return;

                var copiedFile = fileService.CopyToPath(file, filesFolder);


                if (copiedFile is null)
                {
                    MessageBox.Show($"The file {file.Name} was already registered.", "Register File", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                    return;
                }

                fileService.AddFileEntry(copiedFile);
                this.RegisteredFiles.Add(copiedFile);
                this.registerFilesService.RegisterFile(copiedFile);
                this.FirePropertyChanged(nameof(this.RegisteredFiles));
            });

            this.DeleteRegisteredFile = new RelayCommand((arg) =>
            {
                if (!(arg is FileInfo)) throw new ArgumentException("The arg is not of type FileInfo");

                var file = arg as FileInfo;


                this.registerFilesService.DeregisterFile(file);
                this.fileService.DeleteFile(file);
                this.fileService.RemoveFileEntry(file);
                this.RegisteredFiles.Remove(file);
                this.FirePropertyChanged(nameof(this.RegisteredFiles));
            });

            this.RegisterToServerCommand = new RelayCommand((arg) =>
            {
                this.registerFilesService.RegisterMultipleFiles(this.fileService.GetAllFileEntries());
            });

            this.DeregisterFromServerCommand = new RelayCommand((arg) =>
            {
                this.registerFilesService.DeregisterMultipleFiles(this.fileService.GetAllFileEntries());
            });

            this.clientServerService.Start();

            this.LoadLocalFiles();
        }

        public ICommand RegisterFileCommand { get; private set; }

        public ICommand DeleteRegisteredFile { get; private set; }
        public ICommand RegisterToServerCommand { get; private set; }
        public ICommand DeregisterFromServerCommand { get; private set; }

        public ObservableCollection<FileInfo> RegisteredFiles { get; private set; }

        public ObservableCollection<Download> CurrentDownloads { get; private set; }

        private void LoadLocalFiles()
        {
            if (!Directory.Exists(this.filesFolder))
            {
                Directory.CreateDirectory(this.filesFolder);
            }

            var files = Directory.GetFiles(this.filesFolder).Select(f => new FileInfo(f));

            foreach (var file in files)
            {
                this.fileService.AddFileEntry(file);
                this.RegisteredFiles.Add(file);
            }

            this.FirePropertyChanged(nameof(this.RegisteredFiles));
        }
    }
}
