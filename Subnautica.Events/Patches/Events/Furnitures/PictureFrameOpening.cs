namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using Subnautica.Events.Patches.Fixes.Interact;

    [HarmonyPatch(typeof(global::PictureFrame), nameof(global::PictureFrame.OnHandClick))]
    public static class PictureFrameOpening
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::PictureFrame __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.enabled)
            {
                return false;
            }

            if (EventBlocker.IsEventBlocked(TechType.PictureFrame))
            {
                return true;
            }

            var uniqueId = PictureFrame.GetUniqueId(__instance);
            if (uniqueId.IsNull())
            {
                return false;
            }

            var constructable = __instance.GetComponentInParent<Constructable>();
            if (constructable)
            {
                try
                {
                    PictureFrameOpeningEventArgs args = new PictureFrameOpeningEventArgs(uniqueId);

                    Handlers.Furnitures.OnPictureFrameOpening(args);

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"PictureFrameOpening.Prefix: {e}\n{e.StackTrace}");
                    return true;
                }
            }

            try
            {
                SeaTruckPictureFrameOpeningEventArgs args = new SeaTruckPictureFrameOpeningEventArgs(uniqueId);

                Handlers.Vehicle.OnSeaTruckPictureFrameOpening(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"SeaTruckPictureFrameOpening.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}
