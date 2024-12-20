namespace Subnautica.Events.Patches.Identity.Building
{
    using HarmonyLib;
    using Subnautica.API.Features;

    [HarmonyPriority(Priority.First)]
    [HarmonyPatch(typeof(Builder), nameof(Builder.TryPlace))]
    public class ConstructingGhostTryPlacing
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix()
        {
            if(Network.IsMultiplayerActive)
            {
                if (Builder.prefab == null || !Builder.canPlace)
                {
                    return;
                }

                Constructable constructable = Builder.ghostModel.GetComponentInParent<Constructable>();
                if (constructable != null)
                {
                    Network.Identifier.CopyToUniqueIdentifier(Builder.ghostModel, constructable.gameObject);
                }
            }
        }
    }
}
