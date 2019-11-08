using Client.DI;
using Client.ViewModels;
using System.Windows;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            this.DataContext = Container.Resolve<SettingsViewModel>();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as SettingsViewModel).SaveCommand.Execute(null);
            this.Close();
        }

        private void AbortBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
