using Client.Services.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.IO;
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
            string hash = fileService.GetHash(file);
            var deletionFile = new DeletionFile
            {
                FileName = file.Name,
                FileHash = hash,
                FileSize = file.Length
            };

            return deletionFile;
        }

        public void Delete(ICollection<FileInfo> files)
        {
            var deleteRequest = new DeleteRequest();

            foreach (var file in files)
            {
                deleteRequest.DeletionFiles.Add(CreateDeletionFile(file));
            }

            deleteRequest.ClientPort = this.configurationService.LocalServerPort;

            this.DeleteFilesAsync(deleteRequest);
        }

        public void Delete(FileInfo file)
        {
            DeleteRequest deleteRequest = new DeleteRequest();

            deleteRequest.DeletionFiles.Add(CreateDeletionFile(file));
            deleteRequest.ClientPort = this.configurationService.LocalServerPort;

            this.DeleteFilesAsync(deleteRequest);
        }
    }
}
