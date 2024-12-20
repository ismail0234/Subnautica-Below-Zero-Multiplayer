namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::BaseControlRoom), nameof(global::BaseControlRoom.OhClickMinimapConsole))]
    public static class BaseControlRoomMinimapUsing
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::BaseControlRoom __instance)
        {
            if (!Network.IsMultiplayerActive || EventBlocker.IsEventBlocked(TechType.BaseControlRoom))
            {
                return true;
            }

            try
            {
                BaseControlRoomMinimapUsingEventArgs args = new BaseControlRoomMinimapUsingEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject, false));

                Handlers.Furnitures.OnBaseControlRoomMinimapUsing(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"BaseControlRoomMinimapClicking.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}