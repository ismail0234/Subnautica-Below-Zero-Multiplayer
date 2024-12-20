namespace Subnautica.Events.Patches.Events.Player
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::PlayerFrozenMixin), nameof(global::PlayerFrozenMixin.Freeze))]
    public class Freezed
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Postfix(global::PlayerFrozenMixin __instance, bool __result, float endTime)
        {
            if (Network.IsMultiplayerActive && __result)
            {
                try
                {
                    PlayerFreezedEventArgs args = new PlayerFreezedEventArgs(endTime);

                    Handlers.Player.OnFreezed(args);
                }
                catch (Exception e)
                {
                    Log.Error($"PlayerDead.Postfix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}