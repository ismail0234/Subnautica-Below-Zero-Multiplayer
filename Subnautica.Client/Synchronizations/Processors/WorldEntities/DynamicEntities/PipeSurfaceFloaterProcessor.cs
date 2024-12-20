namespace Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Extensions;
    using Subnautica.Client.MonoBehaviours.World;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class PipeSurfaceFloaterProcessor : WorldDynamicEntityProcessor
    {
        /**
         *
         * Dünya yüklenip nesne doğduğunda çalışır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnWorldLoadItemSpawn(NetworkDynamicEntityComponent packet, bool isDeployed, Pickupable pickupable, GameObject gameObject)
        {
            if (!isDeployed)
            {
                return false;
            }

            var component = packet.GetComponent<WorldEntityModel.PipeSurfaceFloater>();
            if (component == null)
            {
                return false;
            }

            gameObject.EnsureComponent<MultiplayerPipeSurfaceFloater>();

            pickupable.MultiplayerDrop();

            if (pickupable.TryGetComponent<global::PipeSurfaceFloater>(out var floater))
            {
                floater.deployed = true;
            }

            if (component.Childrens.Count > 0)
            {
                this.SpawnChildrenOxygenPipes(component.Childrens, gameObject.GetIdentityId());
            }

            return true;
        }

        /**
         *
         * Çocuk oksijen borularını yumurtlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SpawnChildrenOxygenPipes(HashSet<OxygenPipeItem> childrens, string parentId)
        {
            foreach (var children in childrens.Where(q => q.ParentId == parentId))
            {
                SpawnOxygenPipe(children.ParentId, children.UniqueId, children.Position);

                this.SpawnChildrenOxygenPipes(childrens, children.UniqueId);
            }
        }

        /**
         *
         * Oksijen borusu üretir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SpawnOxygenPipe(string parentId, string pipeId, ZeroVector3 position)
        {
            var action = new ItemQueueAction();
            action.OnEntitySpawned = OnOxygenPipeSpawned;
            action.RegisterProperty("ParentId", parentId);

            Entity.SpawnToQueue(TechType.Pipe, pipeId, new ZeroTransform(position, Quaternion.identity.ToZeroQuaternion()), action);
        }

        /**
         *
         * Oksijen borusu spawn olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void OnOxygenPipeSpawned(ItemQueueProcess item, global::Pickupable pickupable, GameObject gameObject)
        {
            pickupable.MultiplayerDrop(ignoreTracker: true);

            if (gameObject.TryGetComponent<global::OxygenPipe>(out var oxygenPipe))
            {
                var parent = Network.Identifier.GetGameObject(item.Action.GetProperty<string>("ParentId"));
                if (parent && parent.TryGetComponent<IPipeConnection>(out var _parent))
                {
                    oxygenPipe.SetParent(_parent);
                }

                oxygenPipe.rigidBody.SetKinematic();
                oxygenPipe.UpdatePipe();
            }
        }
    }
}