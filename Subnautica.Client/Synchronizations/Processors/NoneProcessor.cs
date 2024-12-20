namespace Subnautica.Client.Synchronizations.Processors
{
    using Subnautica.Client.Abstracts;
    using Subnautica.Network.Models.Core;
    using System;

    public class NoneProcessor : NormalProcessor
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
            throw new NotImplementedException();
        }
    }
}
