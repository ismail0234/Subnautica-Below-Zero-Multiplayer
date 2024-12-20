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

    public class FlareProcessor : PlayerItemProcessor
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
            var component = packet.GetComponent<ItemModel.Flare>();
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
         * Nesne doğduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEntitySpawned(ItemQueueProcess item, global::Pickupable pickupable, GameObject gameObject)
        {
            var entity = item.Action.GetProperty<WorldDynamicEntity>("Entity");
            if (entity != null)
            {
                WorldDynamicEntityProcessor.ExecuteItemSpawnProcessor(entity.TechType, entity.Component, true, pickupable, gameObject);

                if (pickupable.TryGetComponent<global::Flare>(out var flare) && !flare.useRigidbody.isKinematic)
                {
                    var forward = item.Action.GetProperty<ZeroVector3>("CustomProperty");
                    if (forward != null)
                    {
                        flare.transform.GetComponent<WorldForces>().enabled = true;
                        flare.throwSound.StartEvent();

                        flare.useRigidbody.ResetForce();
                        flare.useRigidbody.AddForce(forward.ToVector3() * 60f);
                        flare.useRigidbody.AddTorque(flare.transform.right * 1f);
                        flare.useRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        flare.gameObject.EnsureComponent<SetRigidBodyModeOnSlowdown>().TriggerStart(flare.useRigidbody, CollisionDetectionMode.ContinuousSpeculative, 25f);
                    }
                }
            }
        }

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnFixedUpdate()
        {
            foreach (var player in ZeroPlayer.GetPlayers())
            {
                if (player.TechTypeInHand == TechType.Flare)
                {
                    this.ProcessFlare(player);
                }
            }
        }

        /**
         *
         * Fener ışığını açar/kapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool ProcessFlare(ZeroPlayer player)
        {
            if (player.HandItemComponent == null)
            {
                return false;
            }

            var tool = player.GetHandTool<global::Flare>(TechType.Flare);
            if (tool == null)
            {
                return false;
            }

            var item = player.HandItemComponent.GetComponent<ItemModel.Flare>();
            if (item != null)
            {
                tool.light.intensity   = item.Intensity;
                tool.light.range       = item.Range;
                tool.energyLeft        = item.Energy;
                tool.flareActivateTime = -99f;

                if (tool.energyLeft < 3.0f)
                {
                    if (tool.fxIsPlaying || tool.fxControl.emitters[2].fxPS == null)
                    {
                        tool.fxControl.StopAndDestroy(1, 2f);
                        tool.fxControl.Play(2);
                        tool.fxIsPlaying = false;
                    }
                }
                else
                {
                    tool.SetFlareActiveState(true);

                    if (tool.fxControl && !tool.fxIsPlaying)
                    {
                        tool.fxControl.Play(1);
                        tool.fxIsPlaying = true;
                    }
                }
            }

            return true;
        }

        /**
         *
         * İşaret fişeti yere konulduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnFlareDeploying(FlareDeployingEventArgs ev)
        {
            ev.IsAllowed = false;

            FlareProcessor.SendPacketToServer(ev.UniqueId, ev.DeployPosition.ToZeroVector3(), ev.Forward.ToZeroVector3(), Quaternion.identity.ToZeroQuaternion(), ev.Energy);
        }

        /**
         *
         * Sunucuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, ZeroVector3 position, ZeroVector3 forward, ZeroQuaternion rotation, float energy)
        {
            ServerModel.PlayerItemActionArgs result = new ServerModel.PlayerItemActionArgs()
            {
                Item = new ItemModel.Flare()
                {
                    UniqueId = uniqueId,
                    Position = position,
                    Forward  = forward,
                    Rotation = rotation,
                    Energy   = energy,
                }
            };

            NetworkClient.SendPacket(result);
        }
    }
}