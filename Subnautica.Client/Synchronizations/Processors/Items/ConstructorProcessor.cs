namespace Subnautica.Client.Synchronizations.Processors.Items
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using ItemModel   = Subnautica.Network.Models.Items;
    using ServerModel = Subnautica.Network.Models.Server;

    public class ConstructorProcessor : PlayerItemProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPlayerItemComponent packet, byte playerId)
        {
            var component = packet.GetComponent<ItemModel.Constructor>();
            if (component == null)
            {
                return false;
            }
            
            if (component.IsEngageActive())
            {
                var player = ZeroPlayer.GetPlayerById(playerId);

                if (component.IsEngage())
                {
                    if (player != null && player.IsMine)
                    {
                        player.OnHandClickConstructor(component.UniqueId);  
                    }
                    else
                    {
                        player?.EngageStartCinematicConstructor(component.UniqueId);
                    }
                }
                else
                {
                    player?.DisengageStartCinematicConstructor(component.UniqueId);
                }
            }
            else if (component.CraftingTechType != TechType.None)
            {
                Network.DynamicEntity.SetEntity(component.Entity);

                var constructorInput = Network.Identifier.GetComponentByGameObject<global::ConstructorInput>(component.UniqueId);

                Vehicle.CraftVehicle(component.Entity, constructorInput, this.OnCraftedVehicle, component.CraftingFinishTime, notify: true, isMine: ZeroPlayer.IsPlayerMine(playerId));
            }
            else
            {
                if (ZeroPlayer.IsPlayerMine(playerId))
                {
                    World.DestroyItemFromPlayer(component.Entity.UniqueId);
                }

                Network.DynamicEntity.Spawn(component.Entity, this.OnEntitySpawned, component.Forward);
            }

            return true;
        }

        /**
         *
         * Araç üretildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCraftedVehicle(WorldDynamicEntity entity, ItemQueueAction item, GameObject gameObject)
        {
            WorldDynamicEntityProcessor.ExecuteItemSpawnProcessor(entity.TechType, entity.Component, entity.IsDeployed, null, gameObject);
        }

        /**
         *
         * Nesne spawnlandıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEntitySpawned(ItemQueueProcess item, global::Pickupable pickupable, GameObject gameObject)
        {
            pickupable.MultiplayerDrop();

            if (pickupable.TryGetComponent<global::Constructor>(out var constructor))
            {
                if (constructor.TryGetComponent<Rigidbody>(out var rb) && !rb.isKinematic)
                {
                    rb.AddForce(item.Action.GetProperty<ZeroVector3>("CustomProperty").ToVector3() * 6.5f, ForceMode.VelocityChange);
                }

                constructor.Deploy(true);
                constructor.OnDeployAnimationStart();

                Utils.PlayEnvSound(constructor.releaseSound, pickupable.transform.position);

                GoalManager.main.OnCustomGoalEvent("Release_Constructor");
            }
        }

        /**
         *
         * Bir araç yapılmaya çalışıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnConstructorCrafting(ConstructorCraftingEventArgs ev)
        {
            ev.IsAllowed = false;

            ConstructorProcessor.SendPacketToServer(ev.UniqueId, craftingTechType: ev.TechType, craftingPosition: ev.Position.ToZeroVector3(), craftingRotation: ev.Rotation.ToZeroQuaternion());    
        }

        /**
         *
         * Constructor menüyü açtığında/kapattığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnConstructorEngageToggle(ConstructorEngageToggleEventArgs ev)
        {
            if (ev.IsEngage)
            {
                ev.IsAllowed = false;

                if (!Network.HandTarget.IsBlocked(ev.UniqueId))
                {
                    ConstructorProcessor.SendPacketToServer(ev.UniqueId, engageToggle: 1);
                }
            }
            else
            {
                ConstructorProcessor.SendPacketToServer(ev.UniqueId, engageToggle: 2);
            }
        }

        /**
         *
         * Constructor bırakılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnConstructorDeploying(ConstructorDeployingEventArgs ev)
        {
            ev.IsAllowed = false;

            ConstructorProcessor.SendPacketToServer(ev.UniqueId, forward: ev.Forward.ToZeroVector3(), position: ev.DeployPosition.ToZeroVector3());
        }

        /**
         *
         * Sunucuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SendPacketToServer(string uniqueId, TechType craftingTechType = TechType.None, ZeroVector3 craftingPosition = null, ZeroQuaternion craftingRotation = null, byte engageToggle = 0, ZeroVector3 forward = null, ZeroVector3 position = null)
        {
            ServerModel.PlayerItemActionArgs result = new ServerModel.PlayerItemActionArgs()
            {
                Item = new ItemModel.Constructor()
                {
                    UniqueId         = uniqueId,
                    EngageToggle     = engageToggle,
                    Forward          = forward,
                    Position         = position,
                    CraftingTechType = craftingTechType,
                    CraftingPosition = craftingPosition,
                    CraftingRotation = craftingRotation,
                }
            };

            NetworkClient.SendPacket(result);
        }
    }
}
