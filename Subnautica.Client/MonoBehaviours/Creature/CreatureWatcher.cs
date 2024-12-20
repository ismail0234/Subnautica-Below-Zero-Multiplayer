namespace Subnautica.Client.MonoBehaviours.Creature
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Creatures;
    using Subnautica.Client.Core;

    using Subnautica.Network.Models.Server;

    using UnityEngine;

    using ServerModel = Subnautica.Network.Models.Server;

    public class CreatureWatcher : MonoBehaviour
    {
        /**
         *
         * Timing nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StopwatchItem Timing { get; set; } = new StopwatchItem(BroadcastInterval.CreaturePosition);

        /**
         *
         * IsNormalTrigger nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsNormalTrigger { get; set; } = false;

        /**
         *
         * Mesafeyi barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<WorldCreaturePosition> Positions { get; set; } = new List<WorldCreaturePosition>();

        /**
         *
         * Her karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Update()
        {
            if (World.IsLoaded)
            {
                if (this.Timing.IsFinished())
                {
                    this.Timing.Restart();
                    this.IsNormalTrigger = !this.IsNormalTrigger;

                    foreach (var creature in Network.Creatures.GetActiveCreatures())
                    {
                        if (creature.IsMine())
                        {
                            if (creature.Data.IsFastSyncActivated)
                            {
                                this.AddPositionToQueue(creature.Id, creature.GetCreatureObject());
                                continue;
                            }
                            
                            if (this.IsNormalTrigger)
                            {
                                this.AddPositionToQueue(creature.Id, creature.GetCreatureObject());
                                continue;
                            }
                        }
                    }

                    this.SendPositionPacketToServer();
                }

                foreach (var creature in Network.Creatures.GetActiveCreatures())
                {
                    if (creature.IsMine() == false)
                    {
                        creature.GetCreatureObject()?.Movement.SimpleMoveV2();
                    }
                }
            }
        }

        /**
         *
         * Güncellenmiş nesne konumunu kuyruğa alır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void AddPositionToQueue(ushort creatureId, MultiplayerCreature creature)
        {
            if (creature != null && creature.IsActive)
            {
                if (creature.GameObject)
                {
                    this.Positions.Add(new WorldCreaturePosition()
                    {
                        CreatureId = creatureId,
                        Position   = creature.GameObject.transform.position.Compress(),
                        Rotation   = creature.GameObject.transform.rotation.Compress(),
                    });
                }
                else
                {
                    Log.Info("NULL => " + creatureId + ", TYPE => " + creature.CreatureItem.TechType);
                }
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
                    ServerModel.WorldCreaturePositionArgs request = new ServerModel.WorldCreaturePositionArgs()
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

