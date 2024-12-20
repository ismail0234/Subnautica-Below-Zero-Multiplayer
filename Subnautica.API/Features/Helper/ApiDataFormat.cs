namespace Subnautica.API.Features.Helper
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using Subnautica.API.Extensions;

    public class ApiDataFormat
    {
        /**
         *
         * IsStatus değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsStatus { get; set; }
        
        /**
         *
         * IsPreRelease değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsPreRelease { get; set; }

        /**
         *
         * Version değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Version { get; set; }

        /**
         *
         * Assets değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<ApiDataAssetsFormat> Assets { get; set; } = new List<ApiDataAssetsFormat>();

        /**
         *
         * Downloads değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<ApiDataDownloadItem> Downloads { get; set; } = new List<ApiDataDownloadItem>();

        /**
         *
         * Languages değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<string> Languages { get; set; } = new List<string>();

        /**
         *
         * Toplam dosya boyutunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public double GetTotalFileSize()
        {
            long totalFileSize = 0;

            foreach (var item in this.Downloads)
            {
                totalFileSize += item.FileSize;
            }

            return (double)totalFileSize;
        }

        /**
         *
         * İndirilecek dosyaları döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<ApiDataDownloadItem> GetDownloadFiles()
        {
            var files = new List<ApiDataDownloadItem>();

            foreach (var item in this.Downloads)
            {
                files.Add(this.GetDownloadItem(item.TempName, item.LocalPath, item.RemoteUrl, item.FileSize, item.CheckVersion, item.CustomVersion));
            }

            return files;
        }

        /**
         *
         * İndirme nesnesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private ApiDataDownloadItem GetDownloadItem(string tempname, string localPath, string remoteUrl, long fileSize, bool checkVersion, string customVersion)
        {
            return new ApiDataDownloadItem()
            {
                TempName      = tempname,
                LocalPath     = string.Format("{0}{1}{2}", localPath, localPath.IsNull() ? "" : Paths.DS.ToString(), tempname.IsNotNull() ? tempname : Path.GetFileName(remoteUrl)),
                RemoteUrl     = remoteUrl,
                FileSize      = fileSize,
                CheckVersion  = checkVersion,
                CustomVersion = customVersion,
            };
        }
    }

    public class ApiDataAssetsFormat
    {
        /**
         *
         * Path değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Path { get; set; }

        /**
         *
         * Url değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Url { get; set; }
    }

    public class ApiDataDownloadItem
    {
        /**
         *
         * TempName değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string TempName { get; set; }

        /**
         *
         * CheckVersion değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool CheckVersion { get; set; }

        /**
         *
         * CurrentVersion değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string CurrentVersion { get; set; }

        /**
         *
         * CustomVersion değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string CustomVersion { get; set; }

        /**
         *
         * LocalPath değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string LocalPath { get; set; }

        /**
         *
         * RemoteUrl değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string RemoteUrl { get; set; }

        /**
         *
         * FileSize değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public long FileSize { get; set; }
    }
}