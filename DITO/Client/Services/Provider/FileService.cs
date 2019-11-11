using Client.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Client.Services.Provider
{
    public class FileService : IFileService
    {
        private ICollection<FileInfo> files;

        public FileService()
        {
            files = new HashSet<FileInfo>();
        }

        public void AddFileEntry(FileInfo file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            files.Add(file);
        }

        public void RemoveFileEntry(FileInfo file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            files.Remove(file);
        }

        public FileInfo GetFileEntry(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException($"The {nameof(fileName)} must not be null, empty or consits only of white spaces.", nameof(fileName));
            }

            return files.FirstOrDefault(file => file.Name == fileName);
        }

        public byte[] ReadFile(FileInfo file, int startIndex, int length)
        {
            if (file.Length < length)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "The specified length does not correspond to the file size.");
            }

            if (startIndex < 0 || startIndex > file.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            var data = new byte[length];
            using (var stream = new FileStream(file.FullName, FileMode.Open))
            {
                if (stream.CanSeek) stream.Seek(startIndex, SeekOrigin.Begin);

                stream.Read(data, 0, length);
            }

            return data;
        }

        public FileInfo SaveFile(byte[] data, string path, string fileName)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (fileName is null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var fullName = Path.Combine(path, fileName);
            File.WriteAllBytes(fullName, data);
            return new FileInfo(fullName);
        }


    }
}
