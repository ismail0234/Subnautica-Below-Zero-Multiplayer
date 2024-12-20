namespace Subnautica.Client.Synchronizations.Processors.Player
{
    using System.Linq;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class AnimationChangedProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.PlayerAnimationChangedArgs>();
            if (packet.GetPacketOwnerId() == 0)
            {
                return false;
            }

            var player = ZeroPlayer.GetPlayerById(packet.GetPacketOwnerId());
            if (player != null && !player.IsMine)
            {
                player.SetAnimationQueue(packet.Animations.ToDictionary(q => q.Key.ToEnumString(), q => q.Value));
            }

            return true;
        }

        /**
         *
         * Oyuncu Animasyonu değiştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerAnimationChanged(PlayerAnimationChangedEventArgs ev)
        {
            if (World.IsLoaded)
            {
                ServerModel.PlayerAnimationChangedArgs result = new ServerModel.PlayerAnimationChangedArgs()
                {
                    Animations = ev.Animations
                };

                NetworkClient.SendPacket(result);
            }
        }
    }
}
