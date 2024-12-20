namespace Subnautica.API.Extensions
{
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Creatures;
    using Subnautica.API.Features.Creatures.Datas;
    using Subnautica.API.Features.Creatures;

    using System;

    using UnityEngine;
    using System.Collections;
    using System.ComponentModel;

    public static class CreatureExtensions
    {
        /**
         *
         * Yaratık numrasını uniqueId değerine çevirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsMultiplayerCreature(this string creatureId)
        {
            return creatureId.Contains("MultiplayerCreature_");
        }

        /**
         *
         * Yaratık numrasını uniqueId değerine çevirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string ToCreatureStringId(this ushort creatureId)
        {
            return string.Format("MultiplayerCreature_" + creatureId);
        }
        
        /**
         *
         * UniqueId değerini yaratık numarasına çevirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ushort ToCreatureId(this string uniqueId)
        {
            return Convert.ToUInt16(uniqueId.Replace("MultiplayerCreature_", ""));
        }

        /**
         *
         * Yaratık bilgisini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsCanBeAttacked(this TechType techType)
        {
            var creatureData = CreatureData.Instance.GetCreatureData(techType);
            return creatureData == null ? false : creatureData.IsCanBeAttacked;
        }
        /**
         *
         * Yaratık bilgisini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static BaseCreatureData GetCreatureData(this TechType techType)
        {
            return CreatureData.Instance.GetCreatureData(techType);
        }

        /**
         *
         * Senkronize edilmiş yaratık olup/olmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsSynchronizedCreature(this TechType techType)
        {
            return CreatureData.Instance.IsExists(techType);
        }

        /**
         *
         * Senkronize edilmiş yaratık olup/olmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsSynchronized(this global::Creature creature)
        {
            return CraftData.GetTechType(creature.gameObject).IsSynchronizedCreature();
        }

        /**
         *
         * Önceki olayı durdurur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void StopPrevAction(this global::Creature creature)
        {
            if (creature.prevBestAction)
            {
                creature.prevBestAction.StopPerform(Time.time);
                creature.prevBestAction = null;
            }
        }

        /**
         *
         * Yaratık nesnesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static MultiplayerCreature GetCreatureObject(this MultiplayerCreatureItem creature)
        {
            if (Network.Creatures.TryGetActiveCreatureObject(creature.Id, out var creatureObject))
            {
                return creatureObject;
            }

            return null;
        }

        /**
         *
         * Yaratık yumurtlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Spawn(this MultiplayerCreatureItem creature)
        {
            var creatureObject = creature.GetCreatureObject();
            if (creatureObject != null)
            {
                creatureObject.SetCreatureItem(creature);
                creatureObject.Spawn();
            }
        }

        /**
         *
         * Yaratık pasif hale getirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Disable(this MultiplayerCreatureItem creature)
        {
            var creatureObject = creature.GetCreatureObject();
            if (creatureObject != null)
            {
                creatureObject.Disable();
            }

            creature.RemoveCreatureObject();
        }

        /**
         *
         * Yaratık sahibini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ChangeOwnership(this MultiplayerCreatureItem creature)
        {
            var creatureObject = creature.GetCreatureObject();
            if (creatureObject != null)
            {
                creatureObject.ChangeOwnership();
            }
        }

        /**
         *
         * Yaratık nesnesini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SetCreatureObject(this MultiplayerCreatureItem creature, MultiplayerCreature creatureObject)
        {
            API.Features.Network.Creatures.SetActiveCreatureObject(creature.Id, creatureObject);
        }

        /**
         *
         * Yaratık nesnesini kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void RemoveCreatureObject(this MultiplayerCreatureItem creature)
        {
            API.Features.Network.Creatures.RemoveActiveCreatureObject(creature.Id);
        }

        /**
         *
         * Yaratık nesnesini kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static IEnumerator BornAsync(this GameObject creatureGameObject, TaskResult<GameObject> taskResult)
        {
            taskResult.Set(null);

            if (creatureGameObject && creatureGameObject.TryGetComponent<global::CreatureEgg>(out var creatureEgg) && creatureEgg.TryGetComponent<global::WaterParkItem>(out var waterParkItem))
            {
                waterParkItem.SetWaterPark(null);

                if (KnownTech.Add(creatureEgg.eggType, false))
                {
                    ErrorMessage.AddMessage(Language.main.GetFormat<string>("EggDiscovered", Language.main.Get(creatureEgg.eggType.AsString())));
                }

                if (creatureEgg.creaturePrefab != null)
                {
                    var result = AddressablesUtility.InstantiateAsync(creatureEgg.creaturePrefab.RuntimeKey.ToString(), position: Vector3.zero, rotation: Quaternion.identity, awake: false);

                    yield return result;

                    var gameObject = result.GetResult();
                    if (gameObject.TryGetComponent<global::WaterParkCreature>(out var waterParkCreature))
                    {
                        waterParkCreature.age = 0.0f;
                        waterParkCreature.bornInside = true;
                        waterParkCreature.SetMatureTime();
                        waterParkCreature.InitializeCreatureBornInWaterPark();
                    }

                    gameObject.EnsureComponent<global::Pickupable>();

                    taskResult.Set(gameObject);
                }

                creatureEgg.liveMixin.Kill();
                creatureEgg.gameObject.Destroy();
            }
        }
    }
}