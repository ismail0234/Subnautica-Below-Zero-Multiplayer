namespace Subnautica.Server.Processors.Metadata
{
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;
    using Subnautica.Server.Logic;

    using Metadata         = Subnautica.Network.Models.Metadata;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class HoverpadProcessor : MetadataProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(AuthorizationProfile profile, MetadataComponentArgs packet, ConstructionItem construction)
        {
            var constructionComponent = construction.EnsureComponent<Metadata.Hoverpad>();
            var hoverpadComponent     = packet.Component.GetComponent<Metadata.Hoverpad>();

            if (Server.Instance.Logices.Interact.IsBlocked(construction.UniqueId, profile.UniqueId))
            {
                if (hoverpadComponent.ShowroomTriggerType <= 0)
                {
                    return false;
                }
            }

            if (hoverpadComponent.IsSpawning)
            {
                if (!constructionComponent.IsDocked)
                {
                    constructionComponent.FinishedTime = Server.Instance.Logices.World.GetServerTime() + 15f;
                    constructionComponent.IsDocked     = true;
                    constructionComponent.ItemId       = API.Features.Network.Identifier.GenerateUniqueId();
                    constructionComponent.Hoverbike    = new WorldEntityModel.Hoverbike();

                    if (Server.Instance.Storages.Construction.UpdateMetadata(packet.UniqueId, constructionComponent))
                    {
                        hoverpadComponent.FinishedTime = constructionComponent.FinishedTime;
                        hoverpadComponent.IsDocked     = constructionComponent.IsDocked;
                        hoverpadComponent.ItemId       = constructionComponent.ItemId;

                        profile.SendPacketToAllClient(packet);
                    }
                }
            }
            else if (hoverpadComponent.IsCustomizerOpening)
            {
                if (!Server.Instance.Logices.Interact.IsBlocked(packet.UniqueId))
                {
                    Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, packet.UniqueId, true);
                }
            }
            else if (hoverpadComponent.ColorCustomizer != null)
            {
                if (Server.Instance.Logices.Interact.IsBlockedByPlayer(profile.UniqueId, packet.UniqueId))
                {
                    Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId);

                    constructionComponent.Hoverbike.ColorCustomizer.CopyFrom(hoverpadComponent.ColorCustomizer);

                    if (Server.Instance.Storages.Construction.UpdateMetadata(packet.UniqueId, constructionComponent))
                    {
                        hoverpadComponent.ItemId = constructionComponent.ItemId;

                        profile.SendPacketToOtherClients(packet);
                    }
                }
            }
            else if (hoverpadComponent.ShowroomTriggerType > 0)
            {
                if (hoverpadComponent.ShowroomTriggerType == 1)
                {
                    Server.Instance.Logices.Hoverpad.AddPlayerToPlatform(construction.UniqueId, profile.UniqueId);
                }
                else
                {
                    Server.Instance.Logices.Hoverpad.RemovePlayerFromPlatform(construction.UniqueId, profile.UniqueId);
                }

                constructionComponent.ShowroomPlayerCount = Server.Instance.Logices.Hoverpad.GetPlayerCountFromPlatform(construction.UniqueId);

                if (Server.Instance.Storages.Construction.UpdateMetadata(packet.UniqueId, constructionComponent))
                {
                    hoverpadComponent.ShowroomPlayerCount = constructionComponent.ShowroomPlayerCount;

                    profile.SendPacketToAllClient(packet);
                }
            }
            else if (hoverpadComponent.IsUnDocking)
            {
                if (constructionComponent.IsDocked && constructionComponent.ItemId == hoverpadComponent.ItemId)
                {
                    if (!Server.Instance.Logices.Interact.IsBlocked(packet.UniqueId) && !Server.Instance.Logices.Interact.IsBlocked(constructionComponent.ItemId))
                    {
                        Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, constructionComponent.ItemId, true);

                        WorldDynamicEntity dynamicEntity = new WorldDynamicEntity()
                        {
                            Id               = Server.Instance.Logices.World.GetNextItemId(),
                            UniqueId         = constructionComponent.ItemId,
                            Item             = null,
                            TechType         = TechType.Hoverbike,
                            Position         = hoverpadComponent.HoverbikePosition,
                            Rotation         = hoverpadComponent.HoverbikeRotation,
                            OwnershipId      = profile.UniqueId,
                            IsDeployed       = true,
                            IsGlobalEntity   = API.Features.TechGroup.IsGlobalEntity(TechType.Hoverbike),
                            Component        = constructionComponent.Hoverbike,
                            IsUsingByPlayer  = true,
                        };

                        constructionComponent.FinishedTime = 0f;
                        constructionComponent.IsDocked     = false;
                        constructionComponent.ItemId       = null;

                        hoverpadComponent.Entity = dynamicEntity;

                        if (Server.Instance.Storages.World.AddWorldDynamicEntity(dynamicEntity) && Server.Instance.Storages.Construction.UpdateMetadata(packet.UniqueId, constructionComponent))
                        {
                            profile.SendPacketToAllClient(packet);
                        }
                    }
                }
            }
            else if (hoverpadComponent.IsDocking)
            {
                var entity = Server.Instance.Storages.World.GetDynamicEntity(hoverpadComponent.ItemId);
                if (!constructionComponent.IsDocked && entity != null)
                {
                    if (Server.Instance.Logices.Interact.IsBlockedByConstruction(hoverpadComponent.ItemId))
                    {
                        Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId, Interact.HoverpadDocking);
                    }

                    constructionComponent.IsDocked  = true;
                    constructionComponent.ItemId    = hoverpadComponent.ItemId;
                    constructionComponent.Hoverbike = entity.Component.GetComponent<WorldEntityModel.Hoverbike>();

                    Server.Instance.Storages.World.RemoveDynamicEntity(hoverpadComponent.ItemId);

                    if (Server.Instance.Storages.Construction.UpdateMetadata(packet.UniqueId, constructionComponent))
                    {
                        profile.SetInterior(null);
                        profile.SetVehicle(null);
                        profile.SendPacketToAllClient(packet);
                    }
                }
            }

            return true;
        }
    }
}