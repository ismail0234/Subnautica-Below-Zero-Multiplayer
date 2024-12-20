namespace Subnautica.API.Features.Helper
{
    public class FirewallItemFormat
    {
        /**
         *
         * Name değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Name { get; set; }
        /**
         *
         * Description değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Description { get; set; }

        /**
         *
         * Path değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Path { get; set; }

        /**
         *
         * IsEnabled değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsEnabled { get; set; }

         /**
         *
         * IsPublicProfile değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */       
        public bool IsPublicProfile { get; set; }

         /**
         *
         * IsPrivateProfile değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */       
        public bool IsPrivateProfile { get; set; }

        /**
         *
         * IsDomainProfile değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */        
        public bool IsDomainProfile { get; set; }

        /**
         *
         * IsUdp değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsUdp { get; set; }

        /**
         *
         * IsTcp değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsTcp { get; set; }

        /**
         *
         * IsAllow değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllow { get; set; }
    }
}
