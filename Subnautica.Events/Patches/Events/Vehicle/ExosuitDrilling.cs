namespace Subnautica.Events.Patches.Events.Vehicle
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Structures;

    using System;
    using System.Collections.Generic;

    using UnityEngine;

    [HarmonyPatch(typeof(global::Drillable), nameof(global::Drillable.OnDrill))]
    public static class ExosuitDrilling
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::Drillable __instance, Vector3 position, Exosuit exo, out GameObject hitObject)
        {
            if (!Network.IsMultiplayerActive)
            {
                hitObject = null;
                return true;
            }

            hitObject = __instance.renderers[__instance.FindClosestMesh(position, out var center)].gameObject;

            __instance.drillingExo = exo;

            if (__instance.timeLastDrilled + 0.5f < Time.time)
            {
                __instance.timeLastDrilled = Time.time;
                __instance.modelRoot.transform.position = __instance.transform.position + __instance.modelRootOffset + new Vector3(Mathf.Sin(Time.time * 60f), Mathf.Cos((float)((double)Time.time * 58.0 + 0.5)), Mathf.Cos((float)((double)Time.time * 64.0 + 2.0))) * (11f / 1000f);

                if (ExosuitDrilling.GetLeftHealth(__instance) > 0f)
                {
                    try
                    {
                        ExosuitDrillingEventArgs args = new ExosuitDrillingEventArgs(exo.gameObject.GetIdentityId(), __instance.gameObject.GetIdentityId(), __instance.health.Length * 200f, GetRandomTechType(__instance), GetDropPositions(__instance, center), IsMultipleDrill(exo));

                        Handlers.Vehicle.OnExosuitDrilling(args);

                        return args.IsAllowed;
                    }
                    catch (Exception e)
                    {
                        Log.Error($"ExosuitDrilling.Prefix: {e}\n{e.StackTrace}");
                    }
                }
            }

            return false;
        }

        /**
         *
         * Kazım yapan kol sayısını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool IsMultipleDrill(global::Exosuit exo)
        {
            var drillCount = 0;
            drillCount += exo.mainAnimator.GetBool("use_tool_left") ? 1 : 0;
            drillCount += exo.mainAnimator.GetBool("use_tool_right") ? 1 : 0;

            return drillCount >= 2;
        }

        /**
         *
         * Kalan sağlığı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static float GetLeftHealth(global::Drillable __instance)
        {
            float health = 0.0f;
            for (int index = 0; index < __instance.health.Length; ++index)
            {
                health += __instance.health[index];
            }

            return health;
        }

        /**
         *
         * Rastgele teknoloji döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static TechType GetRandomTechType(global::Drillable __instance)
        {
            for (int index = 0; index < __instance.resources.Length; ++index)
            {
                var resource = __instance.resources[index];
                if (resource.chance >= 1.0f)
                {
                    return resource.techType;
                }

                if (global::Player.main.gameObject.GetComponent<PlayerEntropy>().CheckChance(resource.techType, resource.chance))
                {
                    return resource.techType;
                }
            }

            return TechType.Titanium;
        }

        /**
         *
         * Rastgele konumlar döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static List<ZeroVector3> GetDropPositions(global::Drillable __instance, Vector3 centerPosition)
        {
            var positions = new List<ZeroVector3>();
            for (int i = 0; i < 3; i++)
            {
                float max = 1f;
                
                var position = centerPosition;

                position.x += UnityEngine.Random.Range(-max, max);
                position.z += UnityEngine.Random.Range(-max, max);
                position.y += UnityEngine.Random.Range(-max, max);

                __instance.ClipWithTerrain(ref position);

                positions.Add(position.ToZeroVector3());
            }

            return positions;
        }
    }
}