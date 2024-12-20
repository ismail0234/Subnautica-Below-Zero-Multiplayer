namespace Subnautica.Client.Synchronizations.Processors.Items
{
    using Subnautica.Events.EventArgs;
    using Subnautica.Client.Core;
    using Subnautica.API.Extensions;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Network.Core.Components;
    using Subnautica.API.Features;

    using ServerModel = Subnautica.Network.Models.Server;
    using ItemModel   = Subnautica.Network.Models.Items;

    using UnityEngine;

    using FMOD.Studio;

    public class KnifeProcessor : PlayerItemProcessor
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
            var entity = packet.GetComponent<ItemModel.Knife>();
            if (entity == null)
            {
                return false;
            }

            var player = ZeroPlayer.GetPlayerById(playerId);
            if (player == null)
            {
                return false;
            }

            this.PlayAttackAnimation(player, entity);
            return true;
        }

        /**
         *
         * Bıçak saldırı animasyonunu oynatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool PlayAttackAnimation(ZeroPlayer player, ItemModel.Knife entity)
        {
            global::Knife tool = null;

            if (player.TechTypeInHand == TechType.Knife)
            {
                tool = this.GetPlayerTool<global::Knife>(player, TechType.Knife);
            }
            else if (player.TechTypeInHand == TechType.HeatBlade)
            {
                tool = this.GetPlayerTool<global::HeatBlade>(player, TechType.HeatBlade);
            }

            if (tool == null)
            {
                return false;
            }

            float distance = global::Player.main.transform.position.ToZeroVector3().Distance(entity.TargetPosition);
            if (distance > 300f)
            {
                return false;
            }

            VFXSurfaceTypeManager.main.Play(entity.SurfaceType, tool.vfxEventType, entity.TargetPosition.ToVector3(), Quaternion.Euler(entity.Orientation.ToVector3()), null);

            if (distance <= 30f)
            {
                Utils.PlayFMODAsset(entity.IsUnderwater ? tool.swingWaterSound : tool.swingSound, entity.TargetPosition.ToVector3());
            }

            EventInstance fmodEvent = Utils.GetFMODEvent(tool.hitSound, entity.TargetPosition.ToVector3());
            fmodEvent.setParameterValueByIndex(tool.surfaceParamIndex, (float) entity.SoundSurfaceType);
            fmodEvent.start();
            fmodEvent.release();
            return true;    
        }

        /**
         *
         * Bıçak kullanıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnKnifeUsing(KnifeUsingEventArgs ev)
        {
            ServerModel.PlayerItemActionArgs result = new ServerModel.PlayerItemActionArgs()
            {
                Item = new ItemModel.Knife()
                {
                    VFXEventType     = ev.VFXEventType,
                    TargetPosition   = ev.TargetPosition.ToZeroVector3(),
                    Orientation      = ev.Orientation.ToZeroVector3(),
                    SurfaceType      = ev.SurfaceType,
                    SoundSurfaceType = ev.SoundSurfaceType,
                    IsUnderwater     = ev.IsUnderwater,
                }
            };

            NetworkClient.SendPacket(result);
        }
    }
}
