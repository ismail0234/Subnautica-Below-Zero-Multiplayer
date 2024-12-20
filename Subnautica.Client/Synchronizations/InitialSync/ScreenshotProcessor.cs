namespace Subnautica.Client.Synchronizations.InitialSync
{
    using System.Collections.Generic;
    using System.IO;

    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Network.Models.Metadata;

    public class ScreenshotProcessor
    {
        /**
         *
         * Mevcut resim isimlerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static List<string> GetLocalScreenshotFileNames()
        {
            string folderPath = Paths.GetMultiplayerClientRemoteScreenshotsPath(ZeroPlayer.CurrentPlayer.CurrentServerId);
            Directory.CreateDirectory(folderPath);

            List<string> images = new List<string>();
            foreach (var item in Directory.GetFiles(folderPath))
            {
                images.Add(Path.GetFileName(item));
            }

            return images;
        }

        /**
         *
         * Oyuncu'ya özel ekran görüntülerini senkronlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnScreenshotInitialized()
        {
            ScreenshotManager.dirInfo?.Delete();
            ScreenshotManager.savePath = Paths.GetMultiplayerClientSavePath(ZeroPlayer.CurrentPlayer.CurrentServerId);
            ScreenshotManager.dirInfo  = new global::Platform.IO.DirectoryInfo(ScreenshotManager.Combine(ScreenshotManager.savePath, "Screenshots"));

            var screenshotFiles = ScreenshotManager.GetScreenshotFiles();
            if (screenshotFiles != null)
            {
                foreach (var screenshot in screenshotFiles)
                {
                    ScreenshotManager.AddRequest(screenshot.Key, ScreenshotManager.instance);
                }
            }
        }

        /**
         *
         * Sunucu taraflı ekran görüntülerini ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
         public static void OnPictureFrameInitialized(Dictionary<string, PictureFrame> pictureFrames, List<string> existImages)
        {
            foreach (var pictureFrame in pictureFrames)
            {
                MetadataProcessor.ExecuteProcessor(TechType.PictureFrame, pictureFrame.Key, pictureFrame.Value, true);
            }

            foreach (var item in Directory.GetFiles(Paths.GetMultiplayerClientRemoteScreenshotsPath(ZeroPlayer.CurrentPlayer.CurrentServerId)))
            {
                if (!item.Contains(".jpg"))
                {
                    continue;
                }

                if (!existImages.Contains(Path.GetFileName(item.Replace("_thumbnail", ""))))
                {
                    File.Delete(item);
                }
            }
         }
    }
}
