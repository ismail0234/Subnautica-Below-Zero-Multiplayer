namespace Subnautica.API.Features.Helper
{
    using System.Collections.Generic;
    
    public class ApiCreditsDataFormat
    {
        /**
         *
         * ProjectOwner değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ApiCreditsDataItemFormat ProjectOwner { get; set; } = new ApiCreditsDataItemFormat();
        
        /**
         *
         * ServerOwners değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ApiCreditsDataItemFormat ServerOwners { get; set; } = new ApiCreditsDataItemFormat();
        
        /**
         *
         * DiscordAdmins değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ApiCreditsDataItemFormat DiscordAdmins { get; set; } = new ApiCreditsDataItemFormat();
        
        /**
         *
         * DiscordMods değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ApiCreditsDataItemFormat DiscordMods { get; set; } = new ApiCreditsDataItemFormat();
        
        /**
         *
         * PatreonSupporters değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ApiCreditsDataItemFormat PatreonSupporters { get; set; } = new ApiCreditsDataItemFormat();
        
        /**
         *
         * Translators değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ApiCreditsDataItemFormat Translators { get; set; } = new ApiCreditsDataItemFormat();
        
        /**
         *
         * AlphaTesters değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ApiCreditsDataItemFormat AlphaTesters { get; set; } = new ApiCreditsDataItemFormat();
    }

    public class ApiCreditsDataItemFormat
    {
        /**
         *
         * Grup adını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */        
        public string Name { get; set; }

        /**
         *
         * Grup adını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */        
        public List<ApiCreditsDataMemberItemFormat> Members { get; set; } = new List<ApiCreditsDataMemberItemFormat>();
    }

    public class ApiCreditsDataMemberItemFormat
    {
        /**
         *
         * İsmi barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */        
        public string Name { get; set; }
    }
}
