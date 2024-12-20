namespace Subnautica.Events.Patches.Events.Vehicle
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    using UnityEngine;

    [HarmonyPatch(typeof(global::Exosuit), nameof(global::Exosuit.ApplyJumpForce))]
    public static class ExosuitJumping
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::Exosuit __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.timeLastJumped + 1.0f > Time.time)
            {
                return false;
            }

            try
            {
                ExosuitJumpingEventArgs args = new ExosuitJumpingEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject));

                Handlers.Vehicle.OnExosuitJumping(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"ExosuitJumping.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}