namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::StorageContainer), nameof(global::StorageContainer.OnHandHover))]
    public class StorageContainer
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::StorageContainer __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.enabled || __instance.disableUseability)
            {
                return false;
            }

            var uniqueId = GetUniqueId(__instance);
            if (uniqueId.IsNull())
            {
                return false;
            }

            if (Interact.IsBlocked(uniqueId))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }

        /**
         *
         * Yapı idsini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string GetUniqueId(global::StorageContainer __instance)
        {
            var constructable = __instance.gameObject.GetComponent<Constructable>();
            if (constructable)
            {
                if (!constructable.constructed)
                {
                    return null;
                }

                return constructable.gameObject.GetIdentityId();
            }

            var lifepod = __instance.GetComponentInParent<LifepodDrop>();
            if (lifepod)
            {
                return __instance.gameObject.GetIdentityId();
            }

            var seaTruckSegment = __instance.GetComponentInParent<global::SeaTruckSegment>();
            if (seaTruckSegment)
            {
                return __instance.gameObject.GetIdentityId();
            }

            var mapRoomFunctionality = __instance.GetComponentInParent<global::MapRoomFunctionality>();
            if (mapRoomFunctionality)
            {
                return mapRoomFunctionality.GetBaseDeconstructable()?.gameObject?.GetIdentityId();
            }

            var lwe = __instance.GetComponentInParent<LargeWorldEntity>();
            if (lwe)
            {
                return lwe.gameObject.GetIdentityId();
            }

            return __instance.gameObject.GetIdentityId();
        }
    }
}
