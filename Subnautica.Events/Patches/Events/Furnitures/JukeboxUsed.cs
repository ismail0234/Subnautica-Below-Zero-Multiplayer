namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;
    using System.Collections;

    using HarmonyLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;
    using UnityEngine.EventSystems;

    public static class JukeboxShared
    {
        /**
         *
         * Tetiklenme durumunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool IsVolumeTriggered { get; set; } = false;

        /**
         *
         * Olayı Tetikler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool FastTrigger(JukeboxInstance __instance, CustomProperty property)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (EventBlocker.IsEventBlocked(TechType.Jukebox))
            {
                return true;
            }

            var uniqueId = JukeboxShared.GetUniqueId(__instance);
            if (uniqueId.IsNull())
            {
                return false;
            }

            try
            {
                JukeboxUsedEventArgs args = new JukeboxUsedEventArgs(uniqueId, property, __instance.GetComponentInParent<global::SeaTruckSegment>());

                Handlers.Furnitures.OnJukeboxUsed(args);
            }
            catch (Exception e)
            {
                Log.Error($"JukeboxShared.FastTrigger: {e}\n{e.StackTrace}");
            }

            return false;
        }

        /**
         *
         * Olayı Tetikler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void DelayedVolumeTrigger(global::JukeboxInstance __instance)
        {
            if (Network.IsMultiplayerActive && !IsVolumeTriggered && !EventBlocker.IsEventBlocked(TechType.Jukebox))
            {
                UWE.CoroutineHost.StartCoroutine(TriggerEventCallback(__instance));
            }
        }

        /**
         *
         * İç Olayı Tetikler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator TriggerEventCallback(global::JukeboxInstance __instance)
        {
            IsVolumeTriggered = true;

            yield return new WaitForSecondsRealtime(0.1f);

            IsVolumeTriggered = false;

            FastTrigger(__instance, new CustomProperty((byte) JukeboxProcessType.Volume, Math.Round(Jukebox.volume, 4).ToInvariantCultureString()));
        }

        /**
         *
         * İç Olayı Tetikler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string GetUniqueId(global::JukeboxInstance __instance)
        {
            var constructable = __instance.GetComponentInParent<global::Constructable>();
            if (constructable)
            {
                return constructable.gameObject.GetIdentityId();
            }

            var seaTruckSegment = __instance.GetComponentInParent<global::SeaTruckSegment>();
            if (seaTruckSegment)
            {
                return seaTruckSegment.gameObject.GetIdentityId();
            }

            return null;
        }
    }

    [HarmonyPatch]
    public class JukeboxUsed
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::JukeboxInstance), nameof(global::JukeboxInstance.OnButtonPrevious))]
        private static bool OnButtonPrevious(global::JukeboxInstance __instance)
        {
            if (!__instance.isControlling)
            {
                return false;
            }

            return JukeboxShared.FastTrigger(__instance, new CustomProperty((byte) JukeboxProcessType.IsPrevious, true.ToString()));
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::JukeboxInstance), nameof(global::JukeboxInstance.OnButtonNext))]
        private static bool OnButtonNext(global::JukeboxInstance __instance)
        {
            if (!__instance.isControlling)
            {
                return false;
            }

            return JukeboxShared.FastTrigger(__instance, new CustomProperty((byte) JukeboxProcessType.IsNext, true.ToString()));
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::JukeboxInstance), nameof(global::JukeboxInstance.OnButtonShuffle))]
        private static bool OnButtonShuffle(global::JukeboxInstance __instance)
        {
            if (!__instance.isControlling)
            {
                return false;
            }

            return JukeboxShared.FastTrigger(__instance, new CustomProperty((byte)JukeboxProcessType.IsShuffled, (!__instance.shuffle).ToString()));
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::JukeboxInstance), nameof(global::JukeboxInstance.OnButtonRepeat))]
        private static bool OnButtonRepeat(global::JukeboxInstance __instance)
        {
            if (!__instance.isControlling)
            {
                return false;
            }

            return JukeboxShared.FastTrigger(__instance, new CustomProperty((byte)JukeboxProcessType.RepeatMode, Jukebox.GetNextRepeat(__instance.repeat).ToString()));
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::JukeboxInstance), nameof(global::JukeboxInstance.OnButtonPlayPause))]
        private static bool OnButtonPlayPause(global::JukeboxInstance __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                if (__instance._baseComp != null && !__instance._baseComp.IsPowered(__instance.transform.position))
                {
                    return false;
                }

                if (GameModeManager.GetOption<bool>(GameOption.TechnologyRequiresPower) && __instance._powerRelay != null && __instance._powerRelay.GetPower() <= 1f)
                {
                    return false;    
                }
            }

            bool isPaused = __instance.imagePlayPause.sprite == __instance.spritePlay ? false : true;
            return JukeboxShared.FastTrigger(__instance, new CustomProperty((byte) JukeboxProcessType.IsPaused, isPaused.ToString()));
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::JukeboxInstance), nameof(global::JukeboxInstance.OnButtonStop))]
        private static bool OnButtonStop(global::JukeboxInstance __instance)
        {
            return JukeboxShared.FastTrigger(__instance, new CustomProperty((byte) JukeboxProcessType.IsStoped, true.ToString()));
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::JukeboxInstance), nameof(global::JukeboxInstance.OnPositionUp))]
        private static bool OnPositionUp(global::JukeboxInstance __instance, PointerEventData eventData)
        {
            float value;
            if (!__instance.isControlling || !MaterialExtensions.GetBarValue(__instance.imagePosition.rectTransform, (BaseEventData) eventData, __instance._materialPosition, true, out value))
            {
                return false;
            }

            return JukeboxShared.FastTrigger(__instance, new CustomProperty((byte) JukeboxProcessType.Position, value.ToInvariantCultureString()));
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::JukeboxInstance), nameof(global::JukeboxInstance.OnVolume))]
        private static void OnVolume(global::JukeboxInstance __instance, PointerEventData eventData)
        {
            if (!MaterialExtensions.GetBarValue(__instance.imageVolume.rectTransform, eventData, __instance._materialVolume, true, out var value))
            {
                return;
            }

            if (System.Math.Round(__instance.volume, 2) == System.Math.Round(value, 2))
            {
                return;
            }

            JukeboxShared.DelayedVolumeTrigger(__instance);
        }
    }
}