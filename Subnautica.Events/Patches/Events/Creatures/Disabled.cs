namespace Subnautica.Events.Patches.Events.Creatures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::Creature), nameof(global::Creature.OnDisable))]
    public class Disabled
    {
        private static void Prefix(global::Creature __instance)
        {
            try
            {
                CreatureDisabledEventArgs args = new CreatureDisabledEventArgs(__instance);

                Handlers.Creatures.OnDisabled(args);
            }
            catch (Exception e)
            {
                Log.Error($"Disabled.Prefix: {e}\n{e.StackTrace}");
            }
        }
    }
}
