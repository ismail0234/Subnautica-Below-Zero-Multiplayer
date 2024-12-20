namespace Subnautica.Client.Synchronizations.Processors.Items
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using ItemModel        = Subnautica.Network.Models.Items;
    using ServerModel      = Subnautica.Network.Models.Server;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class HoverbikeProcessor : PlayerItemProcessor
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
            var component = packet.GetComponent<ItemModel.Hoverbike>();
            if (component == null)
            {
                return false;
            }

            if (ZeroPlayer.IsPlayerMine(playerId))
            {
                World.DestroyItemFromPlayer(component.Entity.UniqueId);
            }

            Network.DynamicEntity.Spawn(component.Entity, this.OnEntitySpawned, component.Forward);
            return true;
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
            var entity  = item.Action.GetProperty<WorldDynamicEntity>("Entity");
            if (entity != null)
            {
                WorldDynamicEntityProcessor.ExecuteItemSpawnProcessor(entity.TechType, entity.Component, true, pickupable, gameObject);
               
                if (gameObject.TryGetComponent<global::HoverbikePlayerTool>(out var tool))
                {
                    var forward = item.Action.GetProperty<ZeroVector3>("CustomProperty");
                    if (forward != null && tool.TryGetComponent<Rigidbody>(out var rigidBody) && !rigidBody.isKinematic)
                    {
                        rigidBody.AddForce(forward.ToVector3() * 5.0f, ForceMode.VelocityChange);

                        tool.hoverbike.OnBikeDeploy(forward.ToVector3());
                    }

                    tool.equippedBikeAnimator.SetBool("using_tool", true);
                    tool.equippedBikeMesh.SetActive(true);
                    tool.hoverbike.MatchMotorModeToPlayer();

                    tool.sfx_activate.Play();

                    tool.deployed.SetActive(true);
                    tool.equipped.SetActive(false);
                    tool.isEquipped = false;
                }
            }
        }

        /**
         *
         * Hoverbike bırakıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnHoverbikeDeploying(HoverbikeDeployingEventArgs ev)
        {
            ev.IsAllowed = false;

            HoverbikeProcessor.SendPacketToServer(ev.UniqueId, ev.DeployPosition.ToZeroVector3(), ev.Forward.ToZeroVector3(), ev.Hoverbike.ToHoverbikeComponent());
        }

        /**
         *
         * Sunucuya paketi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, ZeroVector3 deployPosition, ZeroVector3 forward, WorldEntityModel.Hoverbike component)
        {
            ServerModel.PlayerItemActionArgs request = new ServerModel.PlayerItemActionArgs()
            {
                Item = new ItemModel.Hoverbike()
                {
                    UniqueId  = uniqueId,
                    Position  = deployPosition,
                    Forward   = forward,
                    Component = component,
                }
            };

            NetworkClient.SendPacket(request);
        }
    }
}
