namespace Subnautica.Events.Patches.Events.Building
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(Builder), nameof(Builder.Update))]
    public class ConstructionGhostMoved
    {
        /**
         *
         * StopwatchItem nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static StopwatchItem StopwatchItem = new StopwatchItem(BroadcastInterval.ConstructingGhostMoved);

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Postfix()
        {
            if (Network.IsMultiplayerActive)
            {
                if (StopwatchItem.IsFinished())
                {
                    StopwatchItem.Restart();

                    if (Builder.prefab == null || Builder.ghostModel == null)
                    {
                        return;
                    }

                    try
                    {
                        ConstructionGhostMovedEventArgs args = new ConstructionGhostMovedEventArgs(
                            Builder.ghostModel,
                            Builder.lastTechType,
                            Builder.GetAimTransform(),
                            Builder.canPlace,
                            Builder.lastRotation
                        );

                        Handlers.Building.OnConstructingGhostMoved(args);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"ConstructingGhostMoved.Postfix: {e}\n{e.StackTrace}");
                    }
                }
            }
        }
    }
}
