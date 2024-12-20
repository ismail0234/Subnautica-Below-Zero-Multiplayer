namespace Subnautica.Client.Synchronizations.Processors.Metadata
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Server;

    using ServerModel = Subnautica.Network.Models.Server;
    using Metadata    = Subnautica.Network.Models.Metadata;

    public class NuclearReactorProcessor : MetadataProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(string uniqueId, TechType techType, MetadataComponentArgs packet, bool isSilence)
        {
            var component = packet.Component.GetComponent<Metadata.NuclearReactor>();
            if (component == null)
            {
                return false;
            }

            var gameObject = Network.Identifier.GetComponentByGameObject<global::BaseNuclearReactorGeometry>(uniqueId);
            if (gameObject == null)
            {
                return false;
            }

            var nuclearReactor = gameObject.GetModule();
            if (nuclearReactor == null)
            {
                return false;
            }
            
            if (component.Items != null && component.Items.Count > 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    var itemType = component.Items.ElementAt(i);
                    var slotId   = global::BaseNuclearReactor.slotIDs[i];

                    if (itemType == TechType.None)
                    {
                        Entity.RemoveToQueue(slotId, nuclearReactor.equipment);
                    }
                    else if (itemType == TechType.DepletedReactorRod || itemType == TechType.ReactorRod)
                    {
                        Entity.SpawnToQueue(slotId, itemType, nuclearReactor.equipment, new ItemQueueAction(this.OnEntitySpawning));
                    }
                }
            }

            return true;
        }

        /**
         *
         * Nesne spawnlanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool OnEntitySpawning(ItemQueueProcess item)
        {
            if (item.Equipment == null)
            {
                return false;
            }

            var itemInSlot = item.Equipment.GetItemInSlot(item.SlotId);
            if (itemInSlot == null || itemInSlot.item.GetTechType() != item.TechType)
            {
                return true;
            }

            return false;
        }

        /**
         *
         * Nükleer Depolamaya eşya eklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnNuclearReactorItemAdded(NuclearReactorItemAddedEventArgs ev)
        {
            NuclearReactorProcessor.SendDataToServer(ev.ConstructionId, TechType.ReactorRod, ev.SlotId, false);
        }

        /**
         *
         * Nükleer Depolamadan eşya kaldırıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnNuclearReactorItemRemoved(NuclearReactorItemRemovedEventArgs ev)
        {
            NuclearReactorProcessor.SendDataToServer(ev.ConstructionId, TechType.DepletedReactorRod, ev.SlotId, true);
        }

        /**
         *
         * Sunucuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SendDataToServer(string uniqueId, TechType techType, string slotId, bool isRemoving)
        {
            var items = new List<TechType>();

            if (!string.IsNullOrEmpty(slotId))
            {
                for (int i = 0; i < 4; i++)
                {
                    if (slotId == global::BaseNuclearReactor.slotIDs[i])
                    {
                        items.Add(techType);
                    }
                    else
                    {
                        items.Add(TechType.None);
                    }
                }
            }

            ServerModel.MetadataComponentArgs result = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = uniqueId,
                Component = new Metadata.NuclearReactor()
                {
                    IsRemoving = isRemoving,
                    Items      = items,
                },
            };

            NetworkClient.SendPacket(result);
        }
    }
}