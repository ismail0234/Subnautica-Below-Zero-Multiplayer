namespace Subnautica.Client.Synchronizations.Processors.Vehicle
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Client.MonoBehaviours.Player;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using UnityEngine;

    using ServerModel = Subnautica.Network.Models.Server;

    public class HealthProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.VehicleHealthArgs>();
            if (packet.UniqueId.IsNull())
            {
                return false;
            }

            var action = new ItemQueueAction();
            action.OnProcessCompleted = this.OnProcessCompleted;
            action.RegisterProperty("NewHealth" , packet.NewHealth);
            action.RegisterProperty("UniqueId"  , packet.UniqueId);
            action.RegisterProperty("DamageType", packet.DamageType);

            Entity.ProcessToQueue(action);
            return true;
        }

        /**
         *
         * İşlem tamamlandıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnProcessCompleted(ItemQueueProcess item)
        {
            var uniqueId   = item.Action.GetProperty<string>("UniqueId");
            var newHealth  = item.Action.GetProperty<float>("NewHealth");
            var damageType = item.Action.GetProperty<DamageType>("DamageType");
            var liveMixin  = Network.Identifier.GetComponentByGameObject<global::LiveMixin>(uniqueId);
            if (liveMixin)
            {
                ZeroLiveMixin.TakeDamage(liveMixin, newHealth, damageType, killAction: this.OnVehicleDestroyed);
            }
        }

        /**
         *
         * Araç patlamadan önce tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void OnVehicleDestroyed(string uniqueId, GameObject gameObject)
        {
            var entity = Network.DynamicEntity.GetEntity(uniqueId);
            if (entity != null)
            {
                foreach (var player in ZeroPlayer.GetPlayers())
                {
                    if (player.VehicleId == entity.Id)
                    {
                        player.ExitVehicle();
                    }
                    
                    if (player.CurrentInteriorId == entity.UniqueId)
                    {
                        player.SetInteriorId(null);
                        player.GetComponent<PlayerAnimation>().UpdateIsInSeaTruck(true);
                    }
                }
            }
        }

        /**
         *
         * Bir nesne hasar aldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnTakeDamaging(TakeDamagingEventArgs ev)
        {
            if (ev.TechType.IsVehicle())
            {
                ev.IsAllowed = false;

                if (ev.Damage > 0.0f)
                {
                    HealthProcessor.SendPacketToServer(ev.UniqueId, ev.Damage, ev.DamageType);
                }
            }
        }

        /**
         *
         * Basınç hasarı alınınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCrushDamaging(CrushDamagingEventArgs ev)
        {
            if (ev.TechType.IsVehicle())
            {
                ev.IsAllowed = false;

                if (Network.DynamicEntity.IsMine(ev.UniqueId))
                {
                    HealthProcessor.SendPacketToServer(ev.UniqueId, ev.Damage, DamageType.Pressure);
                }
            }
        }

        /**
         *
         * Sunucuya paketi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, float damage, DamageType damageType)
        {
            ServerModel.VehicleHealthArgs request = new ServerModel.VehicleHealthArgs()
            {
                UniqueId   = uniqueId,
                DamageType = damageType,
                Damage     = damage,
            };

            NetworkClient.SendPacket(request);
        }
    }
}