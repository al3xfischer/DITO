using System.Collections.Generic;
using System.IO;

namespace Client.Services.Interfaces
{
    public interface IFileService
    {
        ICollection<FileInfo> GetAllFileEntries();

        void AddFileEntry(FileInfo file);

        FileInfo GetFileEntry(string fileName);

        byte[] ReadFile(FileInfo file, long startIndex, long length);

        void RemoveFileEntry(FileInfo file);

        FileInfo SaveFile(byte[] data, string path, string fileName);

        FileInfo CopyToPath(FileInfo file, string targetPath);

        void DeleteFile(FileInfo file);

        string GetHash(FileInfo file);
    }
}