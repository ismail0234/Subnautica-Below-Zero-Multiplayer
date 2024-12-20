namespace Subnautica.Server.Logic.Furnitures
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Features;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts;

    using Metadata    = Subnautica.Network.Models.Metadata;
    using ServerModel = Subnautica.Network.Models.Server;

    public class SpotLight : BaseLogic
    {
        /**
         *
         * Timing nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private StopwatchItem Timing { get; set; } = new StopwatchItem(2000f);

        /**
         *
         * RequiredPower nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float RequiredPower { get; set; } = global::BaseSpotLight.powerPerSecond * 2f;

        /**
         *
         * Her tick'de tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            if (this.Timing.IsFinished() && World.IsLoaded)
            {
                this.Timing.Restart();

                foreach (var construction in this.GetConstructions())
                {
                    var baseSpotLight = Network.Identifier.GetComponentByGameObject<global::BaseSpotLight>(construction.Value.UniqueId);
                    if (baseSpotLight == null)
                    {
                        continue;
                    }

                    var isPowered = baseSpotLight.powerConsumer.IsPowered() && Core.Server.Instance.Logices.PowerConsumer.HasPower(baseSpotLight.powerConsumer, this.RequiredPower);
                    if (isPowered)
                    {
                        Core.Server.Instance.Logices.PowerConsumer.ConsumePower(baseSpotLight.powerConsumer, this.RequiredPower, out float _);
                    }

                    var spotLight = construction.Value.EnsureComponent<Metadata.SpotLight>();
                    if (spotLight.IsPowered != isPowered)
                    {
                        spotLight.IsPowered = isPowered;

                        this.SendPacketToAllClient(construction.Value.UniqueId, spotLight.IsPowered);
                    }
                }
            }
        }

        /**
         *
         * Tüm kullanıcılara paketi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SendPacketToAllClient(string uniqueId, bool isPowered)
        {
            ServerModel.MetadataComponentArgs request = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = uniqueId,
                TechType  = TechType.Spotlight,
                Component = new Metadata.SpotLight()
                {
                    IsPowered = isPowered
                },
            };

            Core.Server.SendPacketToAllClient(request);
        }

        /**
         *
         * Enerji sağlayan yapıları döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<KeyValuePair<string, ConstructionItem>> GetConstructions()
        {
            return Core.Server.Instance.Storages.Construction.Storage.Constructions.Where(q => q.Value.TechType == TechType.Spotlight && q.Value.ConstructedAmount == 1f).ToList();
        }
    }
}