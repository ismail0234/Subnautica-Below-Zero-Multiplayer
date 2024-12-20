namespace Subnautica.Events.Patches.Events.Creatures
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::Crash), nameof(global::Crash.Inflate))]
    public class CrashFishInflating
    {
        private static bool Prefix(global::Crash __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.state == Crash.State.Inflating)
            {
                return false;
            }

            __instance.OnState(Crash.State.Inflating);

            try
            {
                CrashFishInflatingEventArgs args = new CrashFishInflatingEventArgs(__instance.gameObject.GetIdentityId());

                Handlers.Creatures.OnCrashFishInflating(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"CrashFishInflating.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}
