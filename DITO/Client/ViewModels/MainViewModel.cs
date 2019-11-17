using Client.Command;
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
        private readonly SignUpServiceImpl signUpService;
        private readonly DeleteServerFilesService deleteServerFilesService;

        public MainViewModel(IFileService fileService, SignUpServiceImpl signUpService, DeleteServerFilesService deleteServerFilesService)
        {
            this.fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            this.signUpService = signUpService ?? throw new ArgumentNullException(nameof(signUpService));
            this.deleteServerFilesService = deleteServerFilesService ?? throw new ArgumentNullException(nameof(deleteServerFilesService));
            this.filesFolder = Path.Combine(Directory.GetCurrentDirectory(), "shared");
            this.RegisteredFiles = new ObservableCollection<FileInfo>();
            this.CurrentDownloads = new ObservableCollection<Task>();

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
                this.FirePropertyChanged(nameof(this.RegisteredFiles));

                this.signUpService.SignUpOneFile(copiedFile);
            });

            this.DeleteRegisteredFile = new RelayCommand((arg) =>
            {
                if (!(arg is FileInfo)) throw new ArgumentException("The arg is not of type FileInfo");

                var file = arg as FileInfo;

                this.deleteServerFilesService.DeleteOneFile(file);

                this.fileService.DeleteFile(file);
                this.fileService.RemoveFileEntry(file);

                this.RegisteredFiles.Remove(file);
                this.FirePropertyChanged(nameof(this.RegisteredFiles));
            });

            this.LoadLocalFiles();
            this.GetFileInfosFromServer();
        }

        public ICommand RegisterFileCommand { get; private set; }

        public ICommand DeleteRegisteredFile { get; private set; }

        public ObservableCollection<FileInfo> RegisteredFiles { get; private set; }

        public ObservableCollection<Task> CurrentDownloads { get; private set; }
        
        private void GetFileInfosFromServer()
        {
            this.signUpService.SignUp();
        }

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
