namespace Subnautica.Network.Models.Storage.Scanner
{
    using MessagePack;

    using System;
    using System.Collections.Generic;

    [MessagePackObject]
    [Serializable]
    public class Scanner
    {
        /**
         *
         * Taranmış teknolojileri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public HashSet<TechType> Technologies { get; set; } = new HashSet<TechType>();
    }
}