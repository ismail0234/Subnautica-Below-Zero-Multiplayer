namespace Subnautica.Network.Models.WorldEntity.DynamicEntityComponents
{
    using MessagePack;

    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class QuantumLocker : NetworkDynamicEntityComponent
    {
        /**
         *
         * StorageContainer Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsDeployed { get; set; } = true;
    }
}