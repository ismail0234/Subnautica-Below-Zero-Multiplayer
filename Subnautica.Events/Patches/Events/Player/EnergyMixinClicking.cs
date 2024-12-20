namespace Subnautica.Events.Patches.Events.Player
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    [HarmonyPatch(typeof(global::GenericHandTarget), nameof(global::GenericHandTarget.OnHandClick))]
    public class EnergyMixinClicking
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::GenericHandTarget __instance)
        {
            if (!Network.IsMultiplayerActive || EventBlocker.IsEventBlocked(TechType.PictureSamHand))
            {
                return true;
            }

            var batterySlotId = GetBatterySlotId(__instance.gameObject);
            var vehicleId     = GetVehicleUniqueId(__instance.gameObject);
            var vehicleType   = GetVehicleType(__instance.gameObject);
            if (string.IsNullOrEmpty(batterySlotId) || string.IsNullOrEmpty(vehicleId) || !vehicleType.IsVehicle())
            {
                return true;
            }

            try
            {
                EnergyMixinClickingEventArgs args = new EnergyMixinClickingEventArgs(vehicleId, batterySlotId, vehicleType);

                Handlers.Player.OnEnergyMixinClicking(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"EnergyMixinClicking.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }
        
        /**
         *
         * Slot idsini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string GetBatterySlotId(GameObject gameObject)
        {
            return Network.Identifier.GetIdentityId(gameObject, false);
        }

        /**
         *
         * Araç idsini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string GetVehicleUniqueId(GameObject gameObject)
        {
            var vehicleGameObject = GetVehicleGameObject(gameObject);
            if (vehicleGameObject == null)
            {
                return null;
            }

            return Network.Identifier.GetIdentityId(vehicleGameObject, false);
        }

        /**
         *
         * Araç türünü döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static TechType GetVehicleType(GameObject gameObject)
        {
            var vehicleGameObject = GetVehicleGameObject(gameObject);
            if (vehicleGameObject == null)
            {
                return TechType.None;
            }

            return CraftData.GetTechType(vehicleGameObject);
        }

        /**
         *
         * Araç nesnesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static GameObject GetVehicleGameObject(GameObject gameObject)
        {
            var exosuit = gameObject.GetComponentInParent<global::Exosuit>();
            if (exosuit)
            {
                return exosuit.gameObject;
            }

            return gameObject.transform.parent.gameObject;
        }
    }
}