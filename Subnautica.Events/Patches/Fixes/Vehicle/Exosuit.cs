namespace Subnautica.Events.Patches.Fixes.Vehicle
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    [HarmonyPatch]
    public static class Exosuit
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Exosuit), nameof(global::Exosuit.Update))]
        private static bool ExosuitUpdate(global::Exosuit __instance)
        {
            if (!Network.IsMultiplayerActive || __instance.GetPilotingMode())
            {
                return true;
            }

            if (IsUsingByPlayer(__instance))
            {
                __instance.SetIKEnabled(true);

                if (__instance.armsDirty)
                {
                    __instance.UpdateExosuitArms();
                }

                return false;
            }

            return true;
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Exosuit), nameof(global::Exosuit.FixedUpdate))]
        private static bool ExosuitFixedUpdate(global::Exosuit __instance)
        {
            if (!Network.IsMultiplayerActive || __instance.docked || __instance.GetPilotingMode())
            {
                return true;
            }

            return !__instance.useRigidbody.isKinematic;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Exosuit), nameof(global::Exosuit.UpdateAnimations))]
        private static bool ExosuitUpdateAnimations(global::Exosuit __instance)
        {
            if (!Network.IsMultiplayerActive || __instance.GetPilotingMode())
            {
                return true;
            }

            return !IsUsingByPlayer(__instance);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::Exosuit), nameof(global::Exosuit.ShouldSetKinematic))]
        private static void ShouldSetKinematic(global::Exosuit __instance, ref bool __result)
        {
            if (Network.IsMultiplayerActive && __result == false && __instance.GetPilotingMode() == false)
            {
                var vehicle = Network.DynamicEntity.GetEntity(__instance.pilotId);
                if (vehicle == null || !ZeroPlayer.IsPlayerMine(vehicle.OwnershipId))
                {
                    __result = true;
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::ExosuitDrillArm), nameof(global::ExosuitDrillArm.Start))]
        private static bool ExosuitDrillArmStart(global::ExosuitDrillArm __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            __instance.StopEffects();
            return false;
        }

        /**
         *
         * Aracı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool IsUsingByPlayer(global::Exosuit __instance)
        {
            if (__instance.pilotId.IsNull())
            {
                __instance.pilotId = __instance.gameObject.GetIdentityId();
            }

            var vehicle = Network.DynamicEntity.GetEntity(__instance.pilotId);

            return vehicle != null && vehicle.IsUsingByPlayer;
        }
    }
}
