namespace Subnautica.Server.Logic
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Server.Abstracts;

    public class BaseHullStrength : BaseLogic
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
         * Su seviyelerinin önbelleğini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<int, float> WaterLeveLCache { get; set; } = new Dictionary<int, float>();

        /**
         *
         * İstekleri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<string, Dictionary<ushort, byte>> Requests = new Dictionary<string, Dictionary<ushort, byte>>();

        /**
         *
         * Her tick'de tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnFixedUpdate(float deltaTime)
        {
            if (this.Timing.IsFinished() && API.Features.World.IsLoaded)
            {
                this.Timing.Restart();

                foreach (var baseComp in this.GetBases())
                {
                    var baseFloodSim = Network.Identifier.GetComponentByGameObject<global::BaseFloodSim>(baseComp.BaseId, true);
                    if (baseFloodSim && baseFloodSim.baseComp)
                    {
                        this.CheckWaterLevels(baseComp.BaseId, baseFloodSim);
                    }
                }

                if (this.Requests.Count > 0)
                {
                    this.SendPacketToAllClient();
                }
            }
        }

        /**
         *
         * Üs dayanıklılığı düştüğünde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCrushing(BaseHullStrengthCrushingEventArgs ev)
        {
            var random = ev.Instance.victims.GetRandom<global::LiveMixin>();
            if (random)
            {
                random.TakeDamage(10f, random.transform.position, DamageType.Pressure);
            }
        }

        /**
         *
         * Su seviyelerini kontrol eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool CheckWaterLevels(string baseId, global::BaseFloodSim baseFloodSim)
        {
            var maxSize = baseFloodSim.shape.Size;
            if (maxSize > ushort.MaxValue)
            {
                maxSize = ushort.MaxValue;
            }

            var count = 0;
            for (ushort index = 0; index < maxSize; index++)
            {
                if (((int)baseFloodSim.baseComp?.flowData[index] & 64) > 0)
                {
                    count++;
                    if (this.WaterLeveLCache.TryGetValue(index, out var waterCell))
                    {
                        if ((baseFloodSim.cellWaterLevel[index] <= 0f && waterCell != baseFloodSim.cellWaterLevel[index]) || waterCell.Approximately(baseFloodSim.cellWaterLevel[index], 0.01f))
                        {
                            this.WaterLeveLCache[index] = baseFloodSim.cellWaterLevel[index];
                            this.OnCellWaterLevelChanged(baseId, index, baseFloodSim.cellWaterLevel[index]);
                        }
                    }
                    else
                    {
                        this.WaterLeveLCache[index] = baseFloodSim.cellWaterLevel[index];
                        this.OnCellWaterLevelChanged(baseId, index, baseFloodSim.cellWaterLevel[index]);
                    }
                }
            }

            return true;
        }

        /**
         *
         * Oyunculara veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SendPacketToAllClient()
        {
            foreach (var requests in this.Requests)
            {
                foreach (var item in requests.Value.Split(225))
                {
                    BaseCellWaterLevelArgs packet = new BaseCellWaterLevelArgs()
                    {
                        UniqueId = requests.Key,
                        Levels   = item
                    };

                    foreach (var player in Core.Server.Instance.GetPlayers())
                    {
                        if (!player.IsHost)
                        {
                            player.SendPacket(packet);
                        }
                    }
                }
            }

            this.Requests.Clear();
        }

        /**
         *
         * Su seviyesi değiştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void OnCellWaterLevelChanged(string uniqueId, ushort index, float waterLevel)
        {
            if (this.Requests.TryGetValue(uniqueId, out var requests))
            {
                requests.Add(index, waterLevel > 2.49f ? (byte)250 : waterLevel.ToByte());
            }
            else
            {
                this.Requests[uniqueId] = new Dictionary<ushort, byte>()
                {
                    { index, waterLevel > 2.49f ? (byte)250 : waterLevel.ToByte() }
                };
            }

            if (Core.Server.Instance.Storages.World.TryGetBase(uniqueId, out var baseComponent))
            {
                baseComponent.SetCellWaterLevel(index, waterLevel);
            }
        }

        /**
         *
         * Üsleri döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<Base> GetBases()
        {
            return Core.Server.Instance.Storages.World.Storage.Bases.ToList();
        }
    }
}
