namespace Subnautica.Events.Patches.Events.PDA
{
    using HarmonyLib;
    using Subnautica.API.Features;
    using System;

    [HarmonyPatch(typeof(PinManager), nameof(PinManager.NotifyMove))]
    public static class ItemPinMoved
    {
        private static void Postfix()
        {
            if(Network.IsMultiplayerActive)
            {
                try
                {
                    Handlers.PDA.OnItemPinMoved();
                }
                catch (Exception e)
                {
                    Log.Error($"ItemPinMoved.Postfix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}