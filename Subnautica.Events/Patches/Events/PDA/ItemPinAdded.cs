namespace Subnautica.Events.Patches.Events.PDA
{
    using HarmonyLib;
    using Subnautica.API.Features;
    using System;

    [HarmonyPatch(typeof(PinManager), nameof(PinManager.NotifyAdd))]
    public static class ItemPinAdded
    {
        private static void Postfix()
        {
            if(Network.IsMultiplayerActive)
            {
                try
                {
                    Handlers.PDA.OnItemPinAdded();
                }
                catch (Exception e)
                {
                    Log.Error($"ItemPinAdded.Postfix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}