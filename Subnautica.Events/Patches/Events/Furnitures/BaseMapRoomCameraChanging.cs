namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::MapRoomScreen), nameof(global::MapRoomScreen.CycleCamera))]
    public static class BaseMapRoomCameraChanging
    {
        private static bool Prefix(global::MapRoomScreen __instance, int direction = 1)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            try
            {
                MapRoomCameraChangingEventArgs args = new MapRoomCameraChangingEventArgs(__instance.mapRoomFunctionality.GetBaseDeconstructable().gameObject.GetIdentityId(), direction == 1);

                Handlers.Furnitures.OnBaseMapRoomCameraChanging(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"MapRoomCameraChanging.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}
