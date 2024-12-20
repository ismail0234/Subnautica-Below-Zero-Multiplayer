namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;
    using System.IO;

    using HarmonyLib;
    
    using Subnautica.API.Features;

    [HarmonyPatch(typeof(ScreenshotManager.LoadingRequest), MethodType.Constructor, new Type[] { typeof(string), typeof(string) })]
    public static class ScreenshotsFilename
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Postfix(ScreenshotManager.LoadingRequest __instance, string fileName, string url)
        {
            if (Network.IsMultiplayerActive)
            {
                string filePath = Paths.GetMultiplayerClientRemoteScreenshotsPath(ZeroPlayer.CurrentPlayer.CurrentServerId, fileName);
                if (File.Exists(filePath))
                {
                    __instance.path = filePath;
                }
            }
        }
    }
}