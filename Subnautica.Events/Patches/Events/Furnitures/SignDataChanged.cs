namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;
    using System.Collections;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.API.Extensions;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    public static class SignDataChangedShared
    {
        /**
         *
         * Tetiklenme durumunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsTriggered { get; set; } = false;

        /**
         *
         * Olayı Tetikler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void TriggerEvent(global::uGUI_SignInput __instance = null)
        {
            if (Network.IsMultiplayerActive && !IsTriggered && !EventBlocker.IsEventBlocked(TechType.Sign))
            {
                UWE.CoroutineHost.StartCoroutine(TriggerEventCallback(__instance));
            }
        }

        /**
         *
         * Olayı Tetikler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void FastTriggerEvent(global::uGUI_SignInput __instance = null)
        {
            if (Network.IsMultiplayerActive && !EventBlocker.IsEventBlocked(TechType.Sign))
            {
                UWE.CoroutineHost.StartCoroutine(TriggerEventCallback(__instance, true));
            }
        }

        /**
         *
         * İç Olayı Tetikler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator TriggerEventCallback(global::uGUI_SignInput __instance, bool isFast = false)
        {
            if (!isFast)
            {
                IsTriggered = true;

                yield return new WaitForSecondsRealtime(0.1f);

                IsTriggered = false;
            }

            if (__instance != null)
            {
                try
                {
                    var uniqueId      = string.Empty;
                    var techType      = TechType.None;

                    var constructable = __instance.GetComponentInParent<Constructable>();
                    if (constructable)
                    {
                        if (constructable.constructed)
                        {
                            uniqueId = Network.Identifier.GetIdentityId(constructable.gameObject);
                            techType = constructable.techType;
                        }
                    }
                    else
                    {
                        if (__instance.GetComponentInParent<global::SeaTruckSegment>())
                        { 
                            uniqueId = Network.Identifier.GetIdentityId(__instance.gameObject, false);
                            if (uniqueId.IsNotNull())
                            {
                                uniqueId = uniqueId.Replace(ZeroGame.GetSeaTruckColoredLabelUniqueId(null, true), "");
                            }

                            techType = CraftData.GetTechType(__instance.gameObject);
                        }
                        else
                        {
                            var lwe = __instance.GetComponentInParent<LargeWorldEntity>();
                            if (lwe)
                            {
                                uniqueId = Network.Identifier.GetIdentityId(lwe.gameObject, false);
                                techType = CraftData.GetTechType(lwe.gameObject);
                            }
                        }
                    }

                    if (uniqueId.IsNotNull() && techType != TechType.None)
                    {
                        SignDataChangedEventArgs args = new SignDataChangedEventArgs(uniqueId, techType, __instance.text, __instance.scaleIndex, __instance.colorIndex, __instance.elementsState, __instance.background == null ? false : __instance.background.enabled);

                        Handlers.Furnitures.OnSignDataChanged(args);
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"Furnitures.SignDataChanged: {e}\n{e.StackTrace}");
                }
            }
        }
    }

    [HarmonyPatch(typeof(global::uGUI_SignInput), nameof(global::uGUI_SignInput.SetScale))]
    public static class SignDataChangedSetScale
    {
        private static void Prefix(global::uGUI_SignInput __instance, bool increase)
        {
            SignDataChangedShared.FastTriggerEvent(__instance);
        }
    }

    [HarmonyPatch(typeof(global::uGUI_SignInput), nameof(global::uGUI_SignInput.SetBackground))]
    public static class SignDataChangedSetBackground
    {
        private static void Postfix(global::uGUI_SignInput __instance)
        {
            SignDataChangedShared.FastTriggerEvent(__instance);
        }
    }

    [HarmonyPatch(typeof(global::uGUI_SignInput), nameof(global::uGUI_SignInput.UpdateColor))]
    public static class SignDataChangedUpdateColor
    {
        private static void Postfix(global::uGUI_SignInput __instance)
        {
            SignDataChangedShared.FastTriggerEvent(__instance);
        }
    }

    [HarmonyPatch(typeof(global::uGUI_SignInput), nameof(global::uGUI_SignInput.ToggleElementState))]
    public static class SignDataChangedToggleElementState
    {
        private static void Postfix(global::uGUI_SignInput __instance)
        {
            SignDataChangedShared.FastTriggerEvent(__instance);
        }
    }

    [HarmonyPatch(typeof(global::uGUI_SignInput), nameof(global::uGUI_SignInput.OnSelect))]
    public static class SignDataChangedOnSelect
    {
        private static void Prefix(uGUI_SignInput __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                __instance.inputField.onValueChanged.RemoveAllListeners();
                __instance.inputField.onValueChanged.AddListener((string text) => {
                    SignDataChangedShared.TriggerEvent(__instance);
                });
            }
        }
    }

    [HarmonyPatch(typeof(global::uGUI_SignInput), nameof(global::uGUI_SignInput.OnDeselect))]
    public static class SignDataChangedOnDeselect
    {
        private static void Prefix(uGUI_SignInput __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                __instance.inputField.onValueChanged.RemoveAllListeners();


                var uniqueId = string.Empty;
                var techType = TechType.None;

                var constructable = __instance.GetComponentInParent<Constructable>();
                if (constructable)
                {
                    if (constructable.constructed)
                    {
                        uniqueId = Network.Identifier.GetIdentityId(constructable.gameObject);
                        techType = constructable.techType; 
                    }
                }
                else
                {
                    if (__instance.GetComponentInParent<global::SeaTruckSegment>())
                    {
                        uniqueId = Network.Identifier.GetIdentityId(__instance.gameObject, false).Replace(ZeroGame.GetSeaTruckColoredLabelUniqueId(null, true), "");
                        techType = CraftData.GetTechType(__instance.gameObject);
                    }
                    else
                    {
                        var lwe = __instance.GetComponentInParent<LargeWorldEntity>();
                        if (lwe)
                        {
                            uniqueId = Network.Identifier.GetIdentityId(lwe.gameObject, false);
                            techType = CraftData.GetTechType(lwe.gameObject);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(uniqueId) && techType != TechType.None)
                {
                    try
                    {
                        SignDeselectEventArgs args = new SignDeselectEventArgs(uniqueId, techType);

                        Handlers.Furnitures.OnSignDeselect(args);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Furnitures.SignDataChangedOnDeselect: {e}\n{e.StackTrace}");
                    }
                }
            }  
        }
    }

    [HarmonyPatch(typeof(global::ColoredLabel), nameof(global::ColoredLabel.OnHandClick))]
    public class ColoredLabelSelecting
    {
        private static bool Prefix(global::ColoredLabel __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                var uniqueId = string.Empty;
                var techType = TechType.None;

                var constructable = __instance.GetComponentInParent<Constructable>();
                if (constructable)
                {
                    uniqueId = Network.Identifier.GetIdentityId(constructable.gameObject);
                    techType = constructable.techType;
                }
                else
                {
                    if (__instance.GetComponentInParent<global::SeaTruckSegment>())
                    {
                        uniqueId = Network.Identifier.GetIdentityId(__instance.signInput.gameObject, false).Replace(ZeroGame.GetSeaTruckColoredLabelUniqueId(null, true), "");
                        techType = CraftData.GetTechType(__instance.gameObject);
                    }
                    else
                    {
                        var lwe = __instance.GetComponentInParent<LargeWorldEntity>();
                        if (lwe)
                        {
                            uniqueId = Network.Identifier.GetIdentityId(lwe.gameObject, false);
                            techType = CraftData.GetTechType(lwe.gameObject);
                        }
                    }
                }

                if (string.IsNullOrEmpty(uniqueId) && techType != TechType.None)
                {
                    return false;
                }

                try
                {
                    SignSelectEventArgs args = new SignSelectEventArgs(uniqueId, techType);

                    Handlers.Furnitures.OnSignSelect(args);

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"ColoredLabelSelecting.Prefix: {e}\n{e.StackTrace}");
                    return true;
                }
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(global::Sign), nameof(global::Sign.OnHandClick))]
    public static class SignDataChangedOnHandClick
    {
        private static bool Prefix(global::Sign __instance)
        {
            if (!Network.IsMultiplayerActive || !__instance.enabled)
            {
                return true;
            }

            var constructable = __instance.GetComponentInParent<Constructable>();
            if (constructable != null && constructable.constructed)
            {
                try
                {
                    SignSelectEventArgs args = new SignSelectEventArgs(Network.Identifier.GetIdentityId(constructable.gameObject), constructable.techType);

                    Handlers.Furnitures.OnSignSelect(args);

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"Furnitures.SignDataChangedOnHandClick: {e}\n{e.StackTrace}");
                }
            }

            return true;
        }
    }
}