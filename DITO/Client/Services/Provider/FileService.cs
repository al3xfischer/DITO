using Client.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

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

        public ICollection<FileInfo> GetAllFileEntries()
        {
            return this.files;
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

        public FileInfo CopyToPath(FileInfo file, string targetPath)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (targetPath is null)
            {
                throw new ArgumentNullException(nameof(targetPath));
            }

            if (!file.Exists)
            {
                throw new ArgumentException("The file does not exist");
            }

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            var filename = Path.Combine(targetPath, file.Name);
            
            try
            {
                File.Copy(file.FullName, filename);
            }
            catch
            {
                return null;
            }
            
            return new FileInfo(filename);
        }

        public void DeleteFile(FileInfo file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (!file.Exists) return;

            File.Delete(file.FullName);
        }

        public string GetHash(FileInfo file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (!file.Exists)
            {
                throw new ArgumentException("The file does not exist");
            }

            var hash = string.Empty;
            using (var sha = SHA512.Create())
            using(var stream = File.OpenRead(file.FullName))
            {
                hash = Convert.ToBase64String(sha.ComputeHash(stream));
            }

            return hash;
        }
    }
}
