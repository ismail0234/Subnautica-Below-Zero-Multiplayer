namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::Recyclotron), nameof(global::Recyclotron.RecycleAsync))]
    public static class RecyclotronRecycle
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix(global::Recyclotron __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                if(__instance.IsPowered() && __instance.wasteList.Count == 1)
                {
                    var wasteItem  = __instance.wasteList.GetLast<Recyclotron.Waste>().inventoryItem;
                    var gameObject = wasteItem.item.gameObject;
                    if (!__instance.IsUsedBattery(gameObject))
                    {
                        var constructable = __instance.GetComponent<Constructable>();
                        if (constructable)
                        {
                            try
                            {
                                RecyclotronRecycleEventArgs args = new RecyclotronRecycleEventArgs(Network.Identifier.GetIdentityId(constructable.gameObject));

                                Handlers.Furnitures.OnRecyclotronRecycle(args);
                            }
                            catch (Exception e)
                            {
                                Log.Error($"RecyclotronRecycle.Prefix: {e}\n{e.StackTrace}");
                            }
                        }
                    }
                }
            }
        }
    }
}