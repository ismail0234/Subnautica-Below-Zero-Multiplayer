namespace Subnautica.Events.Patches.Events.Story
{
    using global::Story;

    using HarmonyLib;
    using HighlightingSystem;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    using UnityEngine;

    [HarmonyPatch]
    public static class CinematicTriggering
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::CinematicModeTriggerBase), nameof(global::CinematicModeTriggerBase.OnHandClick))]
        private static bool OnHandClick(global::CinematicModeTriggerBase __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }
            
            if (__instance.triggerType != CinematicModeTriggerBase.TriggerType.HandTarget)
            {
                return false;
            }

            var cinematicType = GetCinematicType(__instance.gameObject);
            if (cinematicType == StoryCinematicType.None)
            {
                return true;
            }

            try
            {
                CinematicTriggeringEventArgs args = new CinematicTriggeringEventArgs(GetUniqueId(__instance), cinematicType, true);

                Handlers.Story.OnCinematicTriggering(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"CinematicTriggering.OnHandClick: {e}\n{e.StackTrace}");
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::CinematicModeTriggerBase), nameof(global::CinematicModeTriggerBase.NotifyGoalComplete))]
        private static bool NotifyGoalComplete(CinematicModeTriggerBase __instance, string key)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (StoryGoalManager.main == null || StoryGoalManager.main.IsStoryGoalMuted(key) || !StoryGoal.Equals(__instance.storyGoal, key) || __instance.triggerType == CinematicModeTriggerBase.TriggerType.Volume && !__instance.playerInTrigger)
            {
                return false;
            }

            var cinematicType = GetCinematicType(__instance.gameObject);
            if (cinematicType == StoryCinematicType.None)
            {
                return true;
            }

            try
            {
                CinematicTriggeringEventArgs args = new CinematicTriggeringEventArgs(GetUniqueId(__instance), cinematicType);

                Handlers.Story.OnCinematicTriggering(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"CinematicTriggering.NotifyGoalComplete: {e}\n{e.StackTrace}");
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::CinematicModeTriggerBase), nameof(global::CinematicModeTriggerBase.OnTriggerEnter))]
        private static bool OnTriggerEnter(CinematicModeTriggerBase __instance, Collider collider)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.triggerType != CinematicModeTriggerBase.TriggerType.Volume || __instance.volumeTriggerType != CinematicModeTriggerBase.VolumeTriggerType.OnEnter)
            {
                return false;
            }
            
            var componentInHierarchy = UWE.Utils.GetComponentInHierarchy<global::Player>(collider.gameObject);
            if (componentInHierarchy == null)
            {
                return false;
            }

            __instance.playerInTrigger = true;

            if (!__instance.CheckStoryGoal())
            {
                return false;
            }

            var cinematicType = GetCinematicType(__instance.gameObject);
            if (cinematicType == StoryCinematicType.None)
            {
                return true;
            }

            try
            {
                CinematicTriggeringEventArgs args = new CinematicTriggeringEventArgs(GetUniqueId(__instance), cinematicType);

                Handlers.Story.OnCinematicTriggering(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"CinematicTriggering.OnTriggerEnter: {e}\n{e.StackTrace}");
            }

            return true;
        }

        /**
         *
         * Benzersiz id'yi döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string GetUniqueId(global::CinematicModeTriggerBase __instance)
        {
            var lwe = __instance.GetComponentInParent<LargeWorldEntity>();
            if (lwe)
            {
                return Network.Identifier.GetIdentityId(lwe.gameObject, false);
            }

            if (__instance.TryGetComponent<CinematicModeTriggerBase>(out var cinematic))
            {
                return Network.Identifier.GetIdentityId(cinematic.gameObject, false);
            }

            return null;
        }

        /**
         *
         * Cinematic türünü döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static StoryCinematicType GetCinematicType(GameObject gameObject)
        {
            if (gameObject.name.Contains("sanctuary_cube_trigger"))
            {
                return StoryCinematicType.StoryPrecursorComputerTerminal;
            }

            if (gameObject.name.Contains("Precursor_fabricate_trigger"))
            {
                return StoryCinematicType.StoryBuildAlanTerminal;
            }

            if (gameObject.name.Contains("MargeIntro"))
            {
                return StoryCinematicType.StoryMarg1;
            }

            if (gameObject.name.Contains("NoReturnTrigger"))
            {
                return StoryCinematicType.StoryShieldBaseAlanPortal;
            }

            if (gameObject.name.Contains("ReachedElevatorTopTrigger"))
            {
                return StoryCinematicType.StoryEndGameAlanFirstMeet;
            }

            if (gameObject.name.Contains("RepairPillar1"))
            {
                return StoryCinematicType.StoryEndGameRepairPillar1;
            }

            if (gameObject.name.Contains("RepairPillar2"))
            {
                return StoryCinematicType.StoryEndGameRepairPillar2;
            }

            if (gameObject.name.Contains("ReturnArmsRightTrigger") || gameObject.name.Contains("ReturnArmsLeftTrigger"))
            {
                return StoryCinematicType.StoryEndGameReturnArms;
            }

            if (gameObject.name.Contains("EnterShipTrigger"))
            {
                return StoryCinematicType.StoryEndGameEnterShip;
            }

            if (gameObject.name.Contains("TakeOffTrigger"))
            {
                return StoryCinematicType.StoryEndGameGoToHomeWorld;
            }

            if (gameObject.name.Contains("Al-An"))
            {
                var character = gameObject.GetComponentInParent<PrecursorCharacter>();
                if (character)
                {
                    if (character.availableInteractions[0].triggerOnClick.key.Contains("BeginPrecursorNPCTransfer"))
                    {
                        return StoryCinematicType.StoryAlanTransfer;
                    }

                    if (character.availableInteractions[0].triggerOnClick.key.Contains("Interact_PostTransfer"))
                    {
                        return StoryCinematicType.StoryAlanPostTransfer;
                    }
                }
            }

            var fixedBase = gameObject.GetComponentInParent<FixedBase>();
            if (fixedBase)
            {
                if (fixedBase.name.Contains("Marguerit_Base"))
                {
                    return StoryCinematicType.StoryMarg2;
                }

                if (fixedBase.name.Contains("Marguerite_GreenHouse"))
                {
                    return StoryCinematicType.StoryMarg3;
                }
            }

            return StoryCinematicType.None;
        }
    }
}