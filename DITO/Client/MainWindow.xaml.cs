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
            var vm = Container.Resolve<MainViewModel>();
            this.DataContext = vm;
            vm.RegisterToServerCommand.Execute(null);
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

        private void Menu_Browse_Files_Click(object sender, RoutedEventArgs e)
        {
            new FileBrowser().ShowDialog();
        }

        private void Root_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (this.DataContext as MainViewModel).DeregisterFromServerCommand.Execute(null);
        }
    }
}

