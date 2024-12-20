namespace Subnautica.Events.Patches.Events.Game
{
    using System;

    using HarmonyLib;
    
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::ScreenshotManager), nameof(global::ScreenshotManager.RemoveScreenshot))]
    public static class ScreenshotsRemoved
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Postfix(string filename)
        {
            if(Network.IsMultiplayerActive)
            {
                try
                {
                    ScreenshotsRemovedEventArgs args = new ScreenshotsRemovedEventArgs(filename);

                    Handlers.Game.OnScreenshotsRemoved(args);
                }
                catch (Exception e)
                {
                    Log.Error($"Game.ScreenshotsRemoved: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}