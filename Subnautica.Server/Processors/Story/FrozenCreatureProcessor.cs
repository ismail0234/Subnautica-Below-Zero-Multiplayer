namespace Subnautica.Server.Processors.Story
{
    using Server.Core;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class FrozenCreatureProcessor : NormalProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnExecute(AuthorizationProfile profile, NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.StoryFrozenCreatureArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (packet.CinematicType == StoryCinematicType.StoryFrozenCreatureSample)
            {
                if (Server.Instance.Logices.StoryTrigger.IsCompleteableCinematic(packet.CinematicType.ToString()))
                {
                    if (Server.Instance.Storages.Story.Storage.FrozenCreature.AddSample())
                    {
                        Server.Instance.Logices.StoryTrigger.CompleteTrigger(packet.CinematicType.ToString());

                        profile.SendPacketToAllClient(packet);
                    }
                }
            }
            else if (packet.CinematicType == StoryCinematicType.StoryFrozenCreatureInject)
            {
                if (Server.Instance.Logices.StoryTrigger.IsCompleteableCinematic(packet.CinematicType.ToString()))
                {
                    if (Server.Instance.Storages.Story.Storage.FrozenCreature.Inject(Server.Instance.Logices.World.GetServerTime()))
                    {
                        Server.Instance.Logices.StoryTrigger.CompleteTrigger(packet.CinematicType.ToString());

                        packet.InjectTime = Server.Instance.Storages.Story.Storage.FrozenCreature.InjectTime;

                        profile.SendPacketToAllClient(packet);
                    }
                }
            }

            return true;
        }
    }
}
