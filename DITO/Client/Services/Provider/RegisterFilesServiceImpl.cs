using Client.Services.Interfaces;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Torrent;

namespace Client.Services.Provider
{
    public class RegisterFilesServiceImpl : RegisterFilesService.RegisterFilesServiceClient
    {
        private readonly IFileService fileService;
        private readonly IConfigurationService configurationService;

        public RegisterFilesServiceImpl(IFileService fileService, IConfigurationService configurationService) : base(GrpcChannel.ForAddress("https" +
            "://" + configurationService.ServerName + ":" + configurationService.ServerPort, RegisterFilesServiceImpl.CreateOptions()))
        {
            this.fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            this.configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
        }

        private static GrpcChannelOptions CreateOptions()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var httpClient = new HttpClient(handler);

            return new GrpcChannelOptions { HttpClient = httpClient, };
        }

        private SentFile CreateSentFile(FileInfo file)
        {
            string hash = fileService.GetHash(file);
            var sentFile = new SentFile
            {
                FileName = file.Name,
                FileHash = hash,
                FileSize = file.Length
            };

            return sentFile;
        }

        private RegistrationMessage CreateRegistrationMessage(FileInfo file)
        {
            var registrationMessage = new RegistrationMessage();
            var sentFile = CreateSentFile(file);

            registrationMessage.File = sentFile;
            registrationMessage.ClientPort = configurationService.LocalServerPort;

            return registrationMessage;
        }

        private RegisterMultipleMessage CreateRegisterAllMessage(IEnumerable<FileInfo> files)
        {
            var registrationMessage = new RegisterMultipleMessage();

            registrationMessage.Files.Add(files.Select(f => this.CreateSentFile(f)));
            registrationMessage.ClientPort = this.configurationService.LocalServerPort;

            return registrationMessage;
        }

        public void RegisterMultipleFiles(IEnumerable<FileInfo> files)
        {
            var registrationMessage = this.CreateRegisterAllMessage(files);

            this.RegisterMultipleFilesAsync(registrationMessage);
        }

        public void RegisterFile(FileInfo file)
        {
            var registrationMessage = this.CreateRegistrationMessage(file);

            this.RegisterFileAsync(registrationMessage);
        }

        public void DeregisterFile(FileInfo file)
        {
            var deregistrationMessage = this.CreateRegistrationMessage(file);

            this.DeregisterFileAsync(deregistrationMessage);
        }

        public void DeregisterMultipleFiles(IEnumerable<FileInfo> files)
        {
            var deregistrationMessage = this.CreateRegisterAllMessage(files);

            this.DeregisterMultipleFilesAsync(deregistrationMessage);
        }
    }
}
