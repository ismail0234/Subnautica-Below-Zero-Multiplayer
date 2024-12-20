namespace Subnautica.Network.Models.Construction.Shared
{
    using MessagePack;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Core;
    using Subnautica.Network.Structures;

    using System.Collections.Generic;

    [MessagePackObject]
    public class BaseComponent
    {
        /**
         *
         * Faces değerini saklar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public Dictionary<int, global::Base.FaceType> Faces { get; set; } = new Dictionary<int, global::Base.FaceType>();

        /**
         *
         * Cells değerini saklar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public Dictionary<int, global::Base.CellType> Cells { get; set; } = new Dictionary<int, global::Base.CellType>();

        /**
         *
         * Links değerini saklar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public Dictionary<int, byte> Links { get; set; } = new Dictionary<int, byte>();

        /**
         *
         * Masks değerini saklar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public Dictionary<int, byte> Masks { get; set; } = new Dictionary<int, byte>();

        /**
         *
         * IsGlass değerini saklar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public Dictionary<int, bool> IsGlass { get; set; } = new Dictionary<int, bool>();

        /**
         *
         * Unpowered değerini saklar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public Dictionary<int, bool> Unpowered { get; set; } = new Dictionary<int, bool>();

        /**
         *
         * GridShape değerini saklar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public ZeroInt3 GridShape { get; set; }

        /**
         *
         * CellOffset değerini saklar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public ZeroInt3 CellOffset { get; set; }

        /**
         *
         * Anchor değerini saklar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public ZeroInt3 Anchor { get; set; }

        /**
         *
         * FaceLength değerini saklar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public int FaceLength { get; set; } = -1;

        /**
         *
         * CellLength değerini saklar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(10)]
        public int CellLength { get; set; } = -1;

        /**
         *
         * LinkLength değerini saklar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(11)]
        public int LinkLength { get; set; } = -1;

        /**
         *
         * MaskLength değerini saklar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(12)]
        public int MaskLength { get; set; } = -1;

        /**
         *
         * GlassLength değerini saklar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(13)]
        public int GlassLength { get; set; } = -1;

        /**
         *
         * PowerLength değerini saklar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(14)]
        public int PowerLength { get; set; } = -1;

        /**
         *
         * Bileşenleri ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetComponent(global::Base baseComponent)
        {
            this.FaceLength  = this.GetArrayLength(baseComponent.faces);
            this.CellLength  = this.GetArrayLength(baseComponent.cells);
            this.LinkLength  = this.GetArrayLength(baseComponent.links);
            this.MaskLength  = this.GetArrayLength(baseComponent.masks);
            this.GlassLength = this.GetArrayLength(baseComponent.isGlass);
            this.PowerLength = this.GetArrayLength(baseComponent.unpowered);

            for (int i = 0; i < this.FaceLength; i++)
            {
                var data = baseComponent.faces[i];
                if (data != global::Base.FaceType.None)
                {
                    this.Faces.Add(i, data);
                }
            }

            for (int i = 0; i < this.CellLength; i++)
            {
                var data = baseComponent.cells[i];
                if (data != global::Base.CellType.Empty)
                {
                    this.Cells.Add(i, data);
                }
            }

            for (int i = 0; i < this.LinkLength; i++)
            {
                var data = baseComponent.links[i];
                if (data != 0)
                {
                    this.Links.Add(i, data);
                }
            }

            for (int i = 0; i < this.MaskLength; i++)
            {
                var data = baseComponent.masks[i];
                if (data != 0)
                {
                    this.Masks.Add(i, data);
                }
            }

            for (int i = 0; i < this.GlassLength; i++)
            {
                var data = baseComponent.isGlass[i];
                if (data != false)
                {
                    this.IsGlass.Add(i, data);
                }
            }

            for (int i = 0; i < this.PowerLength; i++)
            {
                var data = baseComponent.unpowered[i];
                if (data != false)
                {
                    this.Unpowered.Add(i, data);
                }
            }

            this.GridShape  = baseComponent.baseShape.ToInt3().ToZeroInt3();
            this.Anchor     = baseComponent.anchor.ToZeroInt3();
            this.CellOffset = baseComponent.cellOffset.ToZeroInt3();
        }

        /**
         *
         * Verileri içe aktarır
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void ImportToBase(global::Base baseComponent)
        {
            baseComponent.baseShape  = new Grid3Shape(this.GridShape.ToInt3());
            baseComponent.anchor     = this.Anchor.ToInt3();
            baseComponent.cellOffset = this.CellOffset.ToInt3();

            if (this.FaceLength != -1)
            {
                baseComponent.faces = new global::Base.FaceType[this.FaceLength];
            }

            if (this.CellLength != -1)
            {
                baseComponent.cells = new global::Base.CellType[this.CellLength];
            }

            if (this.LinkLength != -1)
            {
                baseComponent.links = new byte[this.LinkLength];
            }

            if (this.MaskLength != -1)
            {
                baseComponent.masks = new byte[this.MaskLength];
            }

            if (this.GlassLength != -1)
            {
                baseComponent.isGlass = new bool[this.GlassLength];
            }

            if (this.PowerLength != -1)
            {
                baseComponent.unpowered = new bool[this.PowerLength];
            }

            foreach (var item in this.Faces)
            {
                baseComponent.faces[item.Key] = item.Value;
            }

            foreach (var item in this.Cells)
            {
                baseComponent.cells[item.Key] = item.Value;
            }

            foreach (var item in this.Links)
            {
                baseComponent.links[item.Key] = item.Value;
            }

            foreach (var item in this.Masks)
            {
                baseComponent.masks[item.Key] = item.Value;
            }

            foreach (var item in this.IsGlass)
            {
                baseComponent.isGlass[item.Key] = item.Value;
            }

            foreach (var item in this.Unpowered)
            {
                baseComponent.unpowered[item.Key] = item.Value;
            }
        }

        /**
         *
         * Veriyi sıkıştırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static byte[] Serialize(global::Base baseComponent)
        {
            var component = new BaseComponent();
            component.SetComponent(baseComponent);

            return NetworkTools.Serialize(component);
        }

        /**
         *
         * Bilgileri gösterir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void ShowDetails()
        {
            Log.Info($"[Base Details] FaceLength: {this.FaceLength}, CellLength: {this.CellLength}, LinkLength: {this.LinkLength}, MaskLength: {this.MaskLength}, GlassLength: {this.GlassLength}, PowerLength: {this.PowerLength}");
        }

        /**
         *
         * Dizi uzunluğunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private int GetArrayLength<T>(T[] arrayList)
        {
            return arrayList == null ? -1 : arrayList.Length;
        }
    }
}
