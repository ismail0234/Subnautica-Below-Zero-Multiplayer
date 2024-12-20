namespace Subnautica.Events.Patches.Events.Player
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::PlayerFrozenMixin), nameof(global::PlayerFrozenMixin.Unfreeze))]
    public class Unfreezed
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix(global::PlayerFrozenMixin __instance)
        {
            if (Network.IsMultiplayerActive && __instance.frozen)
            {
                try
                {
                    Handlers.Player.OnUnfreezed();
                }
                catch (Exception e)
                {
                    Log.Error($"Unfreezed.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}