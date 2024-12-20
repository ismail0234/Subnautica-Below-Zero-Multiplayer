namespace Subnautica.Server.Abstracts.Processors
{
    using Subnautica.Network.Models.Core;

    using Subnautica.Server.Core;

    public abstract class NormalProcessor : BaseProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public abstract bool OnExecute(AuthorizationProfile authorization, NetworkPacket networkPacket);

        /**
         *
         * İşlemi çalıştırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool ExecuteProcessor(AuthorizationProfile profile, NetworkPacket packet)
        {
            if (ProcessorShared.Processors.TryGetValue(packet.Type, out var processor))
            {
                processor.OnExecute(profile, packet);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
