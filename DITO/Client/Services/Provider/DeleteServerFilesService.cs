using Client.Services.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Torrent;

namespace Client.Services.Provider
{
    public class DeleteServerFilesService : DeleteFilesService.DeleteFilesServiceClient
    {
        private readonly IFileService fileService;
        private readonly IConfigurationService configurationService;

        public DeleteServerFilesService(IFileService fileService, IConfigurationService configurationService) : base(GrpcChannel.ForAddress("https://" + configurationService.ServerName + ":" + configurationService.ServerPort))
        {
            this.fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            this.configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
        }

        private DeletionFile CreateDeletionFile(FileInfo file)
        {
            string hash = string.Empty;

            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(file.FullName))
            {
                var fileHash = md5.ComputeHash(stream);
                hash = BitConverter.ToString(fileHash).Replace("-", "").ToLowerInvariant();
            }

            DeletionFile deletionFile = new DeletionFile();

            deletionFile.FileName = file.Name;
            deletionFile.FileHash = hash;
            deletionFile.FileSize = file.Length;

            return deletionFile;
        }

        public void DeleteFiles(ICollection<FileInfo> files)
        {
            DeleteRequest deleteRequest = new DeleteRequest();

            foreach (var file in files)
            {
                deleteRequest.DeletionFiles.Add(CreateDeletionFile(file));
            }

            deleteRequest.ClientPort = this.configurationService.LocalServerPort;

            this.DeleteFilesAsync(deleteRequest);
        }

        public void DeleteOneFile(FileInfo file)
        {
            DeleteRequest deleteRequest = new DeleteRequest();

            deleteRequest.DeletionFiles.Add(CreateDeletionFile(file));
            deleteRequest.ClientPort = this.configurationService.LocalServerPort;

            this.DeleteFilesAsync(deleteRequest);
        }
    }
}
