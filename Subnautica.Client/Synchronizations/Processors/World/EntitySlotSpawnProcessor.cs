namespace Subnautica.Client.Synchronizations.Processors.World
{
    using System;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.API.MonoBehaviours;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Metadata;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using ServerModel = Subnautica.Network.Models.Server;

    public class EntitySlotSpawnProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.EntitySlotProcessArgs>();
            if (packet == null)
            {
                return false;
            }

            World.DestroyPickupItem(packet.WorldPickupItem);

            if (packet.IsBreakable)
            {
                Network.DynamicEntity.Spawn(packet.Entity, this.OnBreakableEntitySpawned, Convert.ToInt32(packet.WorldPickupItem.Item.ItemId), ZeroPlayer.IsPlayerMine(packet.GetPacketOwnerId()));
            }
            else 
            {
                if (ZeroPlayer.IsPlayerMine(packet.GetPacketOwnerId()))
                {
                    Entity.SpawnToQueue(packet.WorldPickupItem.Item.TechType, packet.WorldPickupItem.GetItemId(), new ZeroTransform(new ZeroVector3(), new ZeroQuaternion()), new ItemQueueAction(null, this.OnPickupEntitySpawned));
                }
            }

            return true;
        }

        /**
         *
         * Nesne doğduğında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void OnPickupEntitySpawned(ItemQueueProcess item, Pickupable pickupable, GameObject gameObject)
        {
            if (!pickupable.LocalPickup())
            {
                World.DestroyGameObject(pickupable.gameObject);
            }
        }

        /**
         *
         * Nesne doğduğında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void OnBreakableEntitySpawned(ItemQueueProcess item, Pickupable pickupable, GameObject gameObject)
        {
            pickupable.MultiplayerDrop();

            var resource = Network.Identifier.GetComponentByGameObject<BreakableResource>(item.Action.GetProperty<int>("CustomProperty").ToWorldStreamerId(), true);
            if (resource)
            {
                if (item.Action.GetProperty<bool>("CustomProperty2") && !string.IsNullOrEmpty(resource.customGoalText))
                {
                    GoalManager.main.OnCustomGoalEvent(resource.customGoalText);
                }

                FMODUWE.PlayOneShot(resource.breakSound, resource.transform.position);

                if (resource.hitFX)
                {
                    Utils.PlayOneShotPS(resource.breakFX, resource.transform.position, Quaternion.Euler(new Vector3(270f, 0.0f, 0.0f)));
                }
            }

            if (item.Action.GetProperty<bool>("CustomProperty2"))
            {
                Rigidbody rigidbody = pickupable.gameObject.EnsureComponent<Rigidbody>();
                UWE.Utils.SetIsKinematicAndUpdateInterpolation(rigidbody, false);
                rigidbody.AddTorque(Vector3.right * UnityEngine.Random.Range(3, 6));

                if (resource)
                {
                    rigidbody.AddForce(resource.transform.up * 0.1f);
                }
            }
        }

        /**
         *
         * Nesne Slotu doğarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEntitySlotSpawning(EntitySlotSpawningEventArgs ev)
        {
            ev.IsAllowed = false;

            var slot = Network.WorldStreamer.GetSlotById(ev.SlotId);
            if (slot != null)
            {
                ev.IsAllowed = true;
                ev.ClassId   = slot.ClassId;

                if (slot.TechType.IsCreature() && slot.TechType.IsSynchronizedCreature())
                {
                    ev.IsAllowed = false;
                }
            }
        }

        /**
         *
         * Nesne spawn olurken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEntitySpawning(EntitySpawningEventArgs ev)
        {
            if (ev.TechType.IsCreature() &&  ev.TechType.IsSynchronizedCreature())
            {
                ev.IsAllowed = false;
            }
        }

        /**
         *
         * Nesne spawn olduktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEntitySpawned(EntitySpawnedEventArgs ev)
        {
            if (ev.SlotType == SlotType.None && ev.TechType.IsCreature() && !ev.UniqueId.IsWorldStreamer())
            {
                ev.SlotType = SlotType.WorldStreamer;

                var slot = Network.WorldStreamer.GetSlotById(GetCreatureSlotId(ev.GameObject));
                if (slot != null)
                {
                    ev.UniqueId = GetCreatureSlotId(ev.GameObject).ToWorldStreamerId();

                    Network.Identifier.SetIdentityId(ev.GameObject, ev.UniqueId);
                }
            }
            
            if (ev.SlotType == SlotType.WorldStreamer)
            {
                if (ev.UniqueId.IsWorldStreamer())
                {
                    var slotId = ev.UniqueId.WorldStreamerToSlotId();
                    var slot   = Network.WorldStreamer.GetSlotById(slotId);
                    if (slot != null)
                    {
                        if (!ev.TechType.IsCreature())
                        {
                            ev.GameObject.EnsureComponent<SpawnPointComponent>().SetSpawnPoint(slot);
                        }
                    }
                    else
                    {
                        Log.Info("NULL SLOT ID 111: " + ev.UniqueId + ", TechType: " + ev.TechType + ", ClassId: " + ev.ClassId);
                    }
                }
                else
                {
                    Log.Info("NULL SLOT ID 222: " + ev.UniqueId + ", TechType: " + ev.TechType + ", ClassId: " + ev.ClassId);
                }
            }
        }

        /**
         *
         * Kaynak kırıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBreakableResourceBreaking(BreakableResourceBreakingEventArgs ev)
        {
            ev.IsAllowed = false;

            if (ev.UniqueId.IsWorldStreamer())
            {
                EntitySlotSpawnProcessor.SendPacketToServer(ev.UniqueId.WorldStreamerToSlotId(), isBreakable: true, position: ev.Position.ToZeroVector3(), worldPickupItem: WorldPickupItem.Create(StorageItem.Create(ev.UniqueId.WorldStreamerToSlotId().ToString(), ev.TechType), PickupSourceType.EntitySlot));
            }
        }

        /**
         *
         * Oyuncu yerden eşya aldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerItemPickedUp(PlayerItemPickedUpEventArgs ev)
        {
            if (!ev.IsStaticWorldEntity && ev.UniqueId.IsWorldStreamer())
            {
                ev.IsAllowed = false;

                EntitySlotSpawnProcessor.SendPacketToServer(ev.UniqueId.WorldStreamerToSlotId(), isBreakable: false, worldPickupItem: WorldPickupItem.Create(StorageItem.Create(ev.UniqueId.WorldStreamerToSlotId().ToString(), ev.Pickupable.GetTechType()), PickupSourceType.EntitySlot));
            }
        }

        /**
         *
         * Sunucuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SendPacketToServer(int slotId, bool isBreakable = false, WorldPickupItem worldPickupItem = null, ZeroVector3 position = null)
        {
            var slot = Network.WorldStreamer.GetSlotById(slotId);
            if (slot != null)
            {
                ServerModel.EntitySlotProcessArgs request = new ServerModel.EntitySlotProcessArgs()
                {
                    WorldPickupItem = worldPickupItem,
                    IsBreakable     = isBreakable,
                    Position        = position,
                };

                NetworkClient.SendPacket(request);
            }
        }

        /**
         *
         * Yaratık slot id değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static int GetCreatureSlotId(GameObject gameObject)
        {
            if (gameObject.TryGetComponent<global::Creature>(out var creature))
            {
                if (creature.leashPosition == Vector3.zero)
                {
                    return gameObject.transform.position.GetHashCode();
                }

                return creature.leashPosition.GetHashCode();
            }

            return gameObject.transform.position.GetHashCode();
        }
    }
}