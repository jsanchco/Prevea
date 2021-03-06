﻿namespace Prevea.Service.Service
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using IRepository.IRepository;

    #endregion

    public class ServiceBase
    {
        #region Members

        private bool _disposed;
        protected IRepository Repository;

        public string TmpUpload => "~/App_Data/TMP";

        #endregion

        #region Constructor

        public ServiceBase(IRepository repository) => Repository = repository;

        #endregion

        #region Util

        public string GetExtension(int userId)
        {
            var files = Directory.GetFiles(HttpContext.Current.Server.MapPath(TmpUpload));

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var extension = Path.GetExtension(fileName);
                var fileWithOutExtension = fileName.Replace(extension, "");

                if (fileWithOutExtension == userId.ToString())
                {
                    return extension.ToLower();
                }
            }

            return string.Empty;
        }

        public void RestoreFile(int userId, string urlRelative)
        {
            CreateDirectory(urlRelative);

            var extension = Path.GetExtension(urlRelative);
            var fileName = $"{userId}{extension}";
            var urlTmp = Path.Combine(HttpContext.Current.Server.MapPath(TmpUpload), fileName);
            urlRelative = Path.Combine(HttpContext.Current.Server.MapPath(urlRelative));

            RemoveFilesFromDirectory(urlRelative);

            if (!File.Exists(urlTmp))
                return;

            File.Copy(urlTmp, urlRelative, true);
            RemoveFiles(userId);
        }

        public void SaveFileTmp(int userId, HttpPostedFileBase file)
        {
            CreateDirectory();

            RemoveFiles(userId);

            var extension = Path.GetExtension(Path.GetFileName(file.FileName));
            var url = Path.Combine(HttpContext.Current.Server.MapPath(TmpUpload), $"{userId}{extension}");

            file.SaveAs(url);
        }

        public void CreateDirectory(string directory = null)
        {
            var physicalPath = directory == null
                ? HttpContext.Current.Server.MapPath(TmpUpload)
                : new Uri(new Uri(HttpContext.Current.Server.MapPath(directory)), ".").OriginalString;

            var exits = Directory.Exists(physicalPath);

            if (!exits)
                Directory.CreateDirectory(physicalPath);
        }

        public List<string> RemoveFiles(int userId)
        {
            var files = Directory.GetFiles(HttpContext.Current.Server.MapPath(TmpUpload));
            var removeFiles = new List<string>();

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var extension = Path.GetExtension(fileName);
                var fileWithOutExtension = fileName.Replace(extension, "");

                if (fileWithOutExtension != userId.ToString())
                    continue;

                File.Delete(file);
                removeFiles.Add(file);
            }

            return removeFiles;
        }

        public List<string> RemoveFilesFromDirectory(string url)
        {
            var filesRemoved = new List<string>();

            var directory = Path.GetDirectoryName(url);
            if (directory == null)
                return filesRemoved;

            var files = Directory.GetFiles(directory);
            var fileNameWithoutExtension = Path.GetFileName(url).Replace(Path.GetExtension(url), "");
            foreach (var file in files)
            {
                if (Path.GetFileName(file).Replace(Path.GetExtension(file), "") != fileNameWithoutExtension)
                    continue;

                RemoveFile(file);
                filesRemoved.Add(file);
            }

            return filesRemoved;
        }

        public bool RemoveFile(string url)
        {
            if (!File.Exists(url))
                return false;

            File.Delete(url);

            return true;
        }

        public string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #endregion

        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Repository.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
