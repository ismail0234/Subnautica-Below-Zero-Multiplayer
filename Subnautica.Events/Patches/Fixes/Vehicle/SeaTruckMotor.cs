namespace Subnautica.Events.Patches.Fixes.Vehicle
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Storage.World.Childrens;

    using UnityEngine;

    [HarmonyPatch]
    public static class SeaTruckMotor
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckMotor), nameof(global::SeaTruckMotor.StabilizeRoll))]
        private static bool SeaTruckMotor_StabilizeRoll(global::SeaTruckMotor __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }
            var num = Mathf.Abs(__instance.transform.eulerAngles.z - 180f);
            if (num > 179.5f)
            {
                return false;
            }

            var entity = GetEntity(__instance);
            if (entity == null)
            {
                return false;
            }

            return entity.IsMine(ZeroPlayer.CurrentPlayer.UniqueId);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckMotor), nameof(global::SeaTruckMotor.StabilizePitch))]
        private static bool SeaTruckMotor_StabilizePitch(global::SeaTruckMotor __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            var num = Mathf.Abs(__instance.transform.eulerAngles.x - 180f);
            if (180.0f - num <= 0.0f)
            {
                return false;
            }

            var entity = GetEntity(__instance);
            if (entity == null)
            {
                return false;
            }

            return entity.IsMine(ZeroPlayer.CurrentPlayer.UniqueId);
        }

        [HarmonyPatch(typeof(global::SeaTruckMotor), nameof(global::SeaTruckMotor.FixedUpdate))]
        private static bool Prefix(global::SeaTruckMotor __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.truckSegment.isMainCab && __instance.useRigidbody != null && !__instance.useRigidbody.isKinematic && !__instance.piloting && __instance.useRigidbody.velocity.y > -0.300000011920929 && __instance.pilotPosition.position.y > Ocean.GetOceanLevel() - 2.0)
            {
                var entity = GetEntity(__instance);
                if (entity == null)
                {
                    return false;
                }

                return entity.IsMine(ZeroPlayer.CurrentPlayer.UniqueId);
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
        private static WorldDynamicEntity GetEntity(global::SeaTruckMotor __instance)
        {
            var uniqueId = __instance.truckSegment.gameObject.GetIdentityId();
            if (uniqueId.IsNull())
            {
                return null;
            }

            return Network.DynamicEntity.GetEntity(uniqueId);
        }
    }
}
