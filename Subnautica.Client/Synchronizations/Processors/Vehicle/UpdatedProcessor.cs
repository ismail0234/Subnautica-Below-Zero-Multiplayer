namespace Subnautica.Client.Synchronizations.Processors.Vehicle
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Server;

    using UnityEngine;

    using ServerModel = Subnautica.Network.Models.Server;

    public class UpdatedProcessor : NormalProcessor
    {
        /**
         *
         * ExosuitUpdateComponent nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static ExosuitUpdateComponent ExosuitUpdateComponent { get; set; } = new ExosuitUpdateComponent();

        /**
         *
         * SpyPenguinUpdateComponent nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static SpyPenguinUpdateComponent SpyPenguinUpdateComponent { get; set; } = new SpyPenguinUpdateComponent();

        /**
         *
         * HoverbikeUpdateComponent nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static HoverbikeUpdateComponent HoverbikeUpdateComponent { get; set; } = new HoverbikeUpdateComponent();

        /**
         *
         * SpyPenguinAnimations nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static List<string> SpyPenguinAnimations { get; set; } = new List<string>();

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.VehicleUpdatedArgs>();
            if (packet.EntityId == 0)
            {
                return false;
            }

            var player = ZeroPlayer.GetPlayerById(packet.PlayerId);
            if (player == null)
            {
                return false;
            }

            player.SetVehicle(packet.EntityId, packet.Position, packet.Rotation, packet.Component);
            return true;
        }

        /**
         *
         * Araç konumu güncellendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnVehicleUpdated(VehicleUpdatedEventArgs ev)
        {
            var entity = Network.DynamicEntity.GetEntity(ev.UniqueId);
            if (entity != null)
            {
                ServerModel.VehicleUpdatedArgs request = new ServerModel.VehicleUpdatedArgs()
                {
                    EntityId  = entity.Id,
                    Position  = ev.Position.ToZeroVector3(),
                    Rotation  = ev.Rotation.ToZeroQuaternion(),
                    Component = GetVehicleComponent(ev.TechType, ev.Instance),
                };

                NetworkClient.SendPacket(request);
            }
        }

        /**
         *
         * Spy Penguin bir animasyon halinde iken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSpyPenguinItemGrabing(SpyPenguinItemGrabingEventArgs ev)
        {
            SpyPenguinAnimations.Add(ev.AnimationName);
        }

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static VehicleUpdateComponent GetVehicleComponent(TechType techType, GameObject gameObject)
        {
            if (gameObject == null)
            {
                return null;
            }

            if (techType == TechType.Exosuit)
            {
                var exosuit = gameObject.GetComponentInParent<global::Exosuit>();
                if (exosuit)
                {
                    return Components.Exosuit.GetComponent(ExosuitUpdateComponent, exosuit);
                }
            }
            else if (techType == TechType.Hoverbike)
            {
                var exosuit = gameObject.GetComponentInParent<global::Hoverbike>();
                if (exosuit)
                {
                    return Components.Hoverbike.GetComponent(HoverbikeUpdateComponent, exosuit);
                }
            }
            else if (techType == TechType.SpyPenguin)
            {
                var spyPenguin = gameObject.GetComponent<global::SpyPenguin>();
                if (spyPenguin)
                {
                    var component = Components.SpyPenguin.GetComponent(SpyPenguinUpdateComponent, spyPenguin, SpyPenguinAnimations.ToList());
                    SpyPenguinAnimations.Clear();
                    return component;
                }
            }

            return null;
        }
    }
}