namespace Subnautica.Client.Synchronizations.Processors.Player
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Extensions;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;
    
    public class FirstInitialEquipmentProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.PlayerInitialEquipmentArgs>();
            if (packet == null)
            {
                return false;
            }

            foreach (var initialEquipment in global::Player.creativeEquipment)
            {
                CraftData.AddToInventory(initialEquipment.techType, initialEquipment.count);
            }

            global::Inventory.main.quickSlots.Select(0);
            return true;
        }
    }
}