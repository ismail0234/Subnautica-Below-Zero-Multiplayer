namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::BulkheadDoor), nameof(global::BulkheadDoor.OnHandHover))]
    public class BulkheadDoor
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::BulkheadDoor __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (Network.HandTarget.IsBlocked(BulkheadDoor.GetUniqueId(__instance)))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }

        /**
         *
         * UniqueId döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string GetUniqueId(global::BulkheadDoor __instance)
        {
            var constructable = __instance.GetComponentInParent<BaseDeconstructable>();
            if (constructable)
            {
                return Network.Identifier.GetIdentityId(constructable.gameObject);
            }

            var door = __instance.GetComponent<global::BulkheadDoor>();
            if (door)
            {
                return Network.Identifier.GetIdentityId(door.gameObject, false);
            }

            return null;
        }
    }
}
