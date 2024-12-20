namespace Subnautica.Client.Synchronizations.Processors.Creatures
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Creatures;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using UnityEngine;

    using ServerModel = Subnautica.Network.Models.Server;

    public class FreezeProcessor : NormalProcessor
    {
        /**
        *
        * Gelen veriyi işler
        *
        * @author Ismail <ismaiil_0234@hotmail.com>
        *
        */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.CreatureFreezeArgs>();
            if (packet == null)
            {
                return false;
            }

            var action = new CreatureQueueAction();
            action.OnProcessCompleted = this.OnCreatureProcessCompleted;
            action.RegisterProperty("EndTime"    , packet.EndTime);
            action.RegisterProperty("IsInfinity" , packet.IsInfinityLifeTime());
            action.RegisterProperty("BrinicleId" , packet.BrinicleId);

            Network.Creatures.ProcessToQueue(packet.CreatureId, action);
            return true;
        }

        /**
         *
         * İşlem tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCreatureProcessCompleted(MultiplayerCreature creature, CreatureQueueItem item)
        {
            var endTime    = item.Action.GetProperty<float>("EndTime");
            var isInfinity = item.Action.GetProperty<bool>("IsInfinity");
            var brinicleId = item.Action.GetProperty<string>("BrinicleId");
            var lifeTime   = endTime - Network.Session.GetWorldTime();

            if (creature.GameObject.TryGetComponent<global::CreatureFrozenMixin>(out var frozenMixin))
            {
                using (EventBlocker.Create(ProcessType.CreatureFreeze))
                {
                    if (brinicleId.IsNotNull() && creature.CreatureItem.IsMine())
                    {
                        var brinicle = Network.Identifier.GetComponentByGameObject<global::Brinicle>(brinicleId);
                        if (brinicle)
                        {
                            brinicle.frozenTargets.Add(creature.GameObject);
                        }
                    }
                        
                    if (isInfinity)
                    {
                        frozenMixin.Freeze(float.PositiveInfinity, true);
                    }
                    else
                    {
                        frozenMixin.Freeze(Time.time + (float)lifeTime, false);
                    }
                }
            }
        }

        /**
         *
         * Balık donarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnFreezing(CreatureFreezingEventArgs ev)
        {
            ev.IsAllowed = false;

            if (ev.UniqueId.IsMultiplayerCreature())
            {
                FreezeProcessor.SendPacketToServer(ev.UniqueId.ToCreatureId(), ev.LifeTime, ev.BrinicleId);
            }
        }

        /**
         *
         * Sunucuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SendPacketToServer(ushort creatureId, float lifeTime, string brinicleId)
        {
            ServerModel.CreatureFreezeArgs request = new ServerModel.CreatureFreezeArgs()
            {
                CreatureId = creatureId,
                LifeTime   = lifeTime,
                BrinicleId = brinicleId
            };

            NetworkClient.SendPacket(request);
        }
    }
}