namespace Subnautica.Client.Synchronizations.Processors.World
{
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using UnityEngine;

    using ServerModel = Subnautica.Network.Models.Server;

    public class WelderProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.WelderArgs>();
            if (packet == null)
            {
                return false;
            }

            var action = new ItemQueueAction();
            action.OnProcessCompleted = this.OnProcessCompleted;
            action.RegisterProperty("NewHealth", packet.Health);
            action.RegisterProperty("TechType" , packet.TechType);
            action.RegisterProperty("UniqueId" , packet.UniqueId);
            action.RegisterProperty("IsMine"   , ZeroPlayer.IsPlayerMine(packet.GetPacketOwnerId()));

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
            var liveMixin = this.GetLiveMixin(item.Action.GetProperty<string>("UniqueId"));
            if (liveMixin)
            {
                ZeroLiveMixin.AddHealth(liveMixin, item.Action.GetProperty<float>("NewHealth"));
            }

            if (item.Action.GetProperty<bool>("IsMine"))
            {
                var handItem = global::Inventory.Get().quickSlots.heldItem;
                if (handItem != null && handItem.item.TryGetComponent<Welder>(out var welder))
                {
                    welder.energyMixin.ConsumeEnergy(welder.weldEnergyCost);
                    welder.timeLastWelded = Time.time;

                    if (welder.fxControl != null && !welder.fxIsPlaying)
                    {
                        welder.fxControl.Play(global::Player.main.IsUnderwater() ? 1 : 0);
                        welder.fxIsPlaying = true;
                    }
                }
            }
        }

        /**
         *
         * LiveMixin sınıfını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::LiveMixin GetLiveMixin(string uniqueId)
        {
            var gameObject = Network.Identifier.GetGameObject(uniqueId);
            if (gameObject == null)
            {
                return null;
            }

            var liveMixin = gameObject.GetComponentInChildren<global::LiveMixin>();
            if (liveMixin)
            {
                return liveMixin;
            }

            return gameObject.GetComponentInParent<global::LiveMixin>();
        }

        /**
         *
         * Bir araç veya yapı tamir edilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnWelding(WeldingEventArgs ev)
        {
            ev.IsAllowed = false;

            ServerModel.WelderArgs result = new ServerModel.WelderArgs()
            {
                UniqueId = ev.UniqueId,
                TechType = ev.TechType,
                Health   = ev.Health,
            };

            NetworkClient.SendPacket(result);
        }
    }
}
