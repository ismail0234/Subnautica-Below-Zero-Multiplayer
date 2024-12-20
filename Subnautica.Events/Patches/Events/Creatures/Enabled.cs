namespace Subnautica.Events.Patches.Events.Creatures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::Creature), nameof(global::Creature.OnEnable))]
    public class Enabled
    {
        private static void Postfix(global::Creature __instance)
        {
            try
            {
                CreatureEnabledEventArgs args = new CreatureEnabledEventArgs(__instance);

                Handlers.Creatures.OnEnabled(args);
            }
            catch (Exception e)
            {
                Log.Error($"Enabled.Postfix: {e}\n{e.StackTrace}");
            }
        }
    }
}
