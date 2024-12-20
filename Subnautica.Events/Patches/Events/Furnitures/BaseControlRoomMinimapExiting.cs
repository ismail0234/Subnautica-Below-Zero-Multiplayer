namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::BaseControlRoom), nameof(global::BaseControlRoom.Update))]
    public static class BaseControlRoomMinimapExiting
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix(global::BaseControlRoom __instance)
        {
            if (Network.IsMultiplayerActive && __instance.navigatingMinimap)
            {
                var flag = (__instance.minimapConsole.transform.position - global::Player.main.transform.position).magnitude > 3.0f;
                if (GameInput.GetButtonUp(GameInput.Button.RightHand) | flag || GameInput.GetButtonUp(GameInput.Button.Exit))
                {
                    try
                    {
                        BaseControlRoomMinimapExitingEventArgs args = new BaseControlRoomMinimapExitingEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject), __instance.minimapBase.transform.localPosition);

                        Handlers.Furnitures.OnBaseControlRoomMinimapExiting(args);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"BaseControlRoomMinimapExiting.Prefix: {e}\n{e.StackTrace}");
                    }
                }
            }
        }
    }
}