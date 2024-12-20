namespace Subnautica.API.Features
{
    using Subnautica.API.Enums;

    public abstract class SubnauticaPlugin
    {
        /**
         *
         * Eklenti Adı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual string Name { get; }

        /**
         *
         * Eklenti önceliği
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual SubnauticaPluginPriority Priority { get; set; } = SubnauticaPluginPriority.Medium;

        /**
         *
         * Sınıf ayarlarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SubnauticaPlugin()
        {
        }

        /**
         *
         * Eklenti aktif edildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual void OnEnabled()
        {
        }

        /**
         *
         * Eklenti pasif edildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual void OnDisabled()
        {
        }
    }
}
