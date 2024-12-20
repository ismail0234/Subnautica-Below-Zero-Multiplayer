namespace Subnautica.Client.MonoBehaviours.Entity.Components
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Core;
    using Subnautica.Network.Models.Storage.World.Childrens;

    using ServerModel = Subnautica.Network.Models.Server;

    public class EntityPosition
    {
        /**
         *
         * Timing nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StopwatchItem Timing { get; set; } = new StopwatchItem(200f);

        /**
         *
         * Konumları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<WorldDynamicEntityPosition> Positions { get; set; } = new List<WorldDynamicEntityPosition>();

        /**
         *
         * Her karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Update()
        {
            if (this.Timing.IsFinished())
            {
                this.Timing.Restart();

                foreach (var entityId in Network.DynamicEntity.GetActivatedEntityIds())
                {
                    var entity = Network.DynamicEntity.GetEntity(entityId);
                    if (entity == null || entity.IsUsingByPlayer || !entity.IsMine(ZeroPlayer.CurrentPlayer.UniqueId) || entity.ParentId.IsNotNull())
                    {
                        continue;
                    }

                    this.EntityPositionToQueue(entity);
                }

                this.SendPositionPacketToServer();
            }
        }

        /**
         *
         * Güncellenmiş nesne konumunu kuyruğa alır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void EntityPositionToQueue(WorldDynamicEntity entity)
        {
            entity.UpdateGameObject();

            if (entity.GameObject)
            {
                entity.Position = entity.GameObject.transform.position.ToZeroVector3();
                entity.Rotation = entity.GameObject.transform.rotation.ToZeroQuaternion();

                this.Positions.Add(new WorldDynamicEntityPosition() {
                    Id       = entity.Id,
                    Position = entity.Position.Compress(),
                    Rotation = entity.Rotation.Compress(),
                });
            }
        }

        /**
         *
         * Konum verilerini sunucuya gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SendPositionPacketToServer()
        {
            if (this.Positions.Count > 0)
            {
                foreach (var positions in this.Positions.Split(21))
                {
                    ServerModel.WorldDynamicEntityPositionArgs request = new ServerModel.WorldDynamicEntityPositionArgs()
                    {
                        Positions = positions.ToList(),
                    };

                    NetworkClient.SendPacket(request);
                }

                this.Positions.Clear();
            }
        }
    }
}
