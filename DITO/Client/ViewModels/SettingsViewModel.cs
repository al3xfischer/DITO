using Client.Command;
using Client.Models;
using Client.Services.Interfaces;
using System;
using System.Windows.Input;

namespace Client.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IConfigurationService configurationService;

        private DitoConfiguration configuration;

        public SettingsViewModel(IConfigurationService configurationService)
        {
            this.configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
            this.configuration = this.configurationService.Configuration.Clone() as DitoConfiguration;

            this.SaveCommand = new RelayCommand(arg =>
            {
                this.configurationService.ServerName = this.configuration.ServerName;
                this.configurationService.ServerPort = this.configuration.ServerPort;
                this.configurationService.MaxBatchSize = this.configuration.MaxBatchSize;

                this.configurationService.Save();
            });
        }

        public string ServerName
        {
            get => this.configuration.ServerName;
            set => this.configuration.ServerName = value;
        }

        public int ServerPort
        {
            get => this.configuration.ServerPort;
            set => this.configuration.ServerPort = value;
        }

        public int MaxBatchSize
        {
            get => this.configuration.MaxBatchSize;
            set => this.configuration.MaxBatchSize = value;
        }

        public int LocalServerPort
        {
            get => this.configuration.LocalServerPort;
            set => this.configuration.LocalServerPort = value;
        }

        public ICommand SaveCommand { get; }
    }
}
