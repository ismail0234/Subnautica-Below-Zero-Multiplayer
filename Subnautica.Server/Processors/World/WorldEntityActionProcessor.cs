﻿namespace Subnautica.Server.Processors.World
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class WorldEntityActionProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.WorldEntityActionArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            WorldEntityProcessor.ExecuteProcessor(profile, packet);
            return true;
        }
    }
}
