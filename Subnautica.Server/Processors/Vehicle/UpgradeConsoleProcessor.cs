namespace Subnautica.Server.Processors.Vehicle
{
    using System;
    using System.Collections.Generic;

    using Server.Core;

    using Subnautica.API.Extensions;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel      = Subnautica.Network.Models.Server;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class UpgradeConsoleProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.VehicleUpgradeConsoleArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (packet.IsOpening)
            {
                if (!Server.Instance.Logices.Interact.IsBlocked(packet.UniqueId))
                {
                    Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, packet.UniqueId, true);

                    profile.SendPacket(packet);
                }
            }
            else
            {
                if (packet.ItemId.IsNull())
                {
                    return false;
                }

                var entity = Server.Instance.Storages.World.GetVehicle(packet.UniqueId);
                if (entity == null)
                {
                    return false;
                }

                var isSendPacket = false;
                var slotId       = this.GetSlotNumber(packet.SlotId);

                switch (entity.TechType)
                {
                    case TechType.Hoverbike:

                        var hoverbikeComp = entity.Component.GetComponent<WorldEntityModel.Hoverbike>();
                        if (packet.IsAdding)
                        {
                            isSendPacket = this.AddModule(hoverbikeComp.Modules, slotId, packet.ModuleType, packet.ItemId);
                        }
                        else
                        {
                            isSendPacket = this.RemoveModule(hoverbikeComp.Modules, slotId);
                        }

                        break;
                    case TechType.Exosuit:

                        var exosuitComp = entity.Component.GetComponent<WorldEntityModel.Exosuit>();
                        if (packet.IsAdding)
                        {
                            isSendPacket = this.AddModule(exosuitComp.Modules, slotId, packet.ModuleType, packet.ItemId);
                        }
                        else
                        {
                            isSendPacket = this.RemoveModule(exosuitComp.Modules, slotId);
                        }

                        break;
                    case TechType.SeaTruck:

                        var seaTruckComp = entity.Component.GetComponent<WorldEntityModel.SeaTruck>();
                        if (packet.IsAdding)
                        {
                            isSendPacket = this.AddModule(seaTruckComp.Modules, slotId, packet.ModuleType, packet.ItemId);
                        }
                        else
                        {
                            isSendPacket = this.RemoveModule(seaTruckComp.Modules, slotId);
                        }

                        break;
                }


                if (isSendPacket)
                {
                    profile.SendPacketToOtherClients(packet);
                }
            }

            return true;
        }
        
        /**
         *
         * Modülü ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool AddModule(List<UpgradeConsoleItem> modules, int slotId, TechType moduleType, string itemId)
        {
            if (modules[slotId].ModuleType == TechType.None)
            {
                modules[slotId].ModuleType = moduleType;
                modules[slotId].ItemId     = itemId;
                
                return true;
            }

            return false;
        }
        
        /**
         *
         * Modülü kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool RemoveModule(List<UpgradeConsoleItem> modules, int slotId)
        {
            if (modules[slotId].ModuleType != TechType.None)
            {
                modules[slotId].ModuleType = TechType.None;
                modules[slotId].ItemId     = null;
                
                return true;
            }

            return false;
        }

        /**
         *
         * Slot numarasını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private int GetSlotNumber(string slotId)
        {
            if (slotId == "ExosuitArmLeft")
            {
                return 4;
            }

            if (slotId == "ExosuitArmRight")
            {
                return 5;
            }

            return Convert.ToInt32(slotId.Replace("ExosuitModule", "").Replace("HoverbikeModule", "").Replace("SeaTruckModule", "")) - 1;
        }
    }
}
