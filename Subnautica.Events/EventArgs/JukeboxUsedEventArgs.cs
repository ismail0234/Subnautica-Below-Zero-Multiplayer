namespace Subnautica.Events.EventArgs
{
    using System;

    using Subnautica.API.Features;

    public class JukeboxUsedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public JukeboxUsedEventArgs(string uniqueId, CustomProperty data, bool isSeaTruckModule)
        {
            this.UniqueId         = uniqueId;
            this.Data             = data;
            this.IsSeaTruckModule = isSeaTruckModule;
        }

        /**
         *
         * Kimlik
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; private set; }

        /**
         *
         * Data Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CustomProperty Data { get; private set; }

        /**
         *
         * IsSeaTruckModule Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsSeaTruckModule { get; private set; }
    }
}
