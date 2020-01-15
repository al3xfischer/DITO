using Client.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Torrent;
using Google.Protobuf;
using System.IO;
using Client.Models;
using System.Linq;

namespace Client.Services.Provider
{
    public class DownloadService : IDownloadService
    {
        public event EventHandler DownloadStarted;

        private readonly IFileService fileService;
        private string filesFolder;

        public DownloadService(IFileService fileService)
        {
            this.fileService = fileService;
            this.filesFolder = Path.Combine(Directory.GetCurrentDirectory(), "shared");
        }

        public async void AddDownload(IEnumerable<Task<FileReply>> downloads, FileEntry file)
        {
            if (downloads is null)
            {
                throw new ArgumentNullException(nameof(downloads));
            }

            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            /* funktioniert, aber nicht wirklich asynchron:
            
            //var filePath = Path.Combine(this.filesFolder, file.Name);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                
                foreach (var downloadTask in downloads)
                {
                    var chunk = await downloadTask;

                    fileStream.Write(chunk.Payload.ToByteArray(), 0, chunk.Payload.Length);
                }
            }*/

            //var fileReplies = await Task.WhenAll(downloads); geht nicht wenn öfter aufs selbe file zugegriffen wird (immer selber host)

            var fileReplies = new List<FileReply>();

            foreach (var downloadTask in downloads)
            {
                fileReplies.Add(await downloadTask);
            }

            /*var newFile = */this.fileService.SaveFile(this.MergePayloads(fileReplies), this.filesFolder, file.Name);
        }

        private byte[] MergePayloads(IEnumerable<FileReply> fileReplies)
        {
            return fileReplies.SelectMany(fr => fr.Payload).ToArray();
        }
    }
}
