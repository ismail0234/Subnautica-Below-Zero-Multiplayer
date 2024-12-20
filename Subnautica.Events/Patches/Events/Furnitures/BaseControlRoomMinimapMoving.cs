namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::BaseControlRoom), nameof(global::BaseControlRoom.Update))]
    public static class BaseControlRoomMinimapMoving
    {
        /**
         *
         * Geçen zamanı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static readonly StopwatchItem StopwatchItem = new StopwatchItem(BroadcastInterval.BaseControlRoomMinimapMoving);

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix(global::BaseControlRoom __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                if (__instance.navigatingMinimap && StopwatchItem.IsFinished())
                {
                    StopwatchItem.Restart();

                    try
                    {
                        BaseControlRoomMinimapMovingEventArgs args = new BaseControlRoomMinimapMovingEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject, false), __instance.minimapBase.transform.localPosition);

                        Handlers.Furnitures.OnBaseControlRoomMinimapMoving(args);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"BaseControlRoomCellPowerChanging.Prefix: {e}\n{e.StackTrace}");
                    }
                }
            }
        }
    }
}