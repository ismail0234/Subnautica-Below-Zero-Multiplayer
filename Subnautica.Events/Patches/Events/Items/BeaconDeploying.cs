namespace Subnautica.Events.Patches.Events.Items
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    [HarmonyPatch(typeof(global::Beacon), nameof(global::Beacon.Throw))]
    public class BeaconDeploying
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::Beacon __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    BeaconDeployingEventArgs args = new BeaconDeployingEventArgs(Network.Identifier.GetIdentityId(__instance.pickupable.gameObject), __instance.pickupable, GetDropPosition(__instance), __instance.transform.rotation, __instance.deployedOnLand, __instance.beaconLabel.labelName);

                    Handlers.Items.OnBeaconDeploying(args);

                    if (!args.IsAllowed)
                    {
                        __instance._isInUse = false;
                    }

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"BeaconDeploying.Prefix: {e}\n{e.StackTrace}");
                }
            }

            return true;
        }

        /**
         *
         * Koordinatı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Vector3 GetDropPosition(global::Beacon __instance)
        {
            if (__instance.deployedOnLand && __instance.IsValidGroundPlacement(out var groundPosition) && !__instance.HasObstacleInFront())
            {
                groundPosition += Vector3.up * 1.1f;
                return groundPosition;
            }
            
            return ZeroGame.FindDropPosition(__instance.transform.position);
        }
    }
}