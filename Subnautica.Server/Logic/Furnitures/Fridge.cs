namespace Subnautica.Server.Logic.Furnitures
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts;
    using Subnautica.Server.Core;

    using Metadata    = Subnautica.Network.Models.Metadata;
    using ServerModel = Subnautica.Network.Models.Server;

    public class Fridge : BaseLogic
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
         * Zamanlayıcının geri dönüş methodu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnUpdate(float deltaTime)
        {
            if (this.Timing.IsFinished() && World.IsLoaded)
            {
                this.Timing.Restart();

                foreach (var construction in this.GetFridges())
                {
                    var component = construction.Value.EnsureComponent<Metadata.Fridge>();
                    if (component.Components.Count <= 0)
                    {
                        continue;
                    }

                    var fridge = Network.Identifier.GetComponentByGameObject<global::Fridge>(construction.Value.UniqueId);
                    if (fridge == null)
                    {
                        continue;
                    }

                    var wasPowered = fridge.powerConsumer.IsPowered();

                    if (component.WasPowered == wasPowered)
                    {
                        continue;
                    }

                    var serverTime = Server.Instance.Logices.World.GetServerTime();

                    foreach (var item in component.Components)
                    {
                        if (wasPowered)
                        {
                            item.PauseDecay(serverTime);
                        }
                        else
                        {
                            item.UnpauseDecay(serverTime);
                        }
                    }
                    
                    component.WasPowered = wasPowered;

                    this.SendPacketToAllClient(construction.Value.UniqueId, serverTime, wasPowered);
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
        private void SendPacketToAllClient(string uniqueId, float serverTime, bool wasPowered)
        {
            ServerModel.MetadataComponentArgs request = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = uniqueId,
                TechType  = TechType.Fridge,
                Component = new Metadata.Fridge()
                {
                    WasPowered          = wasPowered,
                    CurrentTime         = serverTime,
                    IsPowerStateChanged = true
                },
            };

            Core.Server.SendPacketToAllClient(request);
        }

        /**
         *
         * Buz dolabının güç durumunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsPowered(string constructionId)
        {
            var fridge = Network.Identifier.GetComponentByGameObject<global::Fridge>(constructionId);
            if (fridge == null)
            {
                return false;
            }

            return fridge.powerConsumer.IsPowered();   
        }

        /**
         *
         * Buz dolaplarını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<KeyValuePair<string, ConstructionItem>> GetFridges()
        {
            return Core.Server.Instance.Storages.Construction.Storage.Constructions.Where(q => q.Value.TechType == TechType.Fridge && q.Value.ConstructedAmount == 1f).ToList();
        }
    }
}
