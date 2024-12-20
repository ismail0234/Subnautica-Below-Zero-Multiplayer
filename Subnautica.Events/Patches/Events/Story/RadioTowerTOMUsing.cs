namespace Subnautica.Events.Patches.Events.Story
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::RadioTowerController), nameof(global::RadioTowerController.OnHandClick))]
    public static class RadioTowerTOMUsing
    {
        private static bool Prefix(global::RadioTowerController __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.itemInsertedStoryGoal.IsComplete() || global::Inventory.main.GetPickupCount(__instance.requiredTechType) <= 0)
            {
                return false;
            }

            try
            {
                RadioTowerTOMUsingEventArgs args = new RadioTowerTOMUsingEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject));

                Handlers.Story.OnRadioTowerTOMUsing(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"RadioTowerTOMUsing.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}