namespace Subnautica.Events.Patches.Events.Player
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    using UnityEngine;

    [HarmonyPatch]
    public class ItemDroping
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Inventory), nameof(global::Inventory.InternalDropItem))]
        private static bool Inventory_InternalDropItem(global::Inventory __instance, ref bool __result, Pickupable pickupable, bool notify = true, bool dropFromPlayerCenter = false)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!global::Inventory.CanDropItemHere(pickupable, notify))
            {
                __result = false;
                return false;
            }

            try
            {
                PlayerItemDropingEventArgs args = new PlayerItemDropingEventArgs(Network.Identifier.GetIdentityId(pickupable.gameObject, false), pickupable, GetDropPosition(dropFromPlayerCenter), GetDropRotation(pickupable));

                Handlers.Player.OnItemDroping(args);

                __result = args.IsAllowed;
                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"ItemDroping.Inventory_InternalDropItem: {e}\n{e.StackTrace}");
                __result = false;
                return false;
            }
        }

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::QuickSlots), nameof(global::QuickSlots.Drop))]
        private static bool QuickSlots_Drop(global::QuickSlots __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance._heldItem == null || !global::Inventory.CanDropItemHere(__instance._heldItem.item, true))
            {
                return false;
            }

            try
            {
                PlayerItemDropingEventArgs args = new PlayerItemDropingEventArgs(__instance._heldItem.item.gameObject.GetIdentityId(), __instance._heldItem.item, GetDropPosition(false), GetDropRotation(__instance._heldItem.item));

                Handlers.Player.OnItemDroping(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"ItemDroping.QuickSlots_Drop: {e}\n{e.StackTrace}");
            }
            
            return false;
        }

        /**
         *
         * Nesnenin bırakılacak pozisyonunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Vector3 GetDropPosition(bool dropFromPlayerCenter)
        {
            Vector3 dropPosition;

            if (dropFromPlayerCenter)
            {
                dropPosition = global::Player.main.playerController.GetPlayerCenterPosition();
            }
            else
            {
                Transform transform = MainCameraControl.main.transform;
                dropPosition = global::Inventory.RayCast(transform.position, transform.forward, 10f, 0.75f, 1.5f);
            }

            return ZeroGame.FindDropPosition(dropPosition);
        }

        /**
         *
         * Nesnenin bırakılacak açılarını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Quaternion GetDropRotation(Pickupable pickupable)
        {
            if (pickupable.randomizeRotationWhenDropped)
            {
                return UnityEngine.Random.rotation;
            }

            return pickupable.transform.rotation;
        }
    }
}