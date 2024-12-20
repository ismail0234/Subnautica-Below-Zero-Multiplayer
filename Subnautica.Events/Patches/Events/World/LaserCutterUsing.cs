namespace Subnautica.Events.Patches.Events.World
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::Sealed), nameof(global::Sealed.Weld))]
    public static class LaserCutterUsing
    {
        private static bool Prefix(global::Sealed __instance, float amount)
        {
            if (!Network.IsMultiplayerActive) 
            {
                return true;
            }

            if (!__instance._sealed)
            {
                return false;
            }

            try
            {
                LaserCutterEventArgs args = new LaserCutterEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject, false), amount, __instance.maxOpenedAmount);

                Handlers.World.OnLaserCutterUsing(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"LaserCutterUsing.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}