namespace Subnautica.Events.Patches.Events.Vehicle
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.API.Extensions;

    [HarmonyPatch(typeof(global::ExosuitGrapplingArm), nameof(global::ExosuitGrapplingArm.OnHit))]
    public static class ExosuitGrapplingArm
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::ExosuitGrapplingArm __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            var entity = Network.DynamicEntity.GetEntity(__instance.exosuit.gameObject.GetIdentityId());
            if (entity == null)
            {
                return false;
            }

            return entity.IsMine(ZeroPlayer.CurrentPlayer.UniqueId);
        }
    }
}