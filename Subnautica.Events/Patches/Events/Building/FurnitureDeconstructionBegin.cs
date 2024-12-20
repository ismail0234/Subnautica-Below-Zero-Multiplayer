namespace Subnautica.Events.Patches.Events.Building
{
    using System;

    using FMODUnity;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    [HarmonyPatch(typeof(global::BuilderTool), nameof(global::BuilderTool.HandleInput))]
    public static class FurnitureDeconstructionBegin
    {
        private static bool Prefix(global::BuilderTool __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.handleInputFrame == Time.frameCount)
            {
                return false;
            }

            __instance.handleInputFrame = Time.frameCount;
            if (!__instance.isDrawn || Builder.isPlacing || (!AvatarInputHandler.main.IsEnabled() || __instance.TryDisplayNoPowerTooltip()))
            {
                return false;
            }

            Targeting.AddToIgnoreList(global::Player.main.gameObject);
            Targeting.GetTarget(30f, out var result, out var distance);
            if (result == null)
            {
                return true;
            }

            bool buttonHeld1 = GameInput.GetButtonHeld(GameInput.Button.LeftHand);
            bool buttonDown = GameInput.GetButtonDown(GameInput.Button.Deconstruct);
            bool buttonHeld2 = GameInput.GetButtonHeld(GameInput.Button.Deconstruct);

            Constructable constructable = result.GetComponentInParent<Constructable>();
            if (constructable != null && distance > constructable.placeMaxDistance)
            {
                constructable = null;
            }

            if (constructable != null)
            {
                __instance.OnHover(constructable);
                if (buttonHeld1)
                {
                    __instance.Construct(constructable, true);
                }
                else
                {
                    string reason;
                    if (constructable.DeconstructionAllowed(out reason))
                    {
                        if (!buttonHeld2)
                        {
                            return false;
                        }

                        if (constructable.constructed)
                        {
                            Builder.ResetLast();

                            try
                            {
                                FurnitureDeconstructionBeginEventArgs args = new FurnitureDeconstructionBeginEventArgs(Network.Identifier.GetIdentityId(constructable.gameObject), constructable.techType);

                                Handlers.Building.OnFurnitureDeconstructionBegin(args);

                                if (!args.IsAllowed)
                                {
                                    return false;
                                }

                                constructable.SetState(false, false);
                            }
                            catch(Exception e)
                            {
                                Log.Error($"FurnitureDeconstructionBegin.Exception: {e}");
                                return false;
                            }
                        }
                        else
                        {
                            __instance.Construct(constructable, false, buttonDown);
                        }
                    }
                    else
                    {
                        if (!buttonDown || string.IsNullOrEmpty(reason))
                        {
                            return false;
                        }

                        RuntimeManager.PlayOneShot("event:/bz/ui/item_error");
                        ErrorMessage.AddMessage(reason);
                    }
                }
            }
            else
            {
                BaseDeconstructable deconstructable = result.GetComponentInParent<BaseDeconstructable>();
                if (deconstructable == null)
                {
                    BaseExplicitFace componentInParent = result.GetComponentInParent<BaseExplicitFace>();
                    if (componentInParent != null)
                    {
                        deconstructable = componentInParent.parent;
                    }
                }

                if (deconstructable == null || distance > 11.0)
                {
                    return false;
                }

                string reason;
                if (deconstructable.DeconstructionAllowed(out reason))
                {
                    __instance.OnHover(deconstructable);
                    if (!buttonDown)
                    {
                        return false;
                    }

                    Builder.ResetLast();
                    deconstructable.Deconstruct();
                }
                else
                {
                    if (!buttonDown || string.IsNullOrEmpty(reason))
                    {
                        return false;
                    }

                    RuntimeManager.PlayOneShot("event:/bz/ui/item_error");
                    ErrorMessage.AddMessage(reason);
                }
            }

            return false;
        }
    }
}
