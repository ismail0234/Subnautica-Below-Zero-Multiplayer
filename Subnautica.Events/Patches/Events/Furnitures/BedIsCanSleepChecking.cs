namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;
    
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::Bed), nameof(global::Bed.GetCanSleep))]
    public static class BedIsCanSleepChecking
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::Bed __instance, ref bool __result, global::Player player, bool notify)
        {
            if(!Network.IsMultiplayerActive)
            {
                return true;
            }

            var bedItem = BedInformationItem.GetInformation(__instance);
            if (bedItem == null)
            {
                return false;
            }

            try
            {
                if (!__instance.enabled || player.GetMode() != global::Player.Mode.Normal || player.IsUnderwater() || DayNightCycle.main.IsInSkipTimeMode())
                {
                    __result = false;
                    return false;
                }

                if (player.timeLastSleep + __instance.kSleepInterval <= DayNightCycle.main.timePassed)
                {
                    BedIsCanSleepCheckingEventArgs args = new BedIsCanSleepCheckingEventArgs(bedItem.UniqueId, __instance.GetSide(player), bedItem.IsSeaTruckModule);

                    Handlers.Furnitures.OnBedIsCanSleepChecking(args);

                    __result = args.IsAllowed;
                    return false;
                }
                
                if (notify)
                {
                    ErrorMessage.AddMessage(global::Language.main.Get("BedSleepTimeOut"));
                }

                __result = false;
                return false;
            }
            catch (Exception e)
            {
                Log.Error($"Furnitures.BedIsCanSleepChecking: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}