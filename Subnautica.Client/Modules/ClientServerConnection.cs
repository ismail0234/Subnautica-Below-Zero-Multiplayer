namespace Subnautica.Client.Modules
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;
    using System.Collections.Generic;
    using System.Text;

    using TMPro;

    using UnityEngine.UI;

    public static class ClientServerConnection
    {
        /**
         *
         * Oyun içi menü açılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnInGameMenuOpened(InGameMenuOpenedEventArgs ev)
        {
            if (Network.IsMultiplayerActive)
            {
                if (IngameMenu.main.saveButton.gameObject.activeSelf)
                {
                    IngameMenu.main.saveButton.gameObject.SetActive(false);
                }

                if (IngameMenu.main.maxSecondsToBeRecentlySaved != 900000)
                {
                    IngameMenu.main.maxSecondsToBeRecentlySaved = 900000f;
                }
            }
            else
            {
                IngameMenu.main.saveButton.gameObject.SetActive(true);
                IngameMenu.main.maxSecondsToBeRecentlySaved = 120f;
            }
        }

        /**
         *
         * Oyun içi menü kapandıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnInGameMenuClosed(InGameMenuClosedEventArgs ev)
        {/*
            if (ZeroGame.IsCurrentMultiplayerGame && !ZeroGame.IsMultiplayerConnectionActive)
            {
                ZeroGame.FreezeGame();
            }*/
        }

        /**
         *
         * Arka planda çalışma ayarı değişirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSettingsRunInBackgroundChanging(SettingsRunInBackgroundChangingEventArgs ev)
        {
            if (Network.IsMultiplayerActive)
            {
                ev.IsAllowed = false;
            }
        }

        /**
         *
         * Ayarlardaki pda oyun duraklatma seçeneği değiştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSettingsPdaGamePauseChanging(SettingsPdaGamePauseChangingEventArgs ev)
        {
            if (Network.IsMultiplayerActive)
            {
                ev.IsAllowed = false;
            }
        }
    }
}