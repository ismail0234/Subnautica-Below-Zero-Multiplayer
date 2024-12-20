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

    public class CoffeeVendingMachine : BaseLogic
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

                foreach (var construction in this.GetCoffeeVendingMachines())
                {
                    var component = construction.Value.EnsureComponent<Metadata.CoffeeVendingMachine>();
                    var thermoses = component.Thermoses.Where(q => q.IsActive && !q.IsFull);
                    if (thermoses.Count() <= 0)
                    {
                        continue;
                    }

                    var machine = Network.Identifier.GetComponentByGameObject<global::CoffeeVendingMachine>(construction.Value.UniqueId);
                    if (machine == null)
                    {
                        continue;
                    }

                    var wasPowered = machine.IsPowered();
                    var serverTime = Server.Instance.Logices.World.GetServerTime();

                    foreach (var thermos in thermoses)
                    {
                        if (wasPowered && wasPowered != component.WasPowered)
                        {
                            thermos.AddedTime = serverTime;
                        }

                        if (wasPowered && thermos.AddedTime + global::CoffeeVendingMachine.refillTime <= serverTime)
                        {
                            thermos.Refill();

                            this.SendPacketToAllClient(construction.Value.UniqueId, thermos.ItemId);
                        }
                    }

                    component.WasPowered = wasPowered;
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
        private void SendPacketToAllClient(string uniqueId, string itemId)
        {
            ServerModel.MetadataComponentArgs request = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = uniqueId,
                TechType  = TechType.CoffeeVendingMachine,
                Component = new Metadata.CoffeeVendingMachine()
                {
                    IsAdding = true,
                    IsFull   = true,
                    ItemId   = itemId,
                },
            };

            Core.Server.SendPacketToAllClient(request);
        }

        /**
         *
         * Kahve makinelerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<KeyValuePair<string, ConstructionItem>> GetCoffeeVendingMachines()
        {
            return Core.Server.Instance.Storages.Construction.Storage.Constructions.Where(q => q.Value.TechType == TechType.CoffeeVendingMachine && q.Value.ConstructedAmount == 1f).ToList();
        }
    }
}
