namespace Subnautica.API.Features.Creatures.Datas
{
    using Subnautica.API.Features.Creatures.MonoBehaviours;

    public class VentGardenSmallData : BaseCreatureData
    {
        /**
         *
         * Yaratık Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */  
        public override TechType CreatureType { get; set; } = TechType.SmallVentGarden;

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
        public override float Health { get; set; } = 1f;

        /**
         *
         * Yaratık Görünür mesafesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */  
        public override float VisibilityDistance { get; set; } = 120f;

        /**
         *
         * Yaratık Gözükmeme max mesafe
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */  
        public override float VisibilityLongDistance { get; set; } = 150f;

        /**
         *
         * Pasifken Tasma Pozisyonunda Kalması için gereken uzaklık
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */  
        public override float StayAtLeashPositionWhenPassive { get; set; } = 111f;

        /**
         *
         * Öldükten sonra yeniden canlanabilir mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool IsRespawnable { get; set; } = false;
    }
}
