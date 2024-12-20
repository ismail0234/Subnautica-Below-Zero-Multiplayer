namespace Subnautica.API.Features.NetworkUtility
{
    using System.Linq;

    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Client;
    using Subnautica.Network.Models.Storage.World.Childrens;

    public class Session
    {
        /**
         *
         * Sunucu verilerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public JoiningServerArgs Current { get; private set; }

        /**
         *
         * EndGameWorldTime değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public double EndGameWorldTime { get; private set; }

        /**
         *
         * Oyuncu seatruck içerisinde mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsInSeaTruck { get; set; }

        /**
         *
         * Sunucu verilerini günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetSession(JoiningServerArgs session)
        {
            this.Current = session;
        }

        /**
         *
         * Keşfeldilmiş teknoloji ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void AddDiscoveredTechType(TechType techType)
        {
            this.Current.DiscoveredTechTypes.Add(techType);
        }

        /**
         *
         * Brinicle değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Brinicle GetBrinicle(string uniqueId)
        {
            return this.Current.Brinicles.FirstOrDefault(q => q.UniqueId == uniqueId);
        }

        /**
         *
         * Brinicle değerini günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetBrinicle(Brinicle brinicle)
        {
            this.Current.Brinicles.RemoveWhere(q => q.UniqueId == brinicle.UniqueId);
            this.Current.Brinicles.Add(brinicle);
        }

        /**
         *
         * Brinicle var olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsBrinicleExists(string uniqueId)
        {
            return this.Current.Brinicles.Any(q => q.UniqueId == uniqueId);
        }

        /**
         *
         * CosmeticItem var olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsCosmeticItemExists(string uniqueId)
        {
            return this.Current.CosmeticItems.Any(q => q.StorageItem.ItemId == uniqueId);
        }

        /**
         *
         * Cosmetic Item nesnesini değiştirir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetCosmeticItem(CosmeticItem cosmeticItem)
        {
            this.Current.CosmeticItems.RemoveWhere(q => q.StorageItem.ItemId == cosmeticItem.StorageItem.ItemId);
            this.Current.CosmeticItems.Add(cosmeticItem);
        }

        /**
         *
         * Cosmetic Item nesnesini değiştirir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void RemoveCosmeticItem(string uniqueId)
        {
            this.Current.CosmeticItems.RemoveWhere(q => q.StorageItem.ItemId == uniqueId);
        }

        /**
         *
         * Sunucu verilerini günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool SetConstructionComponent(string uniqueId, MetadataComponent component)
        {
            var construction = this.Current.Constructions.FirstOrDefault(q => q.UniqueId == uniqueId);
            if (construction == null)
            {
                return false;
            }

            construction.Component = component;
            return true;
        }

        /**
         *
         * Oyun zamanını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public double GetWorldTime()
        {
            if (BelowZeroEndGame.isActive)
            {
                return this.EndGameWorldTime;
            }

            return DayNightCycle.main.timePassedAsDouble;
        }

        /**
         *
         * Oyun sonu zamanını değiştirir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetEndGameWorldTime(double time, bool isAdd = false)
        {
            if (isAdd)
            {
                this.EndGameWorldTime += time;
            }
            else
            {
                this.EndGameWorldTime = time;
            }
        }

        /**
         *
         * Bütün verileri temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Dispose()
        {
            this.Current      = null;
            this.IsInSeaTruck = false;
        }
    }
}
