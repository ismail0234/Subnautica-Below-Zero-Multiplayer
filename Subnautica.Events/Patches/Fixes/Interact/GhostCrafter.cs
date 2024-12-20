namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    using UnityEngine;

    [HarmonyPatch(typeof(global::GhostCrafter), nameof(global::GhostCrafter.OnHandHover))]
    public class GhostCrafter
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::GhostCrafter __instance, GUIHand hand)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.enabled || __instance.logic == null)
            {
                return false;
            }

            var uniqueId = GhostCrafter.GetUniqueId(__instance.gameObject);
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
        public static string GetUniqueId(GameObject gameObject)
        {
            var constructable = gameObject.GetComponentInParent<Constructable>();
            if (constructable)
            {
                return constructable.gameObject.GetIdentityId();
            }

            var mapRoomFunctionality = gameObject.GetComponentInParent<MapRoomFunctionality>();
            if (mapRoomFunctionality)
            {
                return mapRoomFunctionality.GetBaseDeconstructable()?.gameObject?.GetIdentityId();
            }

            var spawnBase = gameObject.GetComponentInParent<global::PrefabSpawnBase>();
            if (spawnBase)
            {
                return spawnBase.gameObject.GetIdentityId();
            }

            if (gameObject.name.Contains("PrecursorFabricatorBase"))
            {
                return gameObject.gameObject.GetIdentityId();
            }

            var baseDeconstructable = gameObject.GetComponentInParent<global::BaseDeconstructable>();
            if (baseDeconstructable)
            {
                return baseDeconstructable.gameObject.GetIdentityId();
            }

            return null;
        }

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static TechType GetTechType(GameObject gameObject)
        {
            var constructable = gameObject.GetComponentInParent<Constructable>();
            if (constructable)
            {
                return constructable.techType;
            }

            if (gameObject.GetComponentInParent<MapRoomFunctionality>())
            {
                return TechType.BaseMapRoom;
            }

            if (gameObject.GetComponentInParent<global::PrefabSpawnBase>() || gameObject.name.Contains("PrecursorFabricatorBase"))
            {
                return TechType.Fabricator;
            }

            var baseDeconstructable = gameObject.GetComponentInParent<global::BaseDeconstructable>();
            if (baseDeconstructable)
            {
                return baseDeconstructable.recipe;
            }

            return TechType.None;
        }

        /**
         *
         * Maproom için
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */    
        private static BaseDeconstructable GetBaseMapRoomDeconstructable(MapRoomFunctionality mapRoom)
        {
            var baseComp = mapRoom.GetComponentInParent<global::Base>();
            if (baseComp == null)
            {
                return null;
            }

            var cell = baseComp.NormalizeCell(baseComp.WorldToGrid(mapRoom.transform.position));

            foreach (var component in baseComp.GetComponentsInChildren<BaseDeconstructable>())
            {
                if (component.name == "BaseMapRoom" || component.name == "BaseMapRoom(Clone)")
                {
                    if (baseComp.NormalizeCell(baseComp.WorldToGrid(component.transform.position)) == cell)
                    {
                        return component;
                    }
                }
            }

            return null;
        }
    }
}
