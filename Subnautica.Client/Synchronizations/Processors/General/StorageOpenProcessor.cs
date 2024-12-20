namespace Subnautica.Client.Synchronizations.Processors.General
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class StorageOpenProcessor : NormalProcessor
    {

        /**
         *
         * Açılmış depoları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private HashSet<string> OpenedStorages = new HashSet<string>();

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.StorageOpenArgs>();
            if (packet == null)
            {
                return true;
            }

            this.OpenStorage(packet.UniqueId, packet.TechType, ZeroPlayer.IsPlayerMine(packet.GetPacketOwnerId()));
            return true;
        }

        /**
         *
         * Her Sabit karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnFixedUpdate()
        {
            if (World.IsLoaded && this.OpenedStorages.Count > 0)
            {
                foreach (var uniqueId in this.OpenedStorages.ToList())
                {
                    if (Interact.IsBlocked(uniqueId))
                    {
                        continue;
                    }

                    this.OpenedStorages.Remove(uniqueId);

                    this.CloseStorage(uniqueId);
                }
            }
        }

        /**
         *
         * Depoyu açar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool OpenStorage(string uniqueId, TechType techType, bool isMine = false)
        {
            using (EventBlocker.Create(TechType.BaseBioReactor))
            using (EventBlocker.Create(TechType.BaseNuclearReactor))
            {
                if (techType == TechType.BaseBioReactor)
                {
                    if (isMine)
                    {
                        Network.Identifier.GetComponentByGameObject<global::BaseBioReactorGeometry>(uniqueId, true)?.OnUse(null);
                    }
                }
                else if (techType == TechType.BaseNuclearReactor)
                {   
                    if (isMine)
                    {
                        Network.Identifier.GetComponentByGameObject<global::BaseNuclearReactorGeometry>(uniqueId, true)?.OnUse(null);
                    }
                }
                else if (techType == TechType.BaseMapRoom)
                {   
                    if (isMine)
                    {
                        var baseDeconstructable = Network.Identifier.GetComponentByGameObject<global::BaseDeconstructable>(uniqueId);
                        if (baseDeconstructable)
                        {
                            var maproom = baseDeconstructable.GetMapRoomFunctionality();
                            if (maproom)
                            {
                                maproom.storageContainer.Open(maproom.transform);
                            }
                        }
                    }
                }
                else
                {
                    var gameObject = Network.Identifier.GetComponentByGameObject<global::StorageContainer>(uniqueId, true);
                    if (gameObject)
                    {
                        if (isMine)
                        {
                            gameObject.Open(gameObject.transform);
                        }
                        else
                        {
                            switch (techType)
                            {
                                case TechType.SmallStorage:
                                case TechType.Recyclotron:
                                case TechType.EscapePod:
                                case TechType.QuantumLocker:
                                case TechType.Exosuit:

                                    gameObject.open = true;

                                    this.OpenedStorages.Add(uniqueId);
                                    break;
                            }
                        }
                    }
                }
            }
            
            return true;
        }

        /**
         *
         * Depoyu kapatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void CloseStorage(string uniqueId)
        {
            var gameObject = Network.Identifier.GetComponentByGameObject<global::StorageContainer>(uniqueId, true);
            if (gameObject)
            {
                gameObject.open = false;
            }
        }

        /**
         *
         * PDA Açılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnStorageOpening(StorageOpeningEventArgs ev)
        {
            ev.IsAllowed = false;

            if (!Interact.IsBlocked(ev.ConstructionId))
            {
                StorageOpenProcessor.SendPacketToServer(ev.ConstructionId, ev.TechType);
            }
        }

        /**
         *
         * Sunucuya Veri Gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, TechType techType)
        {
            ServerModel.StorageOpenArgs request = new ServerModel.StorageOpenArgs()
            {
                UniqueId = uniqueId,
                TechType = techType,
            };

            NetworkClient.SendPacket(request);
        }
    }
}