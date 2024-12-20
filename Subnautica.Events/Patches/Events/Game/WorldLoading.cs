namespace Subnautica.Events.Patches.Events.Game
{
    using System;
    using System.Collections;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::LargeWorldStreamer), nameof(global::LargeWorldStreamer.LoadGlobalRootAsync))]
    public static class WorldLoading
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator Postfix(IEnumerator values)
        {
            if (Network.IsMultiplayerActive)
            {
                WorldLoadingEventArgs args = new WorldLoadingEventArgs();

                try
                {
                    Handlers.Game.OnWorldLoading(args);
                }
                catch (Exception e)
                {
                    Log.Error($"ConstructionInitialized.Postfix: {e}\n{e.StackTrace}");
                }

                if (args.WaitingMethods == null)
                {
                    yield return values;
                }
                else
                {
                    foreach (var waitingMethod in args.WaitingMethods)
                    {
                        yield return waitingMethod;
                    }

                    args.WaitingMethods = null;
                }
            }
            else
            {
                yield return values;
            }
        }
    }
}