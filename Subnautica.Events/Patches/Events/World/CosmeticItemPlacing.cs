namespace Subnautica.Events.Patches.Events.Items
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::PlaceTool), nameof(global::PlaceTool.Place))]
    public class CosmeticItemPlacing
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::PlaceTool __instance)
        {
            if (Network.IsMultiplayerActive && __instance.validPosition && __instance.ghostModel && __instance.TryGetComponent<Pickupable>(out var pickupable))
            {
                var techType = pickupable.GetTechType();
                if (techType.IsPoster() || techType.IsPictureFrame() || techType == TechType.FredShavingKit)
                {
                    try
                    {
                        CosmeticItemPlacingEventArgs args = new CosmeticItemPlacingEventArgs(pickupable.gameObject.GetIdentityId(), global::Player.main.GetCurrentSub()?.GetModulesRoot()?.gameObject?.GetIdentityId(), techType, __instance.ghostModel.transform.position, __instance.ghostModel.transform.rotation);

                        Handlers.World.OnCosmeticItemPlacing(args);

/*
                        if (!args.IsAllowed)
                        {
                            __instance.usedThisFrame = false;
                        }*/

                        return args.IsAllowed;
                    }
                    catch (Exception e)
                    {
                        Log.Error($"CosmeticItemPlacing.Prefix: {e}\n{e.StackTrace}");
                    }
                }
            }

            return true;
        }
    }
}