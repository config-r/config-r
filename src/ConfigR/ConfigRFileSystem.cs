// <copyright file="ConfigRFileSystem.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using ScriptCs;

    public class ConfigRFileSystem : IFileSystem
    {
        private static readonly string currentDirectory = Path.GetDirectoryName(typeof(ConfigRFileSystem).Assembly.Location);
        private readonly IFileSystem fileSystem;

        [CLSCompliant(false)]
        public ConfigRFileSystem(IFileSystem fileSystem)
        {
            Guard.AgainstNullArgument("fileSystem", fileSystem);

            this.fileSystem = fileSystem;
        }

        public IEnumerable<string> EnumerateFiles(string dir, string searchPattern)
        {
            return this.fileSystem.EnumerateFiles(dir, searchPattern);
        }

        public void Copy(string source, string dest, bool overwrite)
        {
            this.fileSystem.Copy(source, dest, overwrite);
        }

        public bool DirectoryExists(string path)
        {
            return this.fileSystem.DirectoryExists(path);
        }

        public void CreateDirectory(string path)
        {
            this.fileSystem.CreateDirectory(path);
        }

        public void DeleteDirectory(string path)
        {
            this.fileSystem.DeleteDirectory(path);
        }

        public string ReadFile(string path)
        {
            return this.fileSystem.ReadFile(path);
        }

        public string[] ReadFileLines(string path)
        {
            return this.fileSystem.ReadFileLines(path);
        }

        public bool IsPathRooted(string path)
        {
            return this.fileSystem.IsPathRooted(path);
        }

        public string CurrentDirectory
        {
            get { return currentDirectory; }
        }

        public string NewLine
        {
            get { return this.fileSystem.NewLine; }
        }

        public DateTime GetLastWriteTime(string file)
        {
            return this.fileSystem.GetLastWriteTime(file);
        }

        public void Move(string source, string dest)
        {
            this.fileSystem.Move(source, dest);
        }

        public bool FileExists(string path)
        {
            return this.fileSystem.FileExists(path);
        }

        public void FileDelete(string path)
        {
            this.fileSystem.FileDelete(path);
        }

        public IEnumerable<string> SplitLines(string value)
        {
            return this.fileSystem.SplitLines(value);
        }

        public Stream CreateFileStream(string filePath, FileMode mode)
        {
            return this.fileSystem.CreateFileStream(filePath, mode);
        }

        public string GetWorkingDirectory(string path)
        {
            return string.IsNullOrWhiteSpace(path) ? this.CurrentDirectory : this.fileSystem.GetWorkingDirectory(path);
        }

        public string GetFullPath(string path)
        {
            return this.fileSystem.GetFullPath(path);
        }
    }
}
