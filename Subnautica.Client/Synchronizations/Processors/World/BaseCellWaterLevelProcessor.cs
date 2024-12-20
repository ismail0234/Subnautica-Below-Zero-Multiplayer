namespace Subnautica.Client.Synchronizations.Processors.World
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.MonoBehaviours.Construction;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class BaseCellWaterLevelProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.BaseCellWaterLevelArgs>();
            if (packet == null)
            {
                return false;
            }

            var baseFloodSim = Network.Identifier.GetComponentByGameObject<global::BaseFloodSim>(packet.UniqueId);
            if (baseFloodSim == null)
            {
                return false;
            }

            var baseHullStrength = baseFloodSim.gameObject.EnsureComponent<BuilderBaseHullStrength>();
            if (baseHullStrength == null)
            {
                return false;
            }

            foreach (var level in packet.Levels)
            {
                baseHullStrength.SetCellWaterLevel(level.Key, level.Value.ToFloat());
            }

            return true;
        }
    }
}
