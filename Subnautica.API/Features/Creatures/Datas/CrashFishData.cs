namespace Subnautica.API.Features.Creatures.Datas
{
    using Subnautica.API.Features.Creatures.MonoBehaviours;
    using Subnautica.API.Features.Creatures.Trackers;

    using UnityEngine;

    public class CrashFishData : BaseCreatureData
    {
        /**
         *
         * Yaratık Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */  
        public override TechType CreatureType { get; set; } = TechType.Crash;

        /**
         *
         * Yaratık Hasar alabilir mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */  
        public override bool IsCanBeAttacked { get; set; } = false;

        /**
         *
         * Yaratık Sağlığı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */  
        public override float Health { get; set; } = 25f;

        /**
         *
         * Yaratık Görünür mesafesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */  
        public override float VisibilityDistance { get; set; } = 50f;

        /**
         *
         * Yaratık Gözükmeme mesafe
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */  
        public override float VisibilityLongDistance { get; set; } = 80f;

        /**
         *
         * Pasifken Tasma Pozisyonunda Kalması için gereken uzaklık
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */  
        public override float StayAtLeashPositionWhenPassive { get; set; } = 0f;

        /**
         *
         * Pasifken Tasma Pozisyonuna kaç saniye sonra ışınlanacak?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */  
        public override float StayAtLeashPositionTime { get; set; } = 0f;

        /**
         *
         * Öldükten sonra yeniden canlanabilir mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool IsRespawnable { get; set; } = true;

        /**
         *
         * Yaratık Respawn Time (Min)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override int RespawnTimeMin { get; set; } = 900;

        /**
         *
         * Yaratık Respawn Time (Max)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */  
        public override int RespawnTimeMax { get; set; } = 1200;

        /**
         *
         * Sınıf özelliklerini ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CrashFishData()
        {
            this.AddAnimationTracker(new ProtectCrashHomeTracker());
        }

        /**
         *
         * MonoBehaviour'ları entegre eder. (Client Side)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnRegisterMonoBehaviours(MultiplayerCreature creature)
        {
            base.OnRegisterMonoBehaviours(creature);
            
            creature.GameObject.EnsureComponent<CrashFishMonobehaviour>().SetMultiplayerCreature(creature);
        }

        /**
         *
         * MonoBehaviour'ları entegre eder. (Client Side)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnKill(GameObject gameObject)
        {
            if (gameObject.TryGetComponent<global::Crash>(out var crash))
            {
                SafeAnimator.SetBool(crash.GetAnimator(), "explode", true);

                if (crash.detonateParticlePrefab)
                {
                    Utils.PlayOneShotPS(crash.detonateParticlePrefab, crash.transform.position, crash.transform.rotation);
                }

                DamageSystem.RadiusDamage(crash.maxDamage, crash.transform.position, crash.detonateRadius, DamageType.Explosive, crash.gameObject);
            }

            gameObject.GetComponent<LiveMixin>().Kill();
            return false;
        }
    }
}
