namespace Subnautica.API.Features.Creatures.Datas
{
    using Subnautica.API.Features.Creatures.Trackers;

    public class ArcticRayData : BaseCreatureData
    {
        /**
         *
         * Yaratık Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override TechType CreatureType { get; set; } = TechType.ArcticRay;

        /**
         *
         * Yaratık Hasar alabilir mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool IsCanBeAttacked { get; set; } = true;

        /**
         *
         * Yaratık Sağlığı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override float Health { get; set; } = 100f;

        /**
         *
         * Yaratık Görünür mesafesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override float VisibilityDistance { get; set; } = 75f;

        /**
         *
         * Yaratık Gözükmeme mesafe
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override float VisibilityLongDistance { get; set; } = 95f;

        /**
         *
         * Pasifken Tasma Pozisyonunda Kalması için gereken uzaklık
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override float StayAtLeashPositionWhenPassive { get; set; } = 60f;

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
        public override int RespawnTimeMin { get; set; } = 420;

        /**
         *
         * Yaratık Respawn Time (Max)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override int RespawnTimeMax { get; set; } = 420;

        /**
         *
         * Sınıf özelliklerini ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ArcticRayData()
        {
            this.AddAnimationTracker(new ArcticRayAnimationTracker());
        }
    }
}
