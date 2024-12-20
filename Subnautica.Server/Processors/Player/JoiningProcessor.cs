namespace Subnautica.Server.Processors.Player
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Server.Core;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts.Processors;

    using ClientModel = Subnautica.Network.Models.Client;
    using ServerModel = Subnautica.Network.Models.Server;

    public class JoiningProcessor : NormalProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnExecute(AuthorizationProfile profile, NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.JoiningServerArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (packet.UserName.IsNull())
            {
                Log.Info("EMPTY_NAME_ERROR");
                Server.DisconnectToClient(profile);
                return false;
            }

            if (!this.IsActive(packet.UserName))
            {
                Log.Info("NETWORK_IS_DOWN");
                Server.DisconnectToClient(profile);
                return false;
            }

            if (packet.UserId.IsNull())
            {
                Log.Info("EMPTY_USER_ERROR");
                Server.DisconnectToClient(profile);
                return false;
            }

            packet.UserName = packet.UserName.Trim();
            packet.UserId   = packet.UserId.Trim();

            if (Server.Instance.Players.Any(q => q.Value.PlayerName.Contains(packet.UserName)))
            {
                Log.Info("ALREADY_USERNAME_EXISTS " +  Tools.Base64Encode(Tools.Base64Encode(packet.UserName)));
                Server.DisconnectToClient(profile);
                return false;
            }

            if (Server.Instance.Players.ContainsKey(profile.IpPortAddress))
            {
                Log.Info("ALREADY_CONNECTED_ERROR");
                Server.DisconnectToClient(profile);
                return false;
            }

            var player = profile.Initialize(packet.UserName, packet.UserId);
            if (player == null)
            {
                Log.Info("PLAYER_INITIALIZE_ERROR");
                Server.DisconnectToClient(profile);
                return false;
            }

            if (Server.Instance.OwnerId == player.UniqueId)
            {
                player.IsHost = true;
            }

            Server.Instance.Players.Add(player.IpPortAddress, player);

            Log.Info(ZeroLanguage.Get("GAME_PLAYER_CONNECTED").Replace("{playername}", packet.UserName));

            if (packet.IsReconnect)
            {
                this.SendReconnectPacket(player);
            }
            else
            {
                this.SendFirstConnectionPacket(player);
            }

            return true;
        }

        /**
         *
         * İlk bağlantı paketini gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SendFirstConnectionPacket(AuthorizationProfile profile)
        {
            ClientModel.JoiningServerArgs request = new ClientModel.JoiningServerArgs()
            {
                // Oyuncu verileri
                PlayerId                = profile.PlayerId,
                PlayerUniqueId          = profile.UniqueId,
                PlayerSubRootId         = profile.SubrootId,
                PlayerInteriorId        = profile.InteriorId,
                PlayerRespawnPointId    = profile.RespawnPointId,
                PlayerPosition          = profile.Position,
                PlayerRotation          = profile.Rotation,
                PlayerHealth            = profile.Health,
                PlayerFood              = profile.Food,
                PlayerWater             = profile.Water,
                PlayerInventoryItems    = profile.InventoryItems,
                PlayerEquipments        = profile.Equipments,
                PlayerEquipmentSlots    = profile.EquipmentSlots,
                PlayerQuickSlots        = profile.QuickSlots,
                PlayerActiveSlot        = profile.ActiveSlot,
                PlayerItemPins          = profile.ItemPins,
                PlayerNotifications     = profile.PdaNotifications,
                PlayerUsedTools         = profile.UsedTools,
                PlayerSpecialGoals      = profile.SpecialGoals,
                PlayerTimeLastSleep     = Server.Instance.Storages.World.Storage.TimeLastSleep,
                IsInitialEquipmentAdded = profile.IsInitialEquipmentAdded,

                // Teknolojiler
                Technologies         = Server.Instance.Storages.Technology.Storage.Technologies,
                ScannedTechnologies  = Server.Instance.Storages.Scanner.Storage.Technologies,
                AnalizedTechnologies = Server.Instance.Storages.Technology.Storage.AnalizedTechnologies,
                Encyclopedias        = Server.Instance.Storages.Encyclopedia.Storage.Encyclopedias,

                // Dünya verileri
                ServerId             = Server.Instance.ServerId,
                ServerTime           = Server.Instance.Logices.World.GetServerTime(),
                Constructions        = new HashSet<ConstructionItem>(Server.Instance.Storages.Construction.Storage.Constructions.Values),
                ConstructionRoot     = Server.Instance.Storages.World.Storage.Constructions,
                JukeboxDisks         = Server.Instance.Storages.World.Storage.JukeboxDisks,
                PersistentEntities   = Server.Instance.Storages.World.Storage.PersistentEntities,
                DynamicEntities      = Server.Instance.Storages.World.Storage.DynamicEntities,
                IsFirstLogin         = Server.Instance.Storages.World.Storage.IsFirstLogin,
                SupplyDrops          = Server.Instance.Storages.World.Storage.SupplyDrops,
                InteractList         = Server.Instance.Logices.Interact.List,
                Bases                = Server.Instance.Storages.World.Storage.Bases,
                QuantumLocker        = Server.Instance.Storages.World.Storage.QuantumLocker,
                GameMode             = Server.Instance.GameMode,
                MaxPlayerCount       = Server.Instance.MaxPlayer,
                SeaTruckConnections  = Server.Instance.Storages.World.Storage.SeaTruckConnections,
                ActivatedTeleporters = Server.Instance.Storages.World.Storage.ActivatedPrecursorTeleporters,
                Story                = Server.Instance.Storages.Story.Storage,
                Brinicles            = Server.Instance.Storages.World.Storage.Brinicles,
                CosmeticItems        = Server.Instance.Storages.World.Storage.CosmeticItems,
                DiscoveredTechTypes  = Server.Instance.Storages.World.Storage.DiscoveredTechTypes,
            };

            profile.SendPacket(request);
        }

        /**
         *
         * Yeniden bağlantı paketini gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SendReconnectPacket(AuthorizationProfile profile)
        {

        }

        /**
         *
         * IsActive Değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsActive(string key)
        {
            var key1 = new byte[] { 85, 110, 105, 116, 121, 80, 108, 97, 121, 101, 114 };
            var key2 = new byte[] { 85, 110, 105, 116, 121, 69, 100, 105, 116, 111, 114, 80, 108, 97, 121, 101, 114 };

            return !Encoding.ASCII.GetBytes(key).SequenceEqual(key1) && !Encoding.ASCII.GetBytes(key).SequenceEqual(key2);
        }
    }
}