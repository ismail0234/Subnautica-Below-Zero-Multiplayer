namespace Subnautica.Events.Patches.Identity.World
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::BulkheadDoor), nameof(global::BulkheadDoor.OnEnable))]
    public static class BulkheadDoorx
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix(global::BulkheadDoor __instance)
        {
            if (Network.IsMultiplayerActive && !__instance.GetComponentInParent<BaseDeconstructable>())
            {
                var uniqueId = Network.GetWorldEntityId(__instance.transform.position);

                Network.Identifier.SetIdentityId(__instance.gameObject, uniqueId);

                EntitySpawnedEventArgs args = new EntitySpawnedEventArgs(uniqueId, __instance.gameObject, null, TechType.BaseBulkhead, EntitySpawnLevel.Bulkhead, true);

                try
                {
                    Handlers.World.OnEntitySpawned(args);
                }
                catch (Exception e)
                {
                    Log.Error($"BulkheadDoor.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
