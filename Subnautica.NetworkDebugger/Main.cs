namespace Subnautica.NetworkDebugger
{
    using System;
    using System.Diagnostics;

    using HarmonyLib;

    using Subnautica.API.Features;

    public sealed class Main : SubnauticaPlugin
    {
        /**
         *
         * Eklenti Adı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override string Name { get; } = "Subnautica Network Debugger";

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

            try
            {
                var harmony = new Harmony("Subnautica.NetworkDebugger.Main");
                harmony.PatchAll();
            }
            catch (Exception e)
            {
                Log.Error($"Harmony - Patching failed! {e}");
            }
        }
    }
}
