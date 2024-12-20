namespace Subnautica.Events
{
    using HarmonyLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;

    using System;
    using System.Diagnostics;

    using UnityEngine.SceneManagement;

    public sealed class Main : SubnauticaPlugin
    {
        /**
         *
         * Eklenti Adı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override string Name { get; } = "Subnautica Events";

        /**
         *
         * Eklenti önceliği
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override SubnauticaPluginPriority Priority { get; set; } = SubnauticaPluginPriority.First;

        /**
         *
         * Eklenti Aktifleştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnEnabled()
        {
            base.OnEnabled();
            
            SceneManager.sceneLoaded += Patches.Events.Game.SceneLoaded.Run;

            try
            {
                var harmony = new Harmony("Subnautica.Events.Main");
                harmony.PatchAll();
            }
            catch (Exception e)
            {
                Log.Error($"Harmony - Patching failed! {e}");
            }
        }
    }
}
