namespace Subnautica.API.Features
{
    using Subnautica.API.Features.Creatures;
    using Subnautica.API.Features.NetworkUtility;

    using System;
    using System.Collections.Generic;

    using UnityEngine;

    public class Network
    {
        /**
         *
         * Mevcut oyuncunun barındırıcı olup olmadığını tutar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsHost 
        { 
            get 
            {
                return Server.Core.Server.Instance != null && Server.Core.Server.Instance.IsConnected;
            }
        }

        /**
         *
         * Çok oyunculu modunda mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsMultiplayerActive { get; set; }

        /**
         *
         * Üs yüz parçalarını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static BaseFacePiece BaseFacePiece { get; private set; } = new BaseFacePiece();

        /**
         *
         * Dünya üzerindeki dinamik nesneler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static DynamicEntity DynamicEntity { get; private set; } = new DynamicEntity();

        /**
         *
         * Dünya üzerindeki statik nesneler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static StaticEntity StaticEntity { get; private set; } = new StaticEntity();

        /**
         *
         * Kimlik Tanımlayıcı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Identifier Identifier { get; private set; } = new Identifier();

        /**
         *
         * Mevcut Oturum
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Session Session { get; private set; } = new Session();

        /**
         *
         * WorldStreamer Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static WorldStreamer WorldStreamer { get; private set; } = new WorldStreamer();

        /**
         *
         * Story Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Story Story { get; private set; } = new Story();

        /**
         *
         * Storage Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Storage Storage { get; private set; } = new Storage();

        /**
         *
         * HandTarget Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static HandTarget HandTarget { get; private set; } = new HandTarget();

        /**
         *
         * CellManager Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static CellManager CellManager { get; private set; } = new CellManager();

        /**
         *
         * Temporary Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Temporary Temporary { get; private set; } = new Temporary();

        /**
         *
         * EntityDatabase Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static EntityDatabase EntityDatabase { get; private set; } = new EntityDatabase();

        /**
         *
         * Creatures Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static MultiplayerCreatureManager Creatures { get; private set; } = new MultiplayerCreatureManager();

        /**
         *
         * InviteCode Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static InviteCode InviteCode { get; private set; } = new InviteCode();

        /**
         *
         * Tüm veriyi temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Dispose()
        {
            try 
            {
                World.SetLoaded(false);
                
                Network.IsMultiplayerActive = false;
                Network.BaseFacePiece.Dispose();
                Network.DynamicEntity.Dispose();
                Network.StaticEntity.Dispose();
                Network.Identifier.Dispose();
                Network.Session.Dispose();
                Network.WorldStreamer.Dispose();
                Network.Story.Dispose();
                Network.Storage.Dispose();
                Network.HandTarget.Dispose();
                Network.CellManager.Dispose();
                Network.Temporary.Dispose();
                Network.EntityDatabase.Dispose();
                Network.Creatures.Dispose();
                Network.InviteCode.Dispose();

                Entity.Dispose();
                Interact.Dispose();
                WaitingScreen.Dispose();
                World.Dispose();
                ZeroPlayer.DisposeAll();

                // NOT COMPLETED...
                Network.PersistentVirtualEntities.Clear();
            }
            catch (Exception ex)
            {
                Log.Error($"Network.Dispose -> Exception: {ex}");
            }
        }

        /**
         *
         * Max kanal sayısını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static byte GetChannelCount()
        {
            return 2;
        }

















        ///
        /// NOT COMPLETED...
        ///



        /**
         *
         * Max kanal sayısını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsExistsConstructionInServer(string unqiueId)
        {
            if (!Network.IsHost)
            {
                return false;
            }

            return Subnautica.Server.Core.Server.Instance.Storages.Construction.Storage.Constructions.ContainsKey(unqiueId);
        }


        /**
         *
         * Nesne Id'si döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetWorldEntityId(Vector3 position)
        {
            if (PersistentVirtualEntities.TryGetValue(position, out string uniqueId))
            {
                return uniqueId;
            }

            PersistentVirtualEntities[position] = position.GetHashCode().ToString();
            return PersistentVirtualEntities[position];
        }

        /**
         *
         * Dünyadaki tüm sabit nesneleri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static Dictionary<Vector3, string> PersistentVirtualEntities { get; private set; } = new Dictionary<Vector3, string>();

    }
}
