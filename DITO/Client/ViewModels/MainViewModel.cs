using Client.Command;
using Client.Services.Interfaces;
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

        public MainViewModel(IFileService fileService)
        {
            this.fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            this.filesFolder = Path.Combine(Directory.GetCurrentDirectory(), "shared");
            this.RegisteredFiles = new ObservableCollection<FileInfo>();
            this.CurrentDownlaods = new ObservableCollection<Task>();

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

                //TODO: Register to Server
            });

            this.DeleteRegisteredFile = new RelayCommand((arg) =>
            {
                if (!(arg is FileInfo)) throw new ArgumentException("The arg is not of type FileInfo");

                var file = arg as FileInfo;
                this.fileService.DeleteFile(file);
                this.fileService.RemoveFileEntry(file);

                this.RegisteredFiles.Remove(file);
                this.FirePropertyChanged(nameof(this.RegisteredFiles));
                //// TODO: RM from server
                
            });

            this.LoadLocalFiles();
        }

        public ICommand RegisterFileCommand { get; private set; }

        public ICommand DeleteRegisteredFile { get; private set; }

        public ObservableCollection<FileInfo> RegisteredFiles { get; private set; }

        public ObservableCollection<Task> CurrentDownlaods { get; private set; }

        private void LoadLocalFiles()
        {
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
