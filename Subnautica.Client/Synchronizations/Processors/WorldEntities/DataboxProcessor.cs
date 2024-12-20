namespace Subnautica.Client.Synchronizations.Processors.WorldEntities
{
    using System.Collections;
    
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using Subnautica.Client.Core;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Network.Core.Components;

    using ServerModel = Subnautica.Network.Models.Server;
    using EntityModel = Subnautica.Network.Models.WorldEntity;

    using UnityEngine;
    using Subnautica.API.Extensions;

    public class DataboxProcessor : WorldEntityProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkWorldEntityComponent packet, byte requesterId, bool isSpawning)
        {
            var entity = packet.GetComponent<EntityModel.Databox>();
            if (string.IsNullOrEmpty(entity.UniqueId))
            {
                return false;
            }

            Network.StaticEntity.AddStaticEntity(entity);

            var databox = Network.Identifier.GetComponentByGameObject<global::BlueprintHandTarget>(entity.UniqueId, true);
            if (databox != null)
            {
                databox.used = true;

                if (isSpawning)
                {
                    UWE.CoroutineHost.StartCoroutine(this.PickupBlueprintAsync(databox));
                }
                else
                {
                    this.PickupBlueprint(databox);
                }
            }

            return true;
        }

        /**
         *
         * Asenkron olarak veri nesnesini işler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private IEnumerator PickupBlueprintAsync(global::BlueprintHandTarget databox)
        {
            yield return new WaitForSecondsRealtime(0.25f);

            if (databox != null) 
            {
                this.PickupBlueprint(databox);
            }
        }

        /**
         *
         * Normal olarak veri nesnesini işler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void PickupBlueprint(global::BlueprintHandTarget databox)
        {
            if (databox.animParam.IsNotNull())
            {
                databox.animator.SetBool(databox.animParam, true);
            }

            databox.used = true;
            databox.OnTargetUsed();
        }

        /**
         *
         * Veri kutusundan tasarım alındığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnDataboxItemPickedUp(DataboxItemPickedUpEventArgs ev)
        {
            ServerModel.WorldEntityActionArgs result = new ServerModel.WorldEntityActionArgs()
            {
                Entity = new EntityModel.Databox()
                {
                    UniqueId = ev.UniqueId,
                    IsUsed   = true,
                }
            };

            NetworkClient.SendPacket(result);
        }
    }
}