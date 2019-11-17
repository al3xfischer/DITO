using Client.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Torrent;
using Grpc.Net.Client;
using System.Threading.Tasks;
using Grpc.Core;
using Server;

namespace Client.Services.Provider
{
    public class SignUpServiceImpl : SignUpService.SignUpServiceClient
    {
        private readonly IFileService fileService;
        private readonly IConfigurationService configurationService;

        public SignUpServiceImpl(IFileService fileService, IConfigurationService configurationService) : base(GrpcChannel.ForAddress("https://" + configurationService.ServerName + ":" + configurationService.ServerPort))
        {
            this.fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            this.configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
        }

        private SentFile CreateSentFile(FileInfo file)
        {
            string hash = string.Empty;

            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(file.FullName))
            {
                var fileHash = md5.ComputeHash(stream);
                hash = BitConverter.ToString(fileHash).Replace("-", "").ToLowerInvariant();
            }

            SentFile sentFile = new SentFile();
            sentFile.FileName = file.Name;
            sentFile.FileHash = hash;
            sentFile.FileSize = file.Length;

            return sentFile;
        }

        public async Task<SignUpReply> SignUp()
        {
            SignUpMessage signUpMessage = new SignUpMessage();

            foreach (var file in this.fileService.GetAllFileEntries())
            {
                var sentFile = CreateSentFile(file);

                signUpMessage.Files.Add(sentFile);
                signUpMessage.ClientPort = configurationService.LocalServerPort;
            }

            return await this.SignUpAsync(signUpMessage).ResponseAsync;
        }

        public void SignUpOneFile(FileInfo file)
        {
            SignUpMessage signUpMessage = new SignUpMessage();

            var sentFile = CreateSentFile(file);

            signUpMessage.Files.Add(sentFile);
            signUpMessage.ClientPort = configurationService.LocalServerPort;

            this.SignUpOneFileAsync(signUpMessage);
        }
    }
}
