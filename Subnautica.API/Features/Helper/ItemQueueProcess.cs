namespace Subnautica.API.Features.Helper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Subnautica.Network.Structures;

    using UnityEngine;

    public class ItemQueueProcess
    {
        /**
         *
         * Yumurtlama olup/olmadığı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsSpawning { get; set; } = false;

        /**
         *
         * İşlem olup/olmadığı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsProcess { get; set; } = false;

        /**
         *
         * Nesne Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; set; } = TechType.None;

        /**
         *
         * ItemId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string ItemId { get; set; }

        /**
         *
         * Item Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte[] Item { get; set; }

        /**
         *
         * Container Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ItemsContainer Container { get; set; }

        /**
         *
         * Transform Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroTransform Transform { get; set; }

        /**
         *
         * SlotId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string SlotId { get; set; }

        /**
         *
         * Equipment Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Equipment Equipment { get; set; }

        /**
         *
         * Pickupable Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Pickupable Pickupable { get; set; }

        /**
         *
         * ItemQueueAction Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ItemQueueAction Action { get; set; } = new ItemQueueAction();

        /**
         *
         * Tüm veriyi temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Dipose()
        {
            this.IsSpawning = false;
            this.IsProcess  = false;
            this.TechType   = TechType.None;
            this.Item       = null;
            this.Container  = null;
            this.Equipment  = null;
            this.Pickupable = null;
        }
    }

    public class ItemQueueAction
    {
        /**
         *
         * Özellikleri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
       private List<GenericProperty> Properties = new List<GenericProperty>();

        /**
         *
         * Nesne doğarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Func<ItemQueueProcess, bool> OnEntitySpawning { get; set; }

        /**
         *
         * Nesne doğduktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Action<ItemQueueProcess, Pickupable, GameObject> OnEntitySpawned { get; set; }

        /**
         *
         * İşlem türünde işlem tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Action<ItemQueueProcess> OnProcessCompleted { get; set; }

        /**
         *
         * İşlem türünde asenkron işlem tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Func<ItemQueueProcess, IEnumerator> OnProcessCompletedAsync { get; set; }

        /**
         *
         * Nesne yok edildikten sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Action<ItemQueueProcess> OnEntityRemoved { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ItemQueueAction(Func<ItemQueueProcess, bool> entitySpawning)
        {
            this.OnEntitySpawning = entitySpawning;
        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ItemQueueAction(Action<ItemQueueProcess, Pickupable, GameObject> entitySpawned = null)
        {
            this.OnEntitySpawned = entitySpawned;
        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ItemQueueAction(Func<ItemQueueProcess, bool> entitySpawning, Action<ItemQueueProcess, Pickupable, GameObject> entitySpawned)
        {
            this.OnEntitySpawning = entitySpawning;
            this.OnEntitySpawned  = entitySpawned;
        }

        /**
         *
         * Özellik kaydı yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void RegisterProperty(string key, object value)
        {
            this.Properties.Add(new GenericProperty(key, value));
        }

        /**
         *
         * Özellik kaydı yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public T GetProperty<T>(string key)
        {
            var property = this.Properties.Where(q => q.Key == key).FirstOrDefault();
            if (property == null || property.Value == null)
            {
                return default(T);
            }

            return (T) property.Value;
        }
    }
}
