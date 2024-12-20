namespace Subnautica.Events.Patches.Events.Vehicle
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;
    using System.Collections.Generic;

    using UnityEngine;

    [HarmonyPatch]
    public static class MapRoomCameraDocking
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::MapRoomCameraDocking), nameof(global::MapRoomCameraDocking.OnTriggerEnter))]
        private static bool Prefix(global::MapRoomCameraDocking __instance, Collider other)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.camera != null)
            {
                return false;
            }

            var component1 = other.GetComponent<Rigidbody>();
            var component2 = other.gameObject.GetComponent<MapRoomCamera>();
            if (component1 == null || component2 == null || component1.isKinematic)
            {
                return false;
            }

            try
            {
                MapRoomCameraDockingEventArgs args = new MapRoomCameraDockingEventArgs(MapRoomCameraDocking.GetUniqueId(__instance), component2.gameObject.GetIdentityId(), __instance.dockingTransform.position, __instance.dockingTransform.rotation, __instance.name.Contains("dockingPoint2"));

                Handlers.Vehicle.OnMapRoomCameraDocking(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"MapRoomCameraDocking.Prefix: {e}\n{e.StackTrace}");
            }
            
            return true;
        }

        /**
         *
         * Benzersiz ID'yi döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetUniqueId(global::MapRoomCameraDocking __instance)
        {
            return __instance.GetComponentInParent<MapRoomFunctionality>()?.GetBaseDeconstructable()?.gameObject.GetIdentityId();
        }
    }
}