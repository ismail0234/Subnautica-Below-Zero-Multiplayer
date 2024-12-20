namespace Subnautica.Events.Patches.Events.World
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    using UnityEngine;

    [HarmonyPatch(typeof(global::LiveMixin), nameof(global::LiveMixin.TakeDamage))]
    public static class TakeDamaging
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::LiveMixin __instance, float originalDamage, Vector3 position, DamageType type, GameObject dealer)
        {
            if (Network.IsMultiplayerActive && !__instance.invincible && __instance.health > 0.0f)
            {
                var techType     = TakeDamaging.GetTechType(__instance.gameObject);
                var damageTaken  = GameModeManager.GetDamageTakenModifier(techType, __instance.GetComponent<BaseCell>() != null);

                if (damageTaken != 0.0f)
                {
                    float damage = 0.0f;
                    if (!__instance.shielded)
                    {
                        damage = DamageSystem.CalculateDamage(techType, damageTaken, originalDamage, type, __instance.gameObject, dealer);
                    }

                    float newHealth = Mathf.Max(0.0f, __instance.health - damage);

                    try
                    {
                        TakeDamagingEventArgs args = new TakeDamagingEventArgs(__instance, techType, damage, __instance.health, __instance.maxHealth, newHealth, type, __instance.destroyOnDeath, dealer);

                        Handlers.World.OnTakeDamaging(args);

                        return args.IsAllowed;
                    }
                    catch (Exception e)
                    {
                        Log.Error($"TakeDamaging.Prefix: {e}\n{e.StackTrace}");
                        return false;
                    }
                }
            }

            return true;
        }

        /**
         *
         * TechType döner.
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

            if (gameObject.TryGetComponent<global::Brinicle>(out var _))
            {
                return TechType.Brinicle;
            }

            return TechType.None;
        }
    }
}