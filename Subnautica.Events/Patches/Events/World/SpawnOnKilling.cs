namespace Subnautica.Events.Patches.Events.World
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    [HarmonyPatch(typeof(global::LiveMixin), nameof(global::LiveMixin.Kill))]
    public class SpawnOnKilling
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::LiveMixin __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.TryGetComponent<global::SpawnOnKill>(out var gameObject))
            {
                __instance.health = 1f;

               try
               {
                    SpawnOnKillingEventArgs args = new SpawnOnKillingEventArgs(Network.Identifier.GetIdentityId(gameObject.gameObject), CraftData.GetTechType(gameObject.prefabToSpawn), gameObject.transform.position, gameObject.transform.rotation, GetVelocity(gameObject), ForceMode.Impulse);

                    Handlers.World.OnSpawnOnKilling(args);

                    return args.IsAllowed;
               }
               catch (Exception e)
               {
                    Log.Error($"SpawnOnKill.Prefix: {e}\n{e.StackTrace}");
               }             
            }

            return true;
        }

        /**
         *
         * Hızı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Vector3 GetVelocity(global::SpawnOnKill __instance)
        {
            if (__instance.randomPush)
            {
                if (__instance.prefabToSpawn.gameObject.TryGetComponent<Rigidbody>(out var component))
                {
                    return UnityEngine.Random.onUnitSphere * 1.4f;
                }

                return Vector3.zero;
            }

            return Vector3.zero;
        }
    }
}