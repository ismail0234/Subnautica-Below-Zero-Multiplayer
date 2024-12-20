namespace Subnautica.API.Features.Creatures.Datas
{
    using Subnautica.API.Features.Creatures.MonoBehaviours.Shared;

    public class BruteSharkData : BaseCreatureData
    {
        /**
         *
         * Yaratık Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override TechType CreatureType { get; set; } = TechType.BruteShark;

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
        public override float Health { get; set; } = 200f;

        /**
         *
         * Yaratık Görünür mesafesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override float VisibilityDistance { get; set; } = 90f;

        /**
         *
         * Yaratık Gözükmeme mesafe
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override float VisibilityLongDistance { get; set; } = 115f;

        /**
         *
         * Pasifken Tasma Pozisyonunda Kalması için gereken uzaklık
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override float StayAtLeashPositionWhenPassive { get; set; } = 50f;

        /**
         *
         * Pasifken Tasma Pozisyonuna kaç saniye sonra ışınlanacak?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override float StayAtLeashPositionTime { get; set; } = 15000f;

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
        public override int RespawnTimeMin { get; set; } = 600;

        /**
         *
         * Yaratık Respawn Time (Max)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */  
        public override int RespawnTimeMax { get; set; } = 600;

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

            creature.GameObject.EnsureComponent<MultiplayerMeleeAttack>().SetMultiplayerCreature(creature);
            creature.GameObject.EnsureComponent<MultiplayerAttackLastTarget>().SetMultiplayerCreature(creature);
            creature.GameObject.EnsureComponent<MultiplayerCreatureAggressionManager>().SetMultiplayerCreature(creature);
        }
    }
}
