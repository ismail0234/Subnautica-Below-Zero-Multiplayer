namespace Subnautica.Events.Patches.Fixes.Game
{
    using System.Collections.Generic;

    using HarmonyLib;

    using Subnautica.API.Features;

    using UWE;

    public class FreezeTimeShared
    {
        private static List<FreezeTime.Id> BlacklistedFreezeTime { get; set; } = new List<FreezeTime.Id>()
        { 
            FreezeTime.Id.ApplicationFocus,
            FreezeTime.Id.FeedbackPanel,
            FreezeTime.Id.IngameMenu,
            FreezeTime.Id.TextInput,
            FreezeTime.Id.PDA,
        };

        public static bool IsBlacklisted(FreezeTime.Id id)
        {
            if(!Network.IsMultiplayerActive)
            {
                return true;
            }

            return !BlacklistedFreezeTime.Contains(id);
        }
    }

    [HarmonyPatch(typeof(FreezeTime), nameof(FreezeTime.Begin))]
    public class FreezeTimeBegin
    {
        private static bool Prefix(FreezeTime.Id id)
        {
            return FreezeTimeShared.IsBlacklisted(id);
        }
    }

    [HarmonyPatch(typeof(FreezeTime), nameof(FreezeTime.End))]
    public class FreezeTimeEnd
    {
        private static bool Prefix(FreezeTime.Id id)
        {
            return FreezeTimeShared.IsBlacklisted(id);
        }
    }
}
