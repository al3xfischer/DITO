using Client.DI;
using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Torrent;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for FileBrowser.xaml
    /// </summary>
    public partial class FileBrowser : Window
    {
        public FileBrowser()
        {
            InitializeComponent();
            this.DataContext = Container.Resolve<BrowserViewModel>();
        }

        private void lvFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = (sender as ListView)?.SelectedItems;
            var datacontext = (this.DataContext as BrowserViewModel);

            if (datacontext is null) return;

            datacontext.SelectedFiles = selection.Cast<RequestedTorrentFile>().ToList();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as BrowserViewModel).DownloadCommand.Execute(null);
            this.Close();
        }
    }
}
