namespace Subnautica.Server.Storage
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Core;
    using Subnautica.Server.Abstracts;
    using Subnautica.Server.Core;

    public class Player : BaseStorage
    {
        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void Start(string serverId)
        {
            this.ServerId = serverId;
            this.Load();
        }

        /**
         *
         * Sunucu tarafı Players yolunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string GetPlayerFilePath(string playerUniqueId)
        {
            return Paths.GetMultiplayerServerPlayerSavePath(this.ServerId, string.Format("{0}.bin", playerUniqueId));
        }

        /**
         *
         * Sunucu oyuncu ayarlarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void Load()
        {
            Directory.CreateDirectory(Paths.GetMultiplayerServerPlayerSavePath(this.ServerId));
        }

        /**
         *
         * Oyuncu verisini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public AuthorizationProfile GetPlayerData(string playerUniqueId, string playerName)
        {
            if (string.IsNullOrEmpty(playerUniqueId))
            {
                return null;
            }

            var filePath = this.GetPlayerFilePath(playerUniqueId);
            if (!File.Exists(filePath))
            {
                var profile = new AuthorizationProfile
                {
                    PlayerName = playerName,
                    UniqueId   = playerUniqueId,
                };

                profile.SaveToDisk();
                return profile;
            }

            try
            {
                return NetworkTools.Deserialize<AuthorizationProfile>(File.ReadAllBytes(filePath));
            }
            catch (Exception e)
            {
                Log.Error($"Player.GetPlayerData: {e}");
            }

            return null;
        }

        /**
         *
         * Bütün oyuncuları döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public List<AuthorizationProfile> GetAllPlayers()
        {
            var players = new List<AuthorizationProfile>();

            foreach (var filePath in Directory.GetFiles(Paths.GetMultiplayerServerPlayerSavePath(this.ServerId)))
            {
                var uniqueId = Path.GetFileName(filePath).Replace(".bin", "");
                if (uniqueId.IsNull())
                {
                    continue;
                }

                try
                {
                    var player = NetworkTools.Deserialize<AuthorizationProfile>(File.ReadAllBytes(filePath));
                    player.UniqueId = uniqueId;

                    players.Add(player);
                }
                catch (Exception e)
                {
                    Log.Error($"Player.GetPlayerData: {e}");
                }
            }

            return players;
        }

        /**
         *
         * Verileri diske yazar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void SaveToDisk()
        {
            foreach (var player in Core.Server.Instance.Players)
            {
                if (player.Value.IsAuthorized)
                {
                    player.Value.SaveToDisk();
                }
            }
        }
    }
}
