namespace Subnautica.Events.Patches.Events.Building
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(Builder), nameof(Builder.TryPlace))]
    public class ConstructionGhostTryPlacing
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix()
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            try
            {
                ConstructionGhostTryPlacingEventArgs args = new ConstructionGhostTryPlacingEventArgs(
                    Builder.ghostModel,
                    Network.Identifier.GetIdentityId(Builder.ghostModel), 
                    global::Player.main.GetCurrentSub() == null ? null : Network.Identifier.GetIdentityId(global::Player.main.GetCurrentSub().gameObject, false), 
                    Builder.lastTechType, 
                    Builder.lastRotation, 
                    Builder.placePosition, 
                    Builder.placeRotation, 
                    Builder.GetAimTransform(), 
                    Builder.canPlace,
                    Builder.ghostModel.GetComponentInParent<ConstructableBase>(),
                    Builder.prefab == null || Builder.canPlace == false
                );

                Handlers.Building.OnConstructingGhostTryPlacing(args);

                if (!args.IsAllowed)
                {
                    Builder.End();
                }

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"ConstructingGhostTryPlacing.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}
