namespace Subnautica.Server.Processors.Metadata
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Features;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using Metadata = Subnautica.Network.Models.Metadata;

    public class ChargerProcessor : MetadataProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(AuthorizationProfile profile, MetadataComponentArgs packet, ConstructionItem construction)
        {
            if (Server.Instance.Logices.Interact.IsBlocked(construction.UniqueId, profile.UniqueId))
            {
                return false;
            }

            var component = packet.Component.GetComponent<Metadata.Charger>();
            if (component == null)
            {
                return false;
            }

            if (component.IsOpening)
            {
                Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, construction.UniqueId, true);

                profile.SendPacketToAllClient(packet);
            }
            else if (component.IsClosing)
            {
                profile.SendPacketToOtherClients(packet);
            }
            else if (component.Items?.Count > 0)
            {
                var constructionComponent = construction.EnsureComponent<Metadata.Charger>();

                foreach (string slotId in this.GetSlots(construction.TechType))
                {
                    if (!constructionComponent.Items.Where(q => q.SlotId == slotId).Any())
                    {
                        constructionComponent.Items.Add(new BatteryItem(slotId));
                    }
                }

                foreach(var _battery in component.Items)
                {
                    var battery = constructionComponent.Items.Where(q => q.SlotId == _battery.SlotId).FirstOrDefault();
                    if (battery != null)
                    {
                        if (component.IsRemoving)
                        {
                            battery.Clear();
                        }
                        else
                        {
                            battery.SetBattery(_battery.TechType, _battery.Charge);
                        }
                    }
                }

                packet.Component = constructionComponent;

                if (Server.Instance.Storages.Construction.UpdateMetadata(packet.UniqueId, constructionComponent))
                {
                    profile.SendPacketToOtherClients(packet);
                }
            }

            return true;
        }

        /**
         *
         * Slotları döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<string> GetSlots(TechType techType)
        {
            if (techType == TechType.BatteryCharger)
            {
                return TechGroup.BatteryChargerSlots;
            }
            else if (techType == TechType.PowerCellCharger)
            {
                return TechGroup.PowerCellChargerSlots;
            }

            return new List<string>();
        }
    }
}