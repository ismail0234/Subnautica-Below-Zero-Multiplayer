namespace Subnautica.Events.Patches.Events.PDA
{
    using HarmonyLib;
    using Subnautica.API.Features;
    using System;

    [HarmonyPatch(typeof(PinManager), nameof(PinManager.NotifyRemove))]
    public static class ItemPinRemoved
    {
        private static void Postfix()
        {
            if(Network.IsMultiplayerActive)
            {
                try
                {
                    Handlers.PDA.OnItemPinRemoved();
                }
                catch (Exception e)
                {
                    Log.Error($"ItemPinRemoved.Postfix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}