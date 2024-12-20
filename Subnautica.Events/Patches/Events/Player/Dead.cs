namespace Subnautica.Events.Patches.Events.Player
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::Player), nameof(global::Player.OnKill))]
    public class Dead
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Postfix(global::Player __instance, DamageType damageType)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    PlayerDeadEventArgs args = new PlayerDeadEventArgs(damageType);

                    Handlers.Player.OnDead(args);
                }
                catch (Exception e)
                {
                    Log.Error($"PlayerDead.Postfix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}