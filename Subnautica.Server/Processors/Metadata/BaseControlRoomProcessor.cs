namespace Subnautica.Server.Processors.Metadata
{
    using System.Linq;

    using Subnautica.API.Features;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using Metadata = Subnautica.Network.Models.Metadata;

    public class BaseControlRoomProcessor : MetadataProcessor
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
            var component = packet.Component.GetComponent<Metadata.BaseControlRoom>();
            if (component == null)
            {
                return false;
            }

            if (component.IsColorCustomizerOpening)
            {
                var uniqueId = TechGroup.GetBaseControlRoomCustomizerId(construction.UniqueId);

                if (!Server.Instance.Logices.Interact.IsBlocked(uniqueId))
                {
                    Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, uniqueId, true);
                }
            }
            else if (component.IsNavigateOpening)
            {
                var uniqueId = TechGroup.GetBaseControlRoomNavigateId(construction.UniqueId);

                if (!Server.Instance.Logices.Interact.IsBlocked(uniqueId))
                {
                    Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, uniqueId, true);

                    profile.SendPacket(packet);
                }
            }
            else if (component.IsNavigationExiting)
            {
                if (Server.Instance.Logices.Interact.IsBlockedByPlayer(profile.UniqueId))
                {
                    Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId);

                    if (component.Minimap?.Position != null)
                    {
                        component.IsNavigationExiting = false;

                        if (Server.Instance.Storages.World.TryGetBase(construction.BaseId, out var baseComponent))
                        {
                            baseComponent.MinimapPositions[construction.UniqueId] = component.Minimap.Position;
                        }

                        profile.SendPacketToOtherClients(packet);
                    }
                }
            }
            else if (component.IsColorCustomizerSave)
            {
                if (Server.Instance.Logices.Interact.IsBlockedByPlayer(profile.UniqueId))
                {
                    Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId);

                    if (Server.Instance.Storages.World.TryGetBase(construction.BaseId, out var baseComponent))
                    {
                        baseComponent.SetColorCustomizer(component.Name, component.BaseColor, component.StripeColor1, component.StripeColor2, component.NameColor);
                    }

                    profile.SendPacketToOtherClients(packet);
                }
            }
            else if (component.Minimap != null)
            {
                if (Server.Instance.Storages.World.TryGetBase(construction.BaseId, out var baseComponent))
                {
                    if (component.Minimap.Position != null)
                    {
                        baseComponent.MinimapPositions[construction.UniqueId] = component.Minimap.Position;

                        profile.SendPacketToOtherClients(packet);
                    }
                    else if (component.Minimap.Cell != null)
                    {
                        var isPowered = false;
                        if (baseComponent.DisablePowers.Where(q => q == component.Minimap.Cell).Any())
                        {
                            baseComponent.DisablePowers.RemoveWhere(q => q == component.Minimap.Cell);

                            isPowered = true;
                        }
                        else
                        {
                            baseComponent.DisablePowers.Add(component.Minimap.Cell);

                            isPowered = false;
                        }

                        component.Minimap.IsPowered = isPowered;

                        profile.SendPacketToAllClient(packet);
                    }
                }
            }

            return true;
        }
    }
}
