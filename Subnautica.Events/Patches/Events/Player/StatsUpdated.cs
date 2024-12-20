namespace Subnautica.Events.Patches.Events.Player
{
    using System;

    using HarmonyLib;
    
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::Player), nameof(global::Player.FixedUpdate))]
    public static class StatsUpdated
    {
        /**
         *
         * Timing nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static readonly StopwatchItem Timing = new StopwatchItem(BroadcastInterval.PlayerStatsUpdated);

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Postfix()
        {
            if (Network.IsMultiplayerActive && Timing.IsFinished())
            {
                Timing.Restart();

                try
                {
                    Survival survival = global::Player.main.GetComponent<Survival>();

                    PlayerStatsUpdatedEventArgs args = new PlayerStatsUpdatedEventArgs(global::Player.main.liveMixin.health, survival.food, survival.water);

                    Handlers.Player.OnStatsUpdated(args);
                }
                catch (Exception e)
                {
                    Log.Error($"StatsUpdated.Postfix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}

