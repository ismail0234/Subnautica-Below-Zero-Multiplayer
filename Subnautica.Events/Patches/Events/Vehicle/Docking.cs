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
    public static class Docking
    {
        /**
         *
         * StopwatchItem nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static readonly Dictionary<string, StopwatchItem> StopwatchItems = new Dictionary<string, StopwatchItem>();

        /**
         *
         * Yamalar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::VehicleDockingBay), nameof(global::VehicleDockingBay.Start))]
        private static void VehicleDockingBay_Start(global::VehicleDockingBay __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                __instance.gameObject.WaitForInitialize(CheckAction, SuccessAction);
            }
        }

        /**
         *
         * Yamalar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::VehicleDockingBay), nameof(global::VehicleDockingBay.OnDestroy))]
        private static void VehicleDockingBay_OnDestroy(global::VehicleDockingBay __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                var uniqueId = GetUniqueId(__instance.gameObject);
                if (uniqueId.IsNotNull())
                {
                    StopwatchItems.Remove(uniqueId);
                }
            }
        }

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::VehicleDockingBay), nameof(global::VehicleDockingBay.OnTriggerEnter))]
        private static bool Prefix(global::VehicleDockingBay __instance, Collider other)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.fullyConstructed || (GameModeManager.GetOption<bool>(GameOption.TechnologyRequiresPower) && !__instance.powerConsumer.IsPowered()))
            {
                return false;
            }

            var uniqueId = GetUniqueId(__instance.gameObject);
            if (uniqueId.IsNull())
            {
                return false;
            }

            if (!StopwatchItems.TryGetValue(uniqueId, out var stopwatchItem))
            {
                return false;
            }

            if (!stopwatchItem.IsFinished())
            {
                return false;
            }

            stopwatchItem.Restart();

            var lwe = other.GetComponentInParent<LargeWorldEntity>();
            if (lwe == null)
            {
                return false;
            }

            var gameObject = UWE.Utils.GetEntityRoot(other.gameObject);
            if (gameObject == null)
            {
                gameObject = other.gameObject;
            }

            if (!gameObject.TryGetComponent<Dockable>(out var dockable) || ((IDockingBay)__instance).AllowedToDock(dockable) == false)
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
                VehicleDockingEventArgs args = new VehicleDockingEventArgs(uniqueId, lwe.gameObject, techType, GetBackModulePosition(__instance, dockable), GetEndPosition(__instance, dockable), GetEndRotation(__instance, dockable));

                Handlers.Vehicle.OnDocking(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"Docking.Prefix: {e}\n{e.StackTrace}");
            }
            
            return true;
        }

        /**
         *
         * Id kontrolü yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool CheckAction(GameObject gameObject, int currentTick)
        {
            return GetUniqueId(gameObject).IsNotNull();
        }

        /**
         *
         * Id tanımlanınca çalışır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SuccessAction(GameObject gameObject)
        {
            var uniqueId = GetUniqueId(gameObject);
            if (uniqueId.IsNotNull())
            {
                StopwatchItems[uniqueId] = new StopwatchItem(BroadcastInterval.VehicleDocking);
            }
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
            if (gameObject == null)
            {
                return null;
            }

            var baseDeconstructable = gameObject.GetComponentInParent<BaseDeconstructable>();
            if (baseDeconstructable)
            {
                return baseDeconstructable.gameObject.GetIdentityId();
            }

            return null;
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

        /**
         *
         * End Position değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Vector3 GetBackModulePosition(global::VehicleDockingBay dockingBay, Dockable dockable)
        {
            if (dockingBay.MoonpoolExpansionEnabled())
            {
                return dockingBay.expansionManager.tailDockingPosition.position;
            }

            if (CraftData.GetTechType(dockable.gameObject) != TechType.SeaTruck)
            {
                return Vector3.zero;
            }

            if (dockable.TryGetComponent<global::SeaTruckSegment>(out var seaTruckSegment) && seaTruckSegment.isRearConnected)
            {
                return seaTruckSegment.rearConnection.GetConnection().truckSegment.transform.position;
            }

            return Vector3.zero;
        }

        /**
         *
         * End Position değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Vector3 GetEndPosition(global::VehicleDockingBay dockingBay, Dockable dockable)
        {
            if (dockingBay.MoonpoolExpansionEnabled())
            {
                return dockingBay.expansionManager.seatruckDockingPosition.position;
            }

            return CraftData.GetTechType(dockable.gameObject) == TechType.Exosuit ? dockingBay.dockingEndPosExo.position : dockingBay.dockingEndPos.position;
        }

        /**
         *
         * End Rotation değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Quaternion GetEndRotation(global::VehicleDockingBay dockingBay, Dockable dockable)
        {
            if (dockingBay.MoonpoolExpansionEnabled())
            {
                return dockingBay.expansionManager.seatruckDockingPosition.rotation;
            }

            return CraftData.GetTechType(dockable.gameObject) == TechType.Exosuit ? dockingBay.dockingEndPosExo.rotation : dockingBay.dockingEndPos.rotation;
        }
    }
}