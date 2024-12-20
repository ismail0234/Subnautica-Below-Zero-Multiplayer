namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::Bed), nameof(global::Bed.ExitInUseMode))]
    public static class BedExitInUseMode
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix(global::Bed __instance)
        {
            if (!Network.IsMultiplayerActive || __instance.inUseMode == Bed.InUseMode.None)
            {
                return;
            }

            var bedItem = BedInformationItem.GetInformation(__instance);
            if (bedItem == null)
            {
                return;
            }

            try
            {
                BedExitInUseModeEventArgs args = new BedExitInUseModeEventArgs(bedItem.UniqueId, bedItem.TechType, bedItem.IsSeaTruckModule);

                Handlers.Furnitures.OnBedExitInUseMode(args);
            }
            catch (Exception e)
            {
                Log.Error($"Furnitures.BedExitInUseMode: {e}\n{e.StackTrace}");
            }
        }
    }
}