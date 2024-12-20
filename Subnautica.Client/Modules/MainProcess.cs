namespace Subnautica.Client.Modules
{
    using System;

    using Subnautica.API.Features;
    using Subnautica.Client.Core;
    using Subnautica.Client.Multiplayer.Cinematics;
    using Subnautica.Events.EventArgs;

    public class MainProcess
    {
        /**
         *
         * Eklenti aktifleştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPluginEnabled()
        {
            ZeroLanguage.LoadLanguage(Tools.GetLanguage());
        }

        /**
         *
         * Oyuncu ana menüye gittiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnQuittingToMainMenu(QuittingToMainMenuEventArgs ev)
        {
            ClearAllCache();
        }

        /**
         *
         * Sahne yüklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSceneLoaded(SceneLoadedEventArgs ev)
        {
            if (ev.Scene.name == "XMenu")
            {
                ClearAllCache();
            }
        }

        /**
         *
         * Tüm önbelleği temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ClearAllCache()
        {
            try
            {
                QualitySetting.DisableFastMode();
                Network.Dispose();
                NetworkServer.AbortServer();
                NetworkClient.Disconnect();

                PlayerCinematicQueue.Dispose();
                Multiplayer.Furnitures.Bed.Dispose();
            }
            catch (Exception e)
            {
                Log.Error($"ClearAllCache Exception: {e}");
            }
        }
    }
}