namespace Subnautica.API.Features.Creatures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    using Subnautica.API.Features.Helper;

    public class CreatureQueueItem
    {
        /**
         *
         * CreatureId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ushort CreatureId { get; set; }

        /**
         *
         * IsSpawn değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsSpawn { get; set; }

        /**
         *
         * IsProcess değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsProcess { get; set; }

        /**
         *
         * IsChangeOWS değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsChangeOWS { get; set; }

        /**
         *
         * IsDeath değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsDeath { get; set; }

        /**
         *
         * ItemQueueAction Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CreatureQueueAction Action { get; set; }
    }

    public class CreatureQueueAction
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
         * İşlem türünde işlem tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Action<MultiplayerCreature, CreatureQueueItem> OnProcessCompleted { get; set; }

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
            if (property == null)
            {
                return default(T);
            }

            return (T) property.GetValue<T>();
        }
    }
}