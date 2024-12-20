namespace Subnautica.Events.Patches.Events.Game
{
    using System;
    using System.Collections;

    using HarmonyLib;
    using Steamworks;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::IntroVignette), nameof(global::IntroVignette.Start))]
    public static class IntroChecking
    {
        private static IEnumerator Postfix(IEnumerator values, global::IntroVignette __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                yield return values;
            }
            else
            {
                IntroCheckingEventArgs args = new IntroCheckingEventArgs();

                try
                {
                    Handlers.Game.OnIntroChecking(args);
                }
                catch (Exception e)
                {
                    Log.Error($"IntroChecking.Postfix: {e}\n{e.StackTrace}");
                }

                if (args.IsAllowed)
                {
                    yield return values;
                }
                else
                {
                    IntroVignette.main = __instance;
            
                    if (args.WaitingMethod != null)
                    {
                        yield return args.WaitingMethod;
                    }
                }
            }
        }
    }
}