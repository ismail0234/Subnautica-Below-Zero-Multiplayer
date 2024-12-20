namespace Subnautica.API.Features.Creatures.Datas
{
    using Subnautica.API.Features.Creatures.MonoBehaviours.Shared;

    public class ChelicerateData : BaseCreatureData
    {
        /**
         *
         * Yaratık Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override TechType CreatureType { get; set; } = TechType.Chelicerate;

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
        public override float Health { get; set; } = 5000f;

        /**
         *
         * Yaratık Görünür mesafesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override float VisibilityDistance { get; set; } = 150f;

        /**
         *
         * Yaratık Gözükmeme mesafe
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override float VisibilityLongDistance { get; set; } = 200f;

        /**
         *
         * Pasifken Tasma Pozisyonunda Kalması için gereken uzaklık
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override float StayAtLeashPositionWhenPassive { get; set; } = 100f;

        /**
         *
         * Öldükten sonra yeniden canlanabilir mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool IsRespawnable { get; set; } = false;

        /**
         *
         * Fast Sync (Daha iyi yaratık senkronizasyonu, Fakat 2x bant genişliği tüketimi)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool IsFastSyncActivated { get; set; } = true;

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
            creature.GameObject.EnsureComponent<MultiplayerLeviathanMeleeAttack>().SetMultiplayerCreature(creature);
        }
    }
}
