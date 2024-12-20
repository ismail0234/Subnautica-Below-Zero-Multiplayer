namespace Subnautica.Events.Patches.Events.Items
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    [HarmonyPatch(typeof(global::Welder), nameof(global::Welder.Weld))]
    public class Welding
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::Welder __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                if (__instance.activeWeldTarget == null || !__instance.gameObject.TryGetComponent<EnergyMixin>(out var energyMixin) || energyMixin.IsDepleted() || !__instance.CanWeldTarget(__instance.activeWeldTarget))
                {
                    return false;
                }

                if (!__instance.SupplyWeldMaterial(__instance.activeWeldTarget))
                {
                    return false;
                }

                if (__instance.activeWeldTarget.health >= __instance.activeWeldTarget.maxHealth)
                {
                    return false;
                }

                try
                {
                    WeldingEventArgs args = new WeldingEventArgs(GetUniqueId(__instance.activeWeldTarget.gameObject), GetTechType(__instance.activeWeldTarget.gameObject), __instance.healthPerWeld);

                    Handlers.Items.OnWelding(args);

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"Welding.Prefix: {e}\n{e.StackTrace}");
                }
            }

            return true;
        }

        /**
         *
         * UniqueId değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string GetUniqueId(GameObject gameObject)
        {
            if (GetTechType(gameObject).IsVehicle())
            {
                return Network.Identifier.GetIdentityId(gameObject);
            }

            var baseDeconstructable = gameObject.GetComponentInChildren<global::BaseDeconstructable>();
            if (baseDeconstructable)
            {
                return Network.Identifier.GetIdentityId(baseDeconstructable.gameObject);
            }

            return Network.Identifier.GetIdentityId(gameObject);
        }

        /**
         *
         * TechType değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static TechType GetTechType(GameObject gameObject)
        {
            var techType = CraftData.GetTechType(gameObject);
            if (techType != TechType.None)
            {
                return techType;
            }

            var baseDeconstructable = gameObject.GetComponentInChildren<global::BaseDeconstructable>();
            if (baseDeconstructable)
            {
                return baseDeconstructable.recipe;
            }

            return TechType.None;
        }
    }
}