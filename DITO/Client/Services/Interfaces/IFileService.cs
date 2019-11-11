using System.IO;

namespace Client.Services.Interfaces
{
    public interface IFileService
    {
        void AddFileEntry(FileInfo file);

        FileInfo GetFileEntry(string fileName);

        byte[] ReadFile(FileInfo file, int startIndex, int length);

        void RemoveFileEntry(FileInfo file);

        FileInfo SaveFile(byte[] data, string path, string fileName);
    }
}