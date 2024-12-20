namespace Subnautica.Network.Core.Components
{
    using System;

    using MessagePack;

    using Metadata = Subnautica.Network.Models.Metadata;

    [Union(0, typeof(Metadata.Aquarium))]
    [Union(1, typeof(Metadata.AromatherapyLamp))]
    [Union(2, typeof(Metadata.EmmanuelPendulum))]
    [Union(3, typeof(Metadata.Shower))]
    [Union(4, typeof(Metadata.Sink))]
    [Union(5, typeof(Metadata.SmallStove))]
    [Union(6, typeof(Metadata.Toilet))]
    [Union(7, typeof(Metadata.Snowman))]
    [Union(8, typeof(Metadata.Crafter))]
    [Union(9, typeof(Metadata.Jukebox))]
    [Union(10, typeof(Metadata.JukeboxUsed))]
    [Union(11, typeof(Metadata.Sign))]
    [Union(12, typeof(Metadata.PictureFrame))]
    [Union(13, typeof(Metadata.Trashcans))]
    [Union(14, typeof(Metadata.LabTrashcan))]
    [Union(15, typeof(Metadata.BioReactor))]
    [Union(16, typeof(Metadata.NuclearReactor))]
    [Union(17, typeof(Metadata.CoffeeVendingMachine))]
    [Union(18, typeof(Metadata.Charger))]
    [Union(19, typeof(Metadata.StorageItem))]
    [Union(20, typeof(Metadata.StorageContainer))]
    [Union(21, typeof(Metadata.Hoverpad))]
    [Union(22, typeof(Metadata.Recyclotron))]
    [Union(23, typeof(Metadata.Fridge))]
    [Union(24, typeof(Metadata.FiltrationMachine))]
    [Union(25, typeof(Metadata.Planter))]
    [Union(26, typeof(Metadata.Bed))]
    [Union(27, typeof(Metadata.Bench))]
    [Union(28, typeof(Metadata.BulkheadDoor))]
    [Union(29, typeof(Metadata.BaseControlRoom))]
    [Union(30, typeof(Metadata.SpotLight))]
    [Union(31, typeof(Metadata.TechLight))]
    [Union(32, typeof(Metadata.StorageLocker))]
    [Union(33, typeof(Metadata.BaseMoonpool))]
    [Union(34, typeof(Metadata.BaseMapRoom))]
    [Union(35, typeof(Metadata.BaseMoonpoolExpansionManager))]
    [Union(36, typeof(Metadata.BaseWaterPark))]
    [MessagePackObject]
    public abstract class MetadataComponent
    {
        /**
         *
         * Komponenti döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public T GetComponent<T>()
        {
            if (this is T)
            {
                return (T)Convert.ChangeType(this, typeof(T));
            }

            return default(T);
        }
    }
}
