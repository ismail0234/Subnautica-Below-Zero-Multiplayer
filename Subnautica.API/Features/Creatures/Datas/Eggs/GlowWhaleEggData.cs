namespace Subnautica.API.Features.Creatures.Datas
{
    using System.Collections;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features.Creatures.MonoBehaviours.Shared;

    using UnityEngine;

    public class GlowWhaleEggData : BaseCreatureData
    {
        /**
         *
         * Yaratık Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override TechType CreatureType { get; set; } = TechType.GlowWhaleEgg;

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
        public override float VisibilityDistance { get; set; } = 60f;

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
        public override float StayAtLeashPositionWhenPassive { get; set; } = 40f;

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
         * Doğma Seviyesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override CreatureSpawnLevel SpawnLevel { get; set; } = CreatureSpawnLevel.CustomAsync;

        /**
         *
         * Özel bir yaratık spawnlanmak için kullanılır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override IEnumerator OnCustomCreatureSpawnAsync(TaskResult<GameObject> task)
        {
            yield return CraftData.InstantiateFromPrefabAsync(this.CreatureType, task);
            yield return task.Get().BornAsync(task);
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

            creature.GameObject.EnsureComponent<MultiplayerWaterParkCreature>().SetMultiplayerCreature(creature);
        }
    }
}