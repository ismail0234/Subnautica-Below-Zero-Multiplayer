namespace Subnautica.Events.Patches.Events.Game
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::PowerRelay), nameof(global::PowerRelay.AddInboundPower))]
    public class PowerSourceAdding
    {
        private static void Prefix(global::PowerRelay __instance, IPowerInterface powerInterface)
        {
            if (Network.IsMultiplayerActive)
            {
                if (!__instance.inboundPowerSources.Contains(powerInterface))
                {
                    try
                    {
                        PowerSourceAddingEventArgs args = new PowerSourceAddingEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject, false), powerInterface);

                        Handlers.Game.OnPowerSourceAdding(args);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"PowerSourceAdding.Prefix: {e}\n{e.StackTrace}");
                    }
                }
            }
        }
    }
}
