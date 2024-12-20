namespace Subnautica.Server.Logic.Furnitures
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Features;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts;
    using UnityEngine;

    using Metadata    = Subnautica.Network.Models.Metadata;
    using ServerModel = Subnautica.Network.Models.Server;

    public class TechLight : BaseLogic
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
        private float RequiredPower { get; set; } = global::TechLight.powerPerSecond * 2f;

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
                    var gameObject = Network.Identifier.GetComponentByGameObject<global::TechLight>(construction.Value.UniqueId);
                    if (gameObject == null)
                    {
                        continue;
                    }

                    var techLight = construction.Value.EnsureComponent<Metadata.TechLight>();
                    var oldIsPowered = techLight.IsPowered;

                    if (gameObject.powerRelay)
                    {
                        if (gameObject.powerRelay.GetPowerStatus() == PowerSystem.Status.Normal)
                        {
                            if (Core.Server.Instance.Logices.PowerConsumer.ConsumePower(gameObject.powerRelay, this.RequiredPower, out float _))
                            {
                                techLight.IsPowered = true;
                            }
                            else
                            {
                                techLight.IsPowered = false;
                            }
                        }
                        else
                        {
                            techLight.IsPowered = false;
                        }
                    }
                    else
                    {
                        techLight.IsPowered = false;

                        if (!gameObject.searching)
                        {
                            gameObject.searching = true;
                            gameObject.InvokeRepeating("FindNearestValidRelay", 0.0f, 1f);
                        }
                    }

                    if (techLight.IsPowered != oldIsPowered || gameObject.lights.activeSelf != techLight.IsPowered)
                    {
                        this.SendPacketToAllClient(construction.Value.UniqueId, techLight.IsPowered);
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
                TechType  = TechType.Techlight,
                Component = new Metadata.TechLight()
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
            return Core.Server.Instance.Storages.Construction.Storage.Constructions.Where(q => q.Value.TechType == TechType.Techlight && q.Value.ConstructedAmount == 1f).ToList();
        }
    }
}