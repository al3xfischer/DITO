﻿using Client.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Torrent;
using Grpc.Net.Client;
using System.Threading.Tasks;
using System.Net.Http;

namespace Client.Services.Provider
{
    public class SignUpServiceImpl : SignUpService.SignUpServiceClient
    {
        private readonly IFileService fileService;

        private readonly IConfigurationService configurationService;

        public SignUpServiceImpl(IFileService fileService, IConfigurationService configurationService) : base(GrpcChannel.ForAddress("https" +
            "://" + configurationService.ServerName + ":" + configurationService.ServerPort))
        {
            this.fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            this.configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
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

        public async Task<SignUpReply> SignUp()
        {
            var signUpMessage = new SignUpMessage();

            foreach (var file in this.fileService.GetAllFileEntries())
            {
                var sentFile = CreateSentFile(file);
                signUpMessage.Files.Add(sentFile);
                signUpMessage.ClientPort = configurationService.LocalServerPort;
            }

            return await this.SignUpAsync(signUpMessage).ResponseAsync;
        }

        public void SignUp(FileInfo file)
        {
            var signUpMessage = new SignUpMessage();
            var sentFile = CreateSentFile(file);

            signUpMessage.Files.Add(sentFile);
            signUpMessage.ClientPort = configurationService.LocalServerPort;

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var httpClient = new HttpClient(handler);
            var channel = GrpcChannel.ForAddress("https://10.0.0.4:5001",new GrpcChannelOptions { HttpClient = httpClient,  });
            var client = new Torrent.SignUpService.SignUpServiceClient(channel);

            var x = client.SignUpOneFile(signUpMessage);

            //var x = this.SignUpOneFile(signUpMessage);
            var z = 1;
        }
    }
}
