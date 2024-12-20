namespace Subnautica.Events.Patches.Events.World
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::StoryHandTarget), nameof(global::StoryHandTarget.OnHandClick))]
    public static class AlterraPdaPickedUp
    {
        private static void Prefix(global::StoryHandTarget __instance)
        {
            if (Network.IsMultiplayerActive && __instance.primaryTooltip.Contains("PDA"))
            {
                var uniqueId = __instance.destroyGameObject.GetIdentityId();
                if (uniqueId.IsNotNull())
                {
                    try
                    {
                        AlterraPdaPickedUpEventArgs args = new AlterraPdaPickedUpEventArgs(uniqueId);

                        Handlers.World.OnAlterraPdaPickedUp(args);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"AlterraPdaPickedUp.Prefix: {e}\n{e.StackTrace}");
                    }
                }
            }
        }
    }
}