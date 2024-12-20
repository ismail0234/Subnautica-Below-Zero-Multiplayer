namespace Subnautica.API.Features.Creatures.Datas
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features.Creatures.MonoBehaviours.Shared;

    using UnityEngine;

    public class VoidLeviathanData : BaseCreatureData
    {
        /**
         *
         * Yaratık Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override TechType CreatureType { get; set; } = TechType.GhostLeviathan;

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
        public override float VisibilityDistance { get; set; } = 220f;

        /**
         *
         * Yaratık Gözükmeme mesafe
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override float VisibilityLongDistance { get; set; } = 250f;

        /**
         *
         * Pasifken Tasma Pozisyonunda Kalması için gereken uzaklık
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override float StayAtLeashPositionWhenPassive { get; set; } = 150f;

        /**
         *
         * Pasifken Tasma Pozisyonuna kaç saniye sonra ışınlanacak?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override float StayAtLeashPositionTime { get; set; } = 10000f;

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
         * Doğma Seviyesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override CreatureSpawnLevel SpawnLevel { get; set; } = CreatureSpawnLevel.Custom;

        /**
         *
         * Özel bir yaratık spawnlanmak için kullanılır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override GameObject OnCustomCreatureSpawn()
        {
            var gameObject = UnityEngine.Object.Instantiate<GameObject>(VoidLeviathansSpawner.main.ghostLeviathanPrefab, Vector3.zero, Quaternion.identity, false);
            gameObject.SetTechType(TechType.GhostLeviathan);
            gameObject.SetIdentityId(Network.Identifier.GenerateUniqueId());
            return gameObject;
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

            creature.GameObject.EnsureComponent<MultiplayerMeleeAttack>().SetMultiplayerCreature(creature);
            creature.GameObject.EnsureComponent<MultiplayerAttackLastTarget>().SetMultiplayerCreature(creature);
        }
    }
}
