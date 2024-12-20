namespace Subnautica.Network.Models.Storage.World.Childrens
{
    using System.Collections.Generic;
    using System.Linq;

    using MessagePack;

    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class Base
    {
        /**
         *
         * BaseId değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string BaseId { get; set; }

        /**
         *
         * BaseColor değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public ZeroColor BaseColor { get; set; }

        /**
         *
         * StripeColor1 değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public ZeroColor StripeColor1 { get; set; }

        /**
         *
         * StripeColor2 değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public ZeroColor StripeColor2 { get; set; }

        /**
         *
         * NameColor değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public ZeroColor NameColor { get; set; }
        
        /**
         *
         * Name değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public string Name { get; set; }

        /**
         *
         * DisablePowers değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public HashSet<ZeroInt3> DisablePowers { get; set; } = new HashSet<ZeroInt3>();

        /**
         *
         * MinimapPositions değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public Dictionary<string, ZeroVector3> MinimapPositions { get; set; } = new Dictionary<string, ZeroVector3>();

        /**
         *
         * Leakers değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public HashSet<Leaker> Leakers { get; set; } = new HashSet<Leaker>();

        /**
         *
         * CellWaterLevels değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public Dictionary<ushort, float> CellWaterLevels { get; set; } = new Dictionary<ushort, float>();

        /**
         *
         * Üs renk ayarlarını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetColorCustomizer(string name, ZeroColor baseColor, ZeroColor stripeColor1, ZeroColor stripeColor2, ZeroColor nameColor)
        {
            this.Name         = name;
            this.BaseColor    = baseColor;
            this.StripeColor1 = stripeColor1;
            this.StripeColor2 = stripeColor2;
            this.NameColor    = nameColor;
        }

        /**
         *
         * Su seviyesini ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetCellWaterLevel(ushort index, float waterLevel)
        {
            if (waterLevel > 0f)
            {
                this.CellWaterLevels[index] = waterLevel;
            }
            else
            {
                this.CellWaterLevels.Remove(index);
            }
        }

        /**
         *
         * Yapıya ait verileri siler.
         *
         * @author Ismail Satilmis <ismaiil_0234@hotmail.com>
         *
         */
        public void RemoveConstruction(string constructionId, ZeroInt3 cell)
        {
            if (cell != null)
            {
                this.DisablePowers.RemoveWhere(q => q == cell);
            }

            this.MinimapPositions.Remove(constructionId);
            this.Leakers.RemoveWhere(q => q.UniqueId == constructionId);
        }

        /**
         *
         * Sızıntı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool TryGetLeaker(string uniqueId, out Leaker leaker)
        {
            leaker = this.Leakers.FirstOrDefault(q => q.UniqueId == uniqueId);
            if (leaker == null)
            {
                leaker = new Leaker()
                {
                    UniqueId = uniqueId,
                };

                this.Leakers.Add(leaker);
            }

            return true;
        }

        /**
         *
         * Sızıntı yapılan bir yer ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool UpdateLeakPoints(string uniqueId, float currentHealth, float maxHealth, List<ZeroVector3> leakPoints = null, ZeroVector3 playerPosition = null)
        {
            if (this.TryGetLeaker(uniqueId, out var leaker))
            {
                if (leakPoints != null)
                {
                    leaker.UpdateMaxLeakCount(leakPoints.Count);
                }

                var numLeakPoints = global::Leakable.ComputeNumLeakPoints(currentHealth / maxHealth, leaker.MaxLeakCount);
                if (leaker.GetLeakCount() == numLeakPoints)
                {
                    return false;
                }

                numLeakPoints -= leaker.GetLeakCount();

                if (playerPosition == null)
                {
                    foreach (var point in leakPoints)
                    {
                        if (numLeakPoints > 0 && !leaker.Points.Any(q => q == point))
                        {
                            numLeakPoints--;

                            leaker.Points.Add(point);
                        }
                    }
                }
                else
                {                    
                    while (numLeakPoints < 0)
                    {
                        numLeakPoints++;

                        var lastDistance = 999999f;
                        var lastIndex    = -1;

                        for (int i = 0; i < leaker.Points.Count; i++)
                        {
                            var distance = playerPosition.Distance(leaker.Points.ElementAt(i));
                            if (distance < lastDistance)
                            {
                                lastDistance = distance;
                                lastIndex    = i;
                            }
                        }

                        if (lastIndex != -1)
                        {
                            leaker.Points.RemoveAt(lastIndex);
                        }
                    }
                }
            }

            return true;
        }
    }

    [MessagePackObject]
    public class Leaker
    {
        /**
         *
         * UniqueId değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string UniqueId { get; set; }

        /**
         *
         * Points değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public List<ZeroVector3> Points { get; set; } = new List<ZeroVector3>();

        /**
         *
         * MaxLeakCount değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public byte MaxLeakCount { get; set; }

        /**
         *
         * Max sızıntı sayısını günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void UpdateMaxLeakCount(int maxLeakCount)
        {
            if (this.MaxLeakCount == 0)
            {
                this.MaxLeakCount = (byte) maxLeakCount;
            }
        }

        /**
         *
         * Toplam sızıntı sayısını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int GetLeakCount()
        {
            return this.Points.Count;
        }
    }
}