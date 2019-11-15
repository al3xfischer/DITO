using System.IO;
using System.Windows;
using Client.DI;
using Client.ViewModels;
using Client.Views;
using Microsoft.Win32;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = Container.Resolve<MainViewModel>();

            this.TestMth();
        }

        private void TestMth()
        {
            //var fileService = Container.Resolve<IFileService>();
            //var file = new FileInfo(@"C:\Users\Alex\Desktop\chat.png");
            //fileService.AddFileEntry(file);


            //var server = Container.Resolve<IClientServerService>();
            //server.Start();
            //var con = Container.Resolve<IConfigurationService>();
            //var clientSer = new ClientToClientService(con);

            //var entry = new FileEntry { Length = 22040, Name = "chat.png" };
            //var hosts = new List<Host> { new Host { Name = "10.0.0.4", Port = 5001 } };
            //var res = await Task.WhenAll(clientSer.QueryFile(hosts, entry));
            //var allData = res.SelectMany(r => r.Payload.ToArray());
            //var files = Container.Resolve<IFileService>();
            //files.SaveFile(allData.ToArray(), @"C:\Users\Alex\Desktop", "chat.png");

            //var fileService = new FileService();
            //var file = new FileInfo(@"C:\Users\Alex\Desktop\chat.png");
            //fileService.AddFileEntry(file);

            //var stored = fileService.GetFileEntry("chat.png");
            //var data = fileService.ReadFile(stored, 0, (int)stored.Length/2);
            //var data1 = fileService.ReadFile(stored, data.Length, (int)stored.Length/2);
            //var all = new List<byte[]> { data, data1 };
            //var total = all.Merge(data.Length);
            //fileService.SaveFile(total, @"C:\Users\Alex\Desktop", "chat_2.png");

        }

        private void Menu_Settings_Click(object sender, RoutedEventArgs e)
        {
            new Settings().ShowDialog();
        }

        private void Menu_File_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Menu_Add_File_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();

            if (ofd.ShowDialog() != true) return;

            var fileinfo = new FileInfo(ofd.FileName);
            (this.DataContext as MainViewModel).RegisterFileCommand.Execute(fileinfo);
        }
    }
}

