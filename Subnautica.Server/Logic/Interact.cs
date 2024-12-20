namespace Subnautica.Server.Logic
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Server.Abstracts;

    using ClientModel = Subnautica.Network.Models.Client;

    public class Interact : BaseLogic
    {
        /**
         *
         * Etkileşim Ping Süresi 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const float PingTime = 0.15f;

        /**
         *
         * Etkileşim Süreleri (Tahmini)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const float Bench_Standup            = PingTime + 1.4f;
        public const float Bed_Standup              = PingTime + 6.1f;
        public const float BulkheadDoor             = PingTime + 4.1f;
        public const float Climb                    = PingTime + 2.3f;
        public const float UseVehicleBay            = PingTime + 1.5f;
        public const float HoverpadDocking          = PingTime + 3f;
        public const float SeaTruckEndPilot         = PingTime + 2.5f;
        public const float SeaTruckModuleEndPilot   = PingTime + 0.75f;
        public const float ConstructionTimeout      = PingTime + 0.5f;
        public const float LaserCutterTimeout       = PingTime + 1f;
        public const float RadioTowerTomUsing       = PingTime + 7f;
        public const float ElevatorCall             = PingTime + 11f;
        public const float LifePodUseableDiveHatch  = PingTime + 1.7f;
        public const float NormalUseableDiveHatch   = PingTime + 1.4f;
        public const float BulkHeadUseableDiveHatch = PingTime + 4f;
        public const float MoonpoolExosuitDock      = PingTime + 7f;
        public const float MoonpoolSeaTruckDock     = PingTime + 7.5f;
        public const float MoonpoolExosuitUndock    = PingTime + 3.25f;
        public const float MoonpoolSeaTruckUndock   = PingTime + 6.25f;

        /**
         *
         * Yaratık Etkileşim Süreleri (Tahmini)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const float CreatureGlowWhaleEyeInteract     = PingTime + 10f;
        public const float CreatureLilyPaddlerHypnotize     = PingTime + 5f;
        public const float CreatureCheliceratePlayerAttack  = PingTime + 5f;
        public const float CreatureChelicerateVehicleAttack = PingTime + 6f;
        public const float ShadowLeviathanPlayerAttack      = PingTime + 5f;
        public const float ShadowLeviathanVehicleAttack     = PingTime + 6f;

        /**
         *
         * Bloklu listeyi barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Dictionary<string, string> List { get; set; } = new Dictionary<string, string>();

        /**
         *
         * Gecikmeli blok kaldırılacak listeyi barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<StopwatchItem> RemovingList { get; set; } = new List<StopwatchItem>();

        /**
         *
         * Her tick'den sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnUpdate(float deltaTime)
        {
            if (this.RemovingList.Count > 0)
            {
                var list = this.RemovingList.Where(q => q.IsFinished()).ToList();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        this.List.Remove(item.GetCustomData<string>());
                        this.RemovingList.Remove(item);
                    }

                    this.SendListToPlayers();
                }
            }   
        }

        /**
         *
         * Tüm oyunculara yeni listeyi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SendListToPlayers()
        {
            ClientModel.InteractArgs request = new ClientModel.InteractArgs()
            {
                List = this.List
            };

            Core.Server.SendPacketToAllClient(request);
        }

        /**
         *
         * Bloklu listeye ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool AddBlock(string playerUniqueId, string constructionId, bool autoSend = false)
        {
            this.RemoveTimingItem(playerUniqueId);
            this.List[playerUniqueId] = constructionId;

            if (autoSend)
            {
                this.SendListToPlayers();
            }
            
            return true;
        }

        /**
         *
         * Bloklu listeden kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool RemoveBlockByPlayerId(string playerUniqueId, float delayTime = 0.0f, bool autoSend = true)
        {
            delayTime *= 1000f;

            this.RemoveTimingItem(playerUniqueId);

            if (delayTime <= 0.0f)
            {
                var response = this.List.Remove(playerUniqueId);

                this.SendListToPlayers();
                return response;
            }

            this.RemovingList.Add(new StopwatchItem(delayTime, playerUniqueId));
            return true;
        }

        /**
         *
         * Bloklu olup olmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsBlocked(string constructionId, string ignorePlayerUniqueId = null)
        {
            if (constructionId.IsNull())
            {
                return true;
            }

            var interact = this.List.Where(q => q.Value == constructionId).FirstOrDefault();
            if (interact.Value == null)
            {
                return false;
            }

            if (ignorePlayerUniqueId != null)
            {
                return interact.Key != ignorePlayerUniqueId;
            }

            return true;
        }

        /**
         *
         * Bloklu olup olmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsBlockedByPlayer(string playerUniqueId)
        {
            return this.List.ContainsKey(playerUniqueId);
        }

        /**
         *
         * Oyuncu tarafından bloklu olup olmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsBlockedByPlayer(string playerUniqueId, string constructionId)
        {
            return this.List.TryGetValue(playerUniqueId, out var tempId) && tempId == constructionId;
        }

        /**
         *
         * Bloklu olup olmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsBlockedByConstruction(string constructionId)
        {
            return this.List.Where(q => q.Value == constructionId).Count() > 0;
        }

        /**
         *
         * Zamanlanmış nesneyi kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void RemoveTimingItem(string playerId)
        {
            this.RemovingList.RemoveAll(q => q.GetCustomData<string>() == playerId);
        }
    }
}
