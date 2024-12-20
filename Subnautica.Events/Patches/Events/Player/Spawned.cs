namespace Subnautica.Events.Patches.Events.Player
{
    using System;
    using System.Collections;

    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::Player), nameof(global::Player.ResetPlayerOnDeath))]
    public class Spawned
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator Postfix(IEnumerator values, global::Player __instance)
        {
            yield return values;

            if (Network.IsMultiplayerActive)
            {
                try
                {
                    Handlers.Player.OnSpawned();
                }
                catch (Exception e)
                {
                    Log.Error($"Spawned.Postfix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}