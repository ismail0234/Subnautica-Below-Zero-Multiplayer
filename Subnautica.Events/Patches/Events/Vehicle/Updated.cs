namespace Subnautica.Events.Patches.Events.Vehicle
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    [HarmonyPatch(typeof(global::ArmsController), nameof(global::ArmsController.Update))]
    public static class Updated
    {
        /**
         *
         * StopwatchItem nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static readonly StopwatchItem StopwatchItem = new StopwatchItem(BroadcastInterval.VehicleUpdated);

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Postfix(global::ArmsController __instance)
        {
            if (Network.IsMultiplayerActive && ZeroGame.IsPlayerPiloting())
            {
                if (StopwatchItem.IsFinished())
                {
                    StopwatchItem.Restart();

                    var parentTransform  = GetParentTransform();
                    var largeWorldEntity = GetLargeWorldEntity(parentTransform);
                    if (parentTransform == null || largeWorldEntity == null)
                    {
                        return;
                    }

                    try
                    {
                        VehicleUpdatedEventArgs args = new VehicleUpdatedEventArgs(largeWorldEntity.gameObject.GetIdentityId(), largeWorldEntity.transform.position, largeWorldEntity.transform.rotation, GetVehicleType(), parentTransform.gameObject);

                        Handlers.Vehicle.OnUpdated(args);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"VehicleUpdated.Postfix: {e}\n{e.StackTrace}");
                    }
                }
            }
        }

        /**
         *
         * Ebeveyn transform döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Transform GetParentTransform()
        {
            if (SpyPenguinRemoteManager.main.GetActivePenguin())
            {
                return SpyPenguinRemoteManager.main.GetActivePenguin().transform;
            }

            if (uGUI_CameraDrone.main.activeCamera)
            {
                return uGUI_CameraDrone.main.activeCamera.transform;
            }

            return global::Player.main.transform.parent;
        }

        /**
         *
         * LWE Nesnesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static GameObject GetLargeWorldEntity(Transform parentTransform)
        {
            if (SpyPenguinRemoteManager.main.GetActivePenguin())
            {
                return SpyPenguinRemoteManager.main.GetActivePenguin().gameObject;
            }

            if (uGUI_CameraDrone.main.activeCamera)
            {
                return uGUI_CameraDrone.main.activeCamera.gameObject;
            }

            return parentTransform.GetComponentInParent<LargeWorldEntity>().gameObject;
        }

        /**
         *
         * Teknoloji türünü döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static TechType GetVehicleType()
        {
            if (SpyPenguinRemoteManager.main.currentPenguin)
            {
                return TechType.SpyPenguin;
            }

            if (uGUI_CameraDrone.main.activeCamera)
            {
                return TechType.MapRoomCamera;
            }

            if (global::Player.main.inHovercraft)
            {
                return TechType.Hoverbike;
            }

            if (global::Player.main.inExosuit)
            {
                return TechType.Exosuit;
            }

            if (global::Player.main.inSeatruckPilotingChair)
            {
                return TechType.SeaTruck;
            }

            var parentTransform = global::Player.main.transform.parent;
            if (parentTransform == null)
            {
                return TechType.None;
            }

            return CraftData.GetTechType(parentTransform.gameObject);
        }
    }
}