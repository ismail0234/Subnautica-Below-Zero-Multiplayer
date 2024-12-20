namespace Subnautica.API.Features
{
    using System;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features.Helper;

    using UnityEngine;

    public class ZeroLiveMixin
    {
        /**
         *
         * Araç sağlığını değiştiri.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void AddHealth(global::LiveMixin liveMixin, float newHealth)
        {
            if (newHealth != 0f)
            {
                var health = liveMixin.health;

                liveMixin.health = newHealth;
                liveMixin.onHealDamage.Trigger(liveMixin.health - health);
                liveMixin.gameObject.SendMessage("OnRepair", liveMixin.GetHealthFraction(), SendMessageOptions.DontRequireReceiver);

                if (liveMixin.health == liveMixin.maxHealth)
                {
                    liveMixin.gameObject.SendMessage("OnRepaired", SendMessageOptions.DontRequireReceiver);
                }

                if (liveMixin.loopingDamageEffect && liveMixin.loopingDamageEffectObj && liveMixin.GetHealthFraction() > liveMixin.loopEffectBelowPercent)
                {
                    GameObject.Destroy(liveMixin.loopingDamageEffectObj);
                }
            }
        }

        /**
         *
         * Hasar bilgisi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendDamageInfoNotify(global::LiveMixin liveMixin, float damage, DamageType type = DamageType.Normal)
        {
            liveMixin.damageInfo.Clear();
            liveMixin.damageInfo.originalDamage = damage;
            liveMixin.damageInfo.damage         = damage;
            liveMixin.damageInfo.position       = liveMixin.transform.position;
            liveMixin.damageInfo.type           = type;
            liveMixin.damageInfo.dealer         = null;
            liveMixin.NotifyAllAttachedDamageReceivers(liveMixin.damageInfo);
        }

        /**
         *
         * Hasar verir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool TakeDamage(global::LiveMixin liveMixin, float newHealth, DamageType type = DamageType.Normal, bool isSound = true, Action<string, GameObject> killAction = null)
        {
            var damage = liveMixin.health - newHealth;
            if (damage < 0)
            {
                damage = 10f;
            }

            if (liveMixin.health > 0.0f)
            {
                liveMixin.health = newHealth;

                SendDamageInfoNotify(liveMixin, damage, type);

                if (liveMixin.shielded)
                {
                    return true;
                }

                if (isSound && damage > 0.0 && damage >= liveMixin.minDamageForSound && type != DamageType.Radiation)
                {
                    if (liveMixin.damageClip)
                    {
                        liveMixin.damageClip.Play();
                    }

                    if (liveMixin.damageSound)
                    {
                        global::Utils.PlayFMODAsset(liveMixin.damageSound, liveMixin.damageInfo.position);
                    }
                }

                if (liveMixin.loopingDamageEffect && !liveMixin.loopingDamageEffectObj && liveMixin.GetHealthFraction() < liveMixin.loopEffectBelowPercent)
                {
                    liveMixin.loopingDamageEffectObj = UWE.Utils.InstantiateWrap(liveMixin.loopingDamageEffect, liveMixin.transform.position, Quaternion.identity);
                    liveMixin.loopingDamageEffectObj.transform.parent = liveMixin.transform;
                }

                if (Time.time > liveMixin.timeLastElecDamageEffect + 2.5 && type == DamageType.Electrical && liveMixin.electricalDamageEffect != null)
                {
                    var component = liveMixin.gameObject.GetComponent<FixedBounds>();
                    var bounds = component == null ? UWE.Utils.GetEncapsulatedAABB(liveMixin.gameObject) : component.bounds;
                    var gameObject = UWE.Utils.InstantiateWrap(liveMixin.electricalDamageEffect, bounds.center, Quaternion.identity);

                    gameObject.transform.parent = liveMixin.transform;
                    gameObject.transform.localScale = bounds.size * 0.65f;
                    liveMixin.timeLastElecDamageEffect = Time.time;
                }
                else if (Time.time > liveMixin.timeLastDamageEffect + 1.0 && damage > 0.0 && liveMixin.damageEffect != null && (type == DamageType.Normal || type == DamageType.Collide || (type == DamageType.Explosive || type == DamageType.Puncture) || (type == DamageType.LaserCutter || type == DamageType.Drill)))
                {
                    global::Utils.SpawnPrefabAt(liveMixin.damageEffect, liveMixin.transform, liveMixin.damageInfo.position);
                    liveMixin.timeLastDamageEffect = Time.time;
                }

                if (type == DamageType.Pressure)
                {
                    if (liveMixin.TryGetComponent<CrushDamage>(out var crushDamage))
                    {
                        if (crushDamage.soundOnDamage)
                        {
                            // Bu zaten otomatik olarak sadece yakındaki kişilere sesi oynatıyor.
                            crushDamage.soundOnDamage.Play();
                        }
                    }
                    else
                    {
                        var baseHull = liveMixin.GetComponentInParent<global::BaseHullStrength>();
                        if (baseHull)
                        {
                            Utils.PlayFMODAsset(baseHull.crushSounds[GetCrushSoundIndex(baseHull.totalStrength)], liveMixin.transform);

                            ErrorMessage.AddMessage(global::Language.main.GetFormat<float>("BaseHullStrDamageDetected", baseHull.totalStrength));
                        }
                    }
                }
            }

            if (newHealth <= 0.0)
            {
                liveMixin.health = 0.0f;
                liveMixin.tempDamage = 0.0f;
                liveMixin.SyncUpdatingState();

                if (liveMixin.deathClip)
                {
                    liveMixin.deathClip.Play();
                }

                if (liveMixin.deathSound)
                {
                    global::Utils.PlayFMODAsset(liveMixin.deathSound, liveMixin.transform);
                }

                if (liveMixin.deathEffect != null)
                {
                    UWE.Utils.InstantiateWrap(liveMixin.deathEffect, liveMixin.transform.position, Quaternion.identity);
                }

                if (liveMixin.passDamageDataOnDeath)
                {
                    liveMixin.gameObject.BroadcastMessage("OnKill", type, SendMessageOptions.DontRequireReceiver);
                }
                else if (liveMixin.broadcastKillOnDeath)
                {
                    liveMixin.gameObject.BroadcastMessage("OnKill", SendMessageOptions.DontRequireReceiver);
                }

                if (liveMixin.sendKillOnDeath)
                {
                    if (liveMixin.passDamageDataOnDeath)
                    {
                        liveMixin.gameObject.SendMessage("OnKill", type, SendMessageOptions.DontRequireReceiver);
                    }
                    else
                    {
                        liveMixin.gameObject.SendMessage("OnKill", SendMessageOptions.DontRequireReceiver);
                    }
                }

                var uniqueId = Network.Identifier.GetIdentityId(liveMixin.gameObject, false);
                if (uniqueId.IsNotNull())
                {
                    if (liveMixin.destroyOnDeath || CraftData.GetTechType(liveMixin.gameObject).IsVehicle())
                    {
                        var action = new ItemQueueAction();
                        action.RegisterProperty("UniqueId", uniqueId);
                        action.RegisterProperty("Action"  , killAction);
                        action.OnProcessCompleted = OnKillProcessCompleted;

                        Entity.ProcessToQueue(action);
                    }
                }
            }

            return true;
        }

        /**
         *
         * İşlem tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void OnKillProcessCompleted(ItemQueueProcess item)
        {
            var uniqueId = item.Action.GetProperty<string>("UniqueId");
            var action   = item.Action.GetProperty<Action<string, GameObject>>("Action");
            if (uniqueId.IsNotNull())
            {
                var gameObject = Network.Identifier.GetGameObject(uniqueId);
                if (gameObject)
                {
                    try
                    {
                        action?.Invoke(uniqueId, gameObject);
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"ZeroLiveMixin.OnKillProcessCompleted: {ex}");
                    }
                    finally
                    {
                        World.DestroyGameObject(gameObject);

                        Network.DynamicEntity.RemoveEntity(uniqueId);
                    }
                }
            }
        }

        /**
         *
         * Basınç sesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static int GetCrushSoundIndex(float totalStrength)
        {
            var index = 0;
            if (totalStrength <= -3.0)
            {
                index = 2;
            }
            else if (totalStrength <= -2.0)
            {
                index = 1;
            }

            return index;
        }
    }
}
