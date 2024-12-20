namespace Subnautica.API.Features.Creatures.Datas
{
    using System.Collections;
    using System.Collections.Generic;

    using Subnautica.API.Enums;
    using Subnautica.API.Features.Creatures.MonoBehaviours;
    using Subnautica.API.Features.Creatures.MonoBehaviours.Shared;
    using Subnautica.API.Features.Creatures.Trackers;

    using UnityEngine;

    using UWE;

    public abstract class BaseCreatureData
    {
        /**
         *
         * Yaratık Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public abstract TechType CreatureType { get; set; }

        /**
         *
         * Yaratık Hasar alabilir mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public abstract bool IsCanBeAttacked { get; set; }

        /**
         *
         * Yaratık Sağlığı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public abstract float Health { get; set; }

        /**
         *
         * Yaratık Görünür mesafesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public abstract float VisibilityDistance { get; set; }

        /**
         *
         * Yaratık Gözükmeme max mesafe
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public abstract float VisibilityLongDistance { get; set; }

        /**
         *
         * Öldükten sonra yeniden canlanabilir mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public abstract bool IsRespawnable { get; set; }

        /**
         *
         * Pasifken Tasma Pozisyonunda Kalması için gereken uzaklık
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public abstract float StayAtLeashPositionWhenPassive { get; set; }

        /**
         *
         * Pasifken Tasma Pozisyonuna kaç saniye sonra ışınlanacak?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual float StayAtLeashPositionTime { get; set; } = 30000f;

        /**
         *
         * Yaratık Respawn Time (Min)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual int RespawnTimeMin { get; set; }

        /**
         *
         * Yaratık Respawn Time (Max)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual int RespawnTimeMax { get; set; }

        /**
         *
         * Fast Sync (Daha iyi yaratık senkronizasyonu, Fakat 2x bant genişliği tüketimi)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual bool IsFastSyncActivated { get; set; }

        /**
         *
         * Doğma Seviyesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual CreatureSpawnLevel SpawnLevel { get; set; } = CreatureSpawnLevel.Default;

        /**
         *
         * Animasyon index numrasını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private byte CurrentAnimationIndex = 0;

        /**
         *
         * Animasyon izleyici olayını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public delegate bool AnimationTrackerAction<T1, T2, T3, T4>(T1 a, T2 b, T3 c, out T4 d);

        /**
         *
         * Animasyon izleyicilerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<byte, BaseAnimationTracker> AnimationTrackers { get; set; } = new Dictionary<byte, BaseAnimationTracker>();

        /**
         *
         * MonoBehaviour'ları entegre eder. (Client Side)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual void OnRegisterMonoBehaviours(MultiplayerCreature creature)
        {
            creature.GameObject.EnsureComponent<MultiplayerCreaturedShared>().SetMultiplayerCreature(creature);
        }

        /**
         *
         * Özel bir yaratık spawnlanmak için kullanılır. (Async)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual IEnumerator OnCustomCreatureSpawnAsync(TaskResult<GameObject> task)
        {
            task.Set(null);
            yield return null;
        }

        /**
         *
         * Özel bir yaratık spawnlanmak için kullanılır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual GameObject OnCustomCreatureSpawn()
        {
            return null;
        }

        /**
         *
         * Yaratık kukla öldüğünde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual bool OnKill(GameObject gameObject)
        {
            return true;
        }

        /**
         *
         * Animasyon izleyicisi ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void AddAnimationTracker(BaseAnimationTracker tracker)
        {
            if (this.CurrentAnimationIndex < 250)
            {
                this.AnimationTrackers[++this.CurrentAnimationIndex] = tracker;
            }
            else
            {
                Log.Error($"Sooo much animation: {this.CurrentAnimationIndex}");
            }
        }
        /**
         *
         * Görünürlük mesafesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float GetVisibilityDistance(bool longDistance = false)
        {
            if (longDistance)
            {
                return this.VisibilityLongDistance * this.VisibilityLongDistance;
            }

            return this.VisibilityDistance * this.VisibilityDistance;
        }

        /**
         *
         * Yaratık Canlanma zamanını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int GetRespawnDuration()
        {
            if (this.RespawnTimeMin == this.RespawnTimeMax)
            {
                return this.RespawnTimeMin;
            }

            return Tools.Random.Next(this.RespawnTimeMin, this.RespawnTimeMax);
        }

        /**
         *
         * Animasyonlara sahip mi?.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool HasAnimationTrackers()
        {
            return this.CurrentAnimationIndex > 0;
        }

        /**
         *
         * Animasyon adını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BaseAnimationTracker GetAnimationTrackerById(byte animationId)
        {
            this.AnimationTrackers.TryGetValue(animationId, out var tracker);
            return tracker;
        }

        /**
         *
         * Animasyonları izleyicilerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Dictionary<byte, BaseAnimationTracker> GetAnimationTrackers()
        {
            return this.AnimationTrackers;
        }
    }
}
