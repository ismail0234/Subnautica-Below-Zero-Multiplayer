namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::BaseMiniCell), nameof(global::BaseMiniCell.OnClick))]
    public static class BaseControlRoomCellPowerChanging
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::BaseMiniCell __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            try
            {
                BaseControlRoomCellPowerChangingEventArgs args = new BaseControlRoomCellPowerChangingEventArgs(Network.Identifier.GetIdentityId(__instance.GetComponentInParent<BaseControlRoom>().gameObject, false), __instance.cell);

                Handlers.Furnitures.OnBaseControlRoomCellPowerChanging(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"BaseControlRoomCellPowerChanging.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}