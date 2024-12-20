namespace Subnautica.API.Features
{
    using System;
    using System.ComponentModel;
    using System.Net;
    using System.Threading;

    public class FileDownloader
    {
        /**
         *
         * İnme durumunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool isDownloading = false;

        /**
         *
         * Tamamlanma durumunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */        
        private static bool isSuccess = false;

        /**
         *
         * İnme durumunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsDownloading()
        {
            return isDownloading;
        }

        /**
         *
         * Tamamlanma durumunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsSuccess()
        {
            return isSuccess;
        }

        /**
         *
         * Dosya normal indirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool DownloadFile(string remoteUrl, string localPath)
        {
            remoteUrl = string.Format("{0}?t={1}", remoteUrl, Tools.GetRandomInt(1000000000, 2000000000));
            
            Tools.CreateSubFolders(localPath);

            try
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x86arm) AppleWebKit/527.30 (KHTML, like Gecko) Chrome/100.0.0.0 Safair/535.29");
                    client.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
                    client.DownloadFile(new Uri(remoteUrl), localPath);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Request.DownloadFile: {e}");
                return false;
            }

            return true;
        }

        /**
         *
         * Dosyayı asenkron indirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void DownloadFileAsync(string remoteUrl, string localPath, Action<object, DownloadProgressChangedEventArgs, object> progressCallbackAction = null, Action<object, AsyncCompletedEventArgs, object> completedCallbackAction = null, object customData = null)
        {
            FileDownloader.isDownloading = true;
            
            remoteUrl = string.Format("{0}?t={1}", remoteUrl, Tools.GetRandomInt(1000000000, 2000000000));

            try
            {
                Tools.CreateSubFolders(localPath);

                using (var client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x86arm) AppleWebKit/527.30 (KHTML, like Gecko) Chrome/100.0.0.0 Safair/535.29");
                    client.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler((sender, e) => FileDownloader.OnDownloadProgressChanged(sender, e, progressCallbackAction, customData));
                    client.DownloadFileCompleted   += new AsyncCompletedEventHandler((sender, e) => FileDownloader.OnDownloadFileCompleted(sender, e, completedCallbackAction, customData));
                    client.DownloadFileAsync(new Uri(remoteUrl), localPath);
                }
            }
            catch (Exception e)
            {
                Log.Error($"FileDownloader.DownloadFileAsync: {e}");
            }
        }

        /**
         *
         * Dosya inene kadar bekleme işlemi yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Wait()
        {
            while (IsDownloading())
            {
                Thread.Sleep(250);
            }
        }

        /**
         *
         * Dosya indirilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e, Action<object, DownloadProgressChangedEventArgs, object> callBackAction, object customData = null)
        {
            try
            {
                callBackAction?.Invoke(sender, e, customData);
            }
            catch (Exception ex)
            {
                Log.Error($"FileDownloader.OnDownloadProgressChanged: {ex}");
            }
        }

        /**
         *
         * Dosya indirme tamamlanmdığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */        
        public static void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e, Action<object, AsyncCompletedEventArgs, object> callBackAction, object customData = null)
        {
            FileDownloader.isDownloading = false;
            FileDownloader.isSuccess     = e.Error == null && !e.Cancelled;

            if (e.Error != null)
            {
                Log.Error($"FileDownloader.OnDownloadFileCompleted -> Download Error: {e.Error}");
            }
            else
            {

                try
                {
                    callBackAction?.Invoke(sender, e, customData);
                }
                catch (Exception ex)
                {
                    Log.Error($"FileDownloader.OnDownloadFileCompleted: {ex}");
                }
            }
        }
    }
}
