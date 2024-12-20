namespace Subnautica.Events.Patches.Events.Vehicle
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using Subnautica.Events.Patches.Fixes.Interact;

    using System;

    using UnityEngine;

    [HarmonyPatch(typeof(global::DockedVehicleHandTarget), nameof(global::DockedVehicleHandTarget.OnHandClick))]
    public static class Undocking
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */

        private static bool Prefix(global::DockedVehicleHandTarget __instance)
        {
            if (!Network.IsMultiplayerActive || EventBlocker.IsEventBlocked(TechType.BaseMoonpool))
            {
                return true;
            }

            if (!__instance.isValidHandTarget || PlayerCinematicController.cinematicModeCount > 0)
            {
                return false;
            }

            var vehicleId = GetVehicleId(__instance);
            if (vehicleId.IsNull())
            {
                return false;
            }

            var uniqueId = GetUniqueId(__instance.gameObject);
            if (uniqueId.IsNull())
            {
                return false;
            }

            var techType = GetTechType(__instance.gameObject);
            if (techType == TechType.None)
            {
                return false;
            }

            try
            {
                VehicleUndockingEventArgs args = new VehicleUndockingEventArgs(uniqueId, vehicleId, techType, __instance.dockingBay.GetDockedObject().transform.position, __instance.dockingBay.GetDockedObject().transform.rotation, __instance.name.Contains("Left"));

                Handlers.Vehicle.OnUndocking(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"Undocking.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }

        /**
         *
         * UniqueId değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string GetUniqueId(GameObject gameObject)
        {
            return Network.Identifier.GetIdentityId(gameObject.GetComponentInParent<BaseDeconstructable>().gameObject, false);
        }

        /**
         *
         * VehicleId değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string GetVehicleId(global::DockedVehicleHandTarget dockedVehicleHandTarget)
        {
            if (dockedVehicleHandTarget.dockingBay.GetDockedObject() == null)
            {
                return null;
            }

            return Network.Identifier.GetIdentityId(dockedVehicleHandTarget.dockingBay.GetDockedObject().gameObject, false);
        }

        /**
         *
         * TechType değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static TechType GetTechType(GameObject gameObject)
        {
            var baseDeconstructable = gameObject.GetComponentInParent<BaseDeconstructable>();
            if (baseDeconstructable)
            {
                return baseDeconstructable.recipe;
            }

            return TechType.None;
        }
    }
}