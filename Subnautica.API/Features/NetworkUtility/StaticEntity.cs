namespace Subnautica.API.Features.NetworkUtility
{
    using System.Collections.Generic;

    using Subnautica.Network.Core.Components;

    public class StaticEntity
    {
        /**
         *
         * Dünya üzerinde doğmayacak nesne id'leri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Dictionary<string, NetworkWorldEntityComponent> StaticEntities { get; private set; } = new Dictionary<string, NetworkWorldEntityComponent>();

        /**
         *
         * Kalıcı dünya nesnesi olup olmadığını kontrol eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsStaticEntity(string uniqueId)
        {
            return this.StaticEntities.ContainsKey(uniqueId);
        }

        /**
         *
         * Dünyadaki kalıcı nesneyi döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public NetworkWorldEntityComponent GetEntity(string uniqueId)
        {
            if (this.StaticEntities.TryGetValue(uniqueId, out var entity))
            {
                return entity;
            }

            return null;
        }

        /**
         *
         * Dünyadaki kalıcı nesneyi döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public T GetEntity<T>(string uniqueId)
        {
            if (this.StaticEntities.TryGetValue(uniqueId, out var entity) && entity != null)
            {
                return entity.GetComponent<T>();
            }

            return default(T);
        }

        /**
         *
         * Nesne'nin doğma durumunu kontrol eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsRestricted(string uniqueId)
        {
            if (this.StaticEntities.TryGetValue(uniqueId, out var entity) && entity != null)
            {
                return !entity.IsSpawnable;
            }

            return false;
        }

        /**
         *
         * Dünya kalıcı nesneyi düzenler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void AddStaticEntity(NetworkWorldEntityComponent entity)
        {
            if (entity != null)
            {
                this.StaticEntities[entity.UniqueId] = entity;
            }
        }

        /**
         *
         * Dünya kalıcı nesneyi slota ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void AddStaticEntitySlot(string uniqueId)
        {
            if (!this.StaticEntities.ContainsKey(uniqueId))
            {
                this.StaticEntities[uniqueId] = null;
            }
        }

        /**
         *
         * Dünya kalıcı nesneyi düzenler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetStaticEntities(Dictionary<string, NetworkWorldEntityComponent> entities)
        {
            this.StaticEntities = entities;
        }

        /**
         *
         * Verileri temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Dispose()
        {
            this.StaticEntities.Clear();
        }
    }
}