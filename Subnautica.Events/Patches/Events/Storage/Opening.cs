namespace Subnautica.Events.Patches.Events.Storage
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::StorageContainer), nameof(global::StorageContainer.OnHandClick))]
    public static class Opening
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
            if(!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.enabled || __instance.disableUseability)
            {
               return false;
            }

            var techType = TechType.None;
            var uniqueId = "";

            var constructable = __instance.GetComponent<Constructable>();
            if (constructable && constructable.constructed)
            {
                techType = constructable.techType;
                uniqueId = constructable.gameObject.GetIdentityId();
            }
            else
            {
                if (__instance.GetComponentInParent<LifepodDrop>())
                {
                    techType = TechType.EscapePod;
                    uniqueId = __instance.gameObject.GetIdentityId();
                }
                else
                {
                    if (__instance.GetComponentInParent<SeaTruckSegment>())
                    {
                        techType = CraftData.GetTechType(__instance.transform.gameObject);
                        uniqueId = __instance.gameObject.GetIdentityId();
                    }
                    else
                    {
                        var mapRoomFunctionality = __instance.GetComponentInParent<global::MapRoomFunctionality>();
                        if (mapRoomFunctionality)
                        {
                            techType = TechType.BaseMapRoom;
                            uniqueId = mapRoomFunctionality.GetBaseDeconstructable()?.gameObject?.GetIdentityId();
                        }
                        else
                        {
                            var lwe = __instance.GetComponentInParent<LargeWorldEntity>();
                            if (lwe)
                            {
                                techType = CraftData.GetTechType(lwe.gameObject);
                                uniqueId = lwe.gameObject.GetIdentityId();
                            }
                        }
                    }
                }
            }

            if (uniqueId.IsNull())
            {
                return false;
            }

            try
            {
                StorageOpeningEventArgs args = new StorageOpeningEventArgs(uniqueId, techType);

                Handlers.Storage.OnOpening(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"Storage.Opening: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}