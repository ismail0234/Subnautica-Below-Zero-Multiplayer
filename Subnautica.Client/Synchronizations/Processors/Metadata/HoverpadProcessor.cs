namespace Subnautica.Client.Synchronizations.Processors.Metadata
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using Metadata         = Subnautica.Network.Models.Metadata;
    using ServerModel      = Subnautica.Network.Models.Server;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class HoverpadProcessor : MetadataProcessor
    {
        /**
         *
         * Showroom değerlerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<string, byte> HoverpadShowrooms { get; set; } = new Dictionary<string, byte>();

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(string uniqueId, TechType techType, MetadataComponentArgs packet, bool isSilence)
        {
            var component = packet.Component.GetComponent<Metadata.Hoverpad>();
            if (component == null)
            {
                return false;
            }

            var constructor = Network.Identifier.GetComponentByGameObject<global::HoverpadConstructor>(packet.UniqueId);
            if (constructor == null)
            {
                return false;
            }

            if (isSilence)
            {
                if (component.IsDocked)
                {
                    Vehicle.CraftHoverbike(constructor, component.ItemId, component.FinishedTime, false, (GameObject gameObject) => this.WorldLoadHoverbikeSpawning(gameObject, component.Hoverbike));
                }

                if (component.ShowroomPlayerCount > 0)
                {
                    this.HoverpadShowrooms[packet.UniqueId] = component.ShowroomPlayerCount;
                }

                return true;
            }

            var player = ZeroPlayer.GetPlayerById(packet.GetPacketOwnerId());

            if (component.IsSpawning)
            {
                Vehicle.CraftHoverbike(constructor, component.ItemId, component.FinishedTime, ZeroPlayer.IsPlayerMine(packet.GetPacketOwnerId()));
            }
            else if (component.ColorCustomizer != null)
            {
                var customizeable = Network.Identifier.GetComponentByGameObject<global::ICustomizeable>(component.ItemId);
                if (customizeable != null)
                {
                    customizeable.CopyFrom(component.ColorCustomizer);

                    constructor.hoverpad.terminalGUI.SetScreen();
                    constructor.hoverpad.terminalGUI.SetCustomizeable(constructor.hoverpad.dockedBike.colorNameControl);
                }
            }
            else if (component.ShowroomTriggerType > 0)
            {
                this.HoverpadShowrooms[packet.UniqueId] = component.ShowroomPlayerCount;
            }
            else if (component.IsDocking)
            {
                var vehicleId = Network.DynamicEntity.GetEntity(component.ItemId).Id;

                Network.DynamicEntity.RemoveEntity(component.ItemId);

                var hoverbike = Network.Identifier.GetComponentByGameObject<global::Hoverbike>(component.ItemId);
                if (hoverbike)
                {
                    Vehicle.DockHoverbike(constructor.hoverpad, hoverbike, player != null && player.IsMine, () =>
                    {
                        if (player != null && player.VehicleId == vehicleId)
                        {
                            player.DockStartCinematicHoverpad(packet.UniqueId);
                            return true;
                        }

                        return false;
                    });
                }
            }
            else if (component.IsUnDocking)
            {
                Network.DynamicEntity.SetEntity(component.Entity);
               
                if (ZeroPlayer.IsPlayerMine(packet.GetPacketOwnerId()))
                {
                    player.OnHandClicHoverpadUndock(packet.UniqueId);
                }
                else
                {
                    player?.UndockStartCinematicHoverpad(packet.UniqueId);
                }
            }

            return true;
        }

        /**
         *
         * Harita yüklenmesinde hoverbike doğduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void WorldLoadHoverbikeSpawning(GameObject gameObject, WorldEntityModel.Hoverbike component)
        {
            WorldDynamicEntityProcessor.ExecuteItemSpawnProcessor(TechType.Hoverbike, component, true, null, gameObject);

            var hoverpad = gameObject.GetComponentInParent<global::Hoverpad>();
            if (hoverpad)
            {
                hoverpad.terminalGUI.SetScreen();
                hoverpad.terminalGUI.SetCustomizeable(hoverpad.dockedBike.colorNameControl);
            }
        }
        
        /**
         *
         * Hoverbike inşaa edilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnHoverpadHoverbikeSpawning(HoverpadHoverbikeSpawningEventArgs ev)
        {
            ev.IsAllowed = false;

            if (!Interact.IsBlocked(ev.UniqueId))
            {
                HoverpadProcessor.SendPacketToServer(ev.UniqueId, isSpawning: true);
            }
        }

        /**
         *
         * Renk değiştirme paleti seçildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSubNameInputSelecting(SubNameInputSelectingEventArgs ev)
        {
            if (ev.TechType == TechType.Hoverpad)
            {
                if (!Interact.IsBlocked(ev.UniqueId))
                {
                    HoverpadProcessor.SendPacketToServer(ev.UniqueId, isCustomizerOpening: true);
                }
                else if (Interact.IsBlocked(ev.UniqueId, true))
                {
                    ev.IsAllowed = false;
                }
            }
        }

        /**
         *
         * Renk değiştirme paleti seçimden çıktığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSubNameInputDeselected(SubNameInputDeselectedEventArgs ev)
        {
            if (ev.TechType == TechType.Hoverpad)
            {
                HoverpadProcessor.SendPacketToServer(ev.UniqueId, colorCustomizer: new ZeroColorCustomizer(ev.Name, ev.BaseColor.ToZeroColor(), ev.StripeColor1.ToZeroColor(), ev.StripeColor2.ToZeroColor(), ev.NameColor.ToZeroColor()));
            }
        }

        /**
         *
         * Hoverbike, pad üzerine takılınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnHoverpadDocking(HoverpadDockingEventArgs ev)
        {
            ev.IsAllowed = false;

            if (!Interact.IsBlocked(ev.UniqueId))
            {
                var entity = Network.DynamicEntity.GetEntity(ev.ItemId);
                if (entity != null && entity.IsMine(ZeroPlayer.CurrentPlayer.UniqueId))
                {
                    HoverpadProcessor.SendPacketToServer(ev.UniqueId, itemId: ev.ItemId, isDocking: true);
                }
            }
        }

        /**
         *
         * Hoverbike, pad üzerinden ayrılınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnHoverpadUnDocking(HoverpadUnDockingEventArgs ev)
        {
            ev.IsAllowed = false;

            if (!Interact.IsBlocked(ev.UniqueId) && !Interact.IsBlocked(ev.ItemId))
            {
                HoverpadProcessor.SendPacketToServer(ev.UniqueId, itemId: ev.ItemId, hoverbikePosition: ev.Position.ToZeroVector3(), hoverbikeRotation: ev.Rotation.ToZeroQuaternion(), isUnDocking: true);
            }
        }

        /**
         *
         * Hoverbike yakınına gelince veya ayrılınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnHoverpadShowroomTriggering(HoverpadShowroomTriggeringEventArgs ev)
        {
            ev.IsAllowed = false;

            HoverpadProcessor.SendPacketToServer(ev.UniqueId, showroomTriggerType: ev.IsEnter ? (byte)1 : (byte)2);
        }

        /**
         *
         * Her Sabit karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnFixedUpdate()
        {
            if (this.HoverpadShowrooms.Count > 0)
            {
                foreach (var showroom in this.HoverpadShowrooms.ToArray())
                {
                    var hoverpad = Network.Identifier.GetComponentByGameObject<global::Hoverpad>(showroom.Key, true);
                    if (hoverpad)
                    {
                        if (showroom.Value > 0)
                        {
                            if (hoverpad.animator.GetBool("display_bike") == false)
                            {
                                hoverpad.TryActivateShowroom(true);
                            }
                        }
                        else
                        {
                            this.HoverpadShowrooms.Remove(showroom.Key);
                            hoverpad.TryActivateShowroom(false);
                        }
                    }
                    else
                    {
                        this.HoverpadShowrooms.Remove(showroom.Key);
                    }
                }
            }
        }

        /**
         *
         * Verileri temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnDispose()
        {
            this.HoverpadShowrooms.Clear();
        }

        /**
         *
         * Sunucuya Veri Gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SendPacketToServer(string uniqueId, string itemId = null, bool isCustomizerOpening = false, byte showroomTriggerType = 0, ZeroColorCustomizer colorCustomizer = null, ZeroVector3 hoverbikePosition = null, ZeroQuaternion hoverbikeRotation = null,  bool isSpawning = false, bool isDocking = false, bool isUnDocking = false)
        {
            ServerModel.MetadataComponentArgs result = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = uniqueId,
                Component = new Metadata.Hoverpad()
                {
                    ItemId              = itemId,
                    IsSpawning          = isSpawning,
                    IsDocking           = isDocking,
                    IsUnDocking         = isUnDocking,
                    IsCustomizerOpening = isCustomizerOpening,
                    ShowroomTriggerType = showroomTriggerType,
                    ColorCustomizer     = colorCustomizer,
                    HoverbikePosition   = hoverbikePosition,
                    HoverbikeRotation   = hoverbikeRotation,
                },
            };

            NetworkClient.SendPacket(result);
        }
    }
}