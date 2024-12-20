namespace Subnautica.Client.Abstracts
{
    using Subnautica.Network.Models.Core;

    public abstract class NormalProcessor : BaseProcessor
    {

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public abstract bool OnDataReceived(NetworkPacket networkPacket);
    }
}
