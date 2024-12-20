namespace Subnautica.Client.Synchronizations.Processors.Metadata
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Features;
    using Subnautica.API.Enums;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.API.Features.Helper;

    using UnityEngine.EventSystems;

    using ServerModel = Subnautica.Network.Models.Server;
    using Metadata    = Subnautica.Network.Models.Metadata;
    using UnityEngine;

    public class ChargerProcessor : MetadataProcessor
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
            var component = packet.Component.GetComponent<Metadata.Charger>();
            if (component == null)
            {
                return false;
            }

            var gameObject = Network.Identifier.GetComponentByGameObject<global::Charger>(uniqueId);
            if (gameObject == null)
            {
                return false;
            }

            if (isSilence)
            {
                using (EventBlocker.Create(ProcessType.MetadataRequest))
                {
                    gameObject.equipment.ClearItems();
                }
                
                gameObject.opened = component.Items != null && component.Items.Where(q => q.IsActive).Any();
                gameObject.animator.SetBool(gameObject.animParamOpen, gameObject.opened);
                gameObject.animator.Play(gameObject.opened ? gameObject.animatorOpenedStateName : gameObject.animatorClosedStateName);
                gameObject.ToggleUI(gameObject.opened);
            }

            if (component.IsClosing)
            {
                gameObject.opened = false;
                gameObject.sequence.Reset();
                gameObject.animator.SetBool(gameObject.animParamOpen, false);
                gameObject.ToggleUI(false);

                if (gameObject.soundClose != null)
                {
                    FMODUWE.PlayOneShot(gameObject.soundClose, gameObject.transform.position);
                }
            }
            else if (component.IsOpening)
            {
                ZeroPlayer player = ZeroPlayer.GetPlayerById(packet.GetPacketOwnerId());
                if (player == null)
                {
                    return false;
                }

                if (player.IsMine)
                {
                    using (EventBlocker.Create(techType))
                    {
                        gameObject.OnHandClick(new HandTargetEventData(EventSystem.current) {
                            guiHand = global::Player.main.guiHand
                        });
                    }
                }
                else
                {
                    gameObject.opened = true;
                    gameObject.OnOpen();
                    gameObject.UpdateVisuals();
                }
            }
            else
            {
                foreach(BatteryItem battery in component.Items)
                {
                    ItemQueueAction action = new ItemQueueAction();
                    action.RegisterProperty("BatteryItem", battery);
                    action.RegisterProperty("Charger"    , gameObject);

                    if (battery.IsActive)
                    {
                        action.OnEntitySpawning = this.OnEntitySpawning;
                        action.OnEntitySpawned  = this.OnEntitySpawned;

                        Entity.SpawnToQueue(battery.SlotId, battery.TechType, gameObject.equipment, action);
                    }
                    else
                    {
                        action.OnEntityRemoved = this.OnEntityRemoved;
                        
                        Entity.RemoveToQueue(battery.SlotId, gameObject.equipment);
                    }
                }
            }

            return true;
        }

        /**
         *
         * Nesne yok edildikten sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEntityRemoved(ItemQueueProcess item)
        {
            var battery = item.Action.GetProperty<BatteryItem>("BatteryItem");
            var charger = item.Action.GetProperty<Charger>("Charger");
            if (battery != null && battery != null)
            {
                charger.batteries[battery.SlotId] = null;

                if (charger.slots.TryGetValue(battery.SlotId, out var definition))
                {
                    charger.UpdateVisuals(definition, -1f, TechType.None);
                }
            }
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
            if (itemInSlot == null)
            {
                return true;
            }

            var battery = item.Action.GetProperty<BatteryItem>("BatteryItem");
            if (itemInSlot.item.GetTechType() != battery.TechType)
            {
                return true;
            }

            return false;
        }

        /**
         *
         * Nesne spawnlandıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEntitySpawned(ItemQueueProcess item, Pickupable pickupable, GameObject gameObject)
        {
            var battery = item.Action.GetProperty<BatteryItem>("BatteryItem");
            var charger = item.Action.GetProperty<Charger>("Charger");
            if (battery != null && charger != null)
            {
                pickupable.GetComponent<Battery>().charge = battery.Charge;

                if (charger.slots.TryGetValue(battery.SlotId, out var definition))
                {
                    charger.UpdateVisuals(definition, battery.Charge / battery.Capacity, battery.TechType);
                }
            }
        }

        /**
         *
         * Şarj cihazına pil eklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnChargerItemAdded(ChargerItemAddedEventArgs ev)
        {
            ChargerProcessor.SendDataToServer(ev.ConstructionId, ev.Item.GetTechType(), ev.SlotId, ev.Item.GetComponent<Battery>().charge, false, false, false);
        }

        /**
         *
         * Şarj cihazın'dan pil kaldırılınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnChargerItemRemoved(ChargerItemRemovedEventArgs ev)
        {
            ChargerProcessor.SendDataToServer(ev.ConstructionId, ev.Item.GetTechType(), ev.SlotId, 0.0f, false, true, false);
        }

        /**
         *
         * Şarj cihazına tıklanınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnChargerOpening(ChargerOpeningEventArgs ev)
        {
            ev.IsAllowed = false;

            if (!Interact.IsBlocked(ev.UniqueId))
            {
                ChargerProcessor.SendDataToServer(ev.UniqueId, TechType.None, null, 0.0f, true, false, false);
            }
        }

        /**
         *
         * PDA kapatıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnClosing(PDAClosingEventArgs ev)
        {
            if (ev.TechType == TechType.BatteryCharger || ev.TechType == TechType.PowerCellCharger)
            {
                if (!string.IsNullOrEmpty(ev.UniqueId) && Interact.IsBlockedByMe(ev.UniqueId))
                {
                    var charger = Network.Identifier.GetComponentByGameObject<global::Charger>(ev.UniqueId);
                    if (charger && !charger.HasChargables())
                    {
                        ChargerProcessor.SendDataToServer(ev.UniqueId, TechType.None, null, 0.0f, false, false, true);
                    }
                }
            }
        }

        /**
         *
         * Sunucuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SendDataToServer(string uniqueId, TechType techType, string slotId, float currentCharge, bool isOpening, bool isRemoving, bool isClosing)
        {
            ServerModel.MetadataComponentArgs result = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = uniqueId,
                Component = new Metadata.Charger()
                {
                    IsOpening  = isOpening,
                    IsRemoving = isRemoving,
                    IsClosing  = isClosing,
                    Items      = new List<BatteryItem>() 
                    {
                        new BatteryItem(slotId, techType, currentCharge)
                    },
                },
            };

            NetworkClient.SendPacket(result);
        }
    }
}