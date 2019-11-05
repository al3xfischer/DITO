using System.Windows;
using Client.DI;
using Client.ViewModels;
using Client.Views;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Reply { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = Container.Resolve<MainViewModel>();
        }

        private void Menu_Settings_Click(object sender, RoutedEventArgs e)
        {
           new Settings().Show();
        }

        private void Menu_File_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

