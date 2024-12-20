namespace Subnautica.Server.Logic.Furnitures
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Metadata;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts;
    using Subnautica.Server.Core;

    using Metadata         = Subnautica.Network.Models.Metadata;
    using ServerModel      = Subnautica.Network.Models.Server;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class Bed : BaseLogic
    {
        /**
         *
         * Timing nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StopwatchItem Timing { get; set; } = new StopwatchItem(1000f);

        /**
         *
         * SleepGameTimeDuration Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float SleepGameTimeDuration { get; set; } = 396f;

        /**
         *
         * SleepRealTimeDuration Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float SleepRealTimeDuration { get; set; } = 5f;

        /**
         *
         * Sınıfı başlatır
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnStart()
        {
            foreach (var bed in this.GetBeds())
            {
                if (bed.IsUsing())
                {
                    bed.Standup();
                }
            }
        }

        /**
         *
         * Zamanlayıcının geri dönüş methodu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            if (this.Timing.IsFinished() && World.IsLoaded)
            {
                this.Timing.Restart();

                if (this.IsTimeLastSleepAvailable() && !Server.Instance.Storages.World.Storage.SkipTimeMode)
                {
                    var sleepingPlayerCount = this.GetBeds().Count(q => q.IsSleeping(Server.Instance.Logices.World.GetServerTime()));
                    if (sleepingPlayerCount > 0 && sleepingPlayerCount >= Server.Instance.Players.Count && this.SkipTime(this.SleepGameTimeDuration, this.SleepRealTimeDuration))
                    {
                        Server.Instance.Storages.World.Storage.TimeLastSleep = Server.Instance.Logices.World.GetServerTime();

                        this.SendPacketToAllClient();
                    }
                }
            }
        }

        /**
         *
         * Oyuncunun yataklarını siler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void ClearPlayerBeds(byte playerId)
        {
            foreach (var bed in this.GetBeds())
            {
                if (bed.PlayerId_v2 == playerId)
                {
                    bed.Standup();
                }
            }
        }

        /**
         *
         * Oyunculara paketleri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SendPacketToAllClient()
        {
            ServerModel.SleepTimeSkipArgs request = new ServerModel.SleepTimeSkipArgs()
            {
                TimeLastSleep   = Server.Instance.Storages.World.Storage.TimeLastSleep,
                SkipModeEndTime = Server.Instance.Storages.World.Storage.SkipModeEndTime,
                TimeAmount      = this.SleepGameTimeDuration,
                SkipDuration    = this.SleepRealTimeDuration,
            };

            Core.Server.SendPacketToAllClient(request); 
        }

        /**
         *
         * Uyku zamanı üzerinden geçen süreyi kontrol eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsTimeLastSleepAvailable()
        {
            return Server.Instance.Storages.World.Storage.TimeLastSleep + 600f <= Server.Instance.Logices.World.GetServerTime();
        }

        /**
         *
         * Zamanı ileri sarar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool SkipTime(float timeAmount, float skipDuration)
        {
            if (Server.Instance.Storages.World.Storage.SkipTimeMode || timeAmount <= 0.0 || skipDuration <= 0.0)
            {
                return false;
            }

            Server.Instance.Storages.World.Storage.SkipTimeMode    = true;
            Server.Instance.Storages.World.Storage.SkipModeEndTime = Server.Instance.Logices.World.GetServerTime() + timeAmount;
            Server.Instance.Storages.World.Storage.WorldSpeed      = timeAmount / skipDuration;
            return true;
        }

        /**
         *
         * Yatakları döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<BedSideItem> GetBeds()
        {
            var beds = new List<BedSideItem>();

            foreach (var item in Server.Instance.Storages.Construction.Storage.Constructions.Where(q => q.Value.ConstructedAmount == 1f && API.Features.TechGroup.Beds.Contains(q.Value.TechType)))
            {
                beds.AddRange(item.Value.EnsureComponent<Metadata.Bed>().Sides);
            }

            foreach (var item in Server.Instance.Storages.World.Storage.DynamicEntities.Where(q => q.TechType == TechType.SeaTruckSleeperModule))
            {
                beds.Add(item.Component.GetComponent<WorldEntityModel.SeaTruckSleeperModule>().Bed);
            }

            return beds;
        }
    }
}
