namespace Subnautica.Events.Patches.Events.Creatures
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::LilyPaddlerHypnotize), nameof(global::LilyPaddlerHypnotize.StartPerform))]
    public class LilyPaddlerHypnotizeStarting
    {
        private static bool Prefix(global::LilyPaddlerHypnotize __instance)
        {
            if (!Network.IsMultiplayerActive || __instance.lastTarget.target == null || EventBlocker.IsEventBlocked(TechType.LilyPaddler))
            {
                return true;
            }

            var player = ZeroPlayer.GetPlayerByGameObject(__instance.lastTarget.target);
            if (player == null)
            {
                return false;
            }

            try
            {
                LilyPaddlerHypnotizeStartingEventArgs args = new LilyPaddlerHypnotizeStartingEventArgs(__instance.creature.gameObject.GetIdentityId(), player.PlayerId);

                Handlers.Creatures.OnLilyPaddlerHypnotizeStarting(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"LilyPaddlerHypnotizeStarting.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}
