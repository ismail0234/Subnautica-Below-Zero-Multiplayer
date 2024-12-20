namespace Subnautica.Client.MonoBehaviours.Construction
{
    using System.Collections.Generic;

    using UnityEngine;

    public class BuilderBaseHullStrength : MonoBehaviour
    {
        /**
         *
         * BaseFloodSim nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private BaseFloodSim BaseFloodSim { get; set; }

        /**
         *
         * TargetWaterLevels nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<ushort, CellWaterLevelItem> TargetWaterLevels { get; set; } = new Dictionary<ushort, CellWaterLevelItem>();

        /**
         *
         * TargetLevelCount nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private int TargetLevelCount { get; set; } = 0;

        /**
         *
         * ActiveLevels nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private HashSet<ushort> ActiveLevels { get; set; } = new HashSet<ushort>();

        /**
         *
         * RemoveToActiveLevels nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private HashSet<ushort> RemoveToActiveLevels { get; set; } = new HashSet<ushort>();

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            this.BaseFloodSim = this.GetComponent<BaseFloodSim>();
        }

        /**
         *
         * Su seviye dizisinin boyutunu değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void ChangeCellWaterLevelSize(int size)
        {
            if (size > this.TargetLevelCount)
            {
                for (ushort i = (ushort) this.TargetWaterLevels.Count; i <= size; i++)
                {
                    this.TargetWaterLevels[i] = new CellWaterLevelItem();
                }

                this.TargetLevelCount = this.TargetWaterLevels.Count;
            }
        }

        /**
         *
         * Su seviyesini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetCellWaterLevel(ushort index, float value)
        {
            this.ChangeCellWaterLevelSize(index + 2);

            this.TargetWaterLevels[index].Reset();
            this.TargetWaterLevels[index].Start(index, value);

            this.ActiveLevels.Add(index);
        }

        /**
         *
         * Her karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Update()
        {
            if (this.ActiveLevels.Count > 0)
            {
                foreach (var index in this.ActiveLevels)
                {
                    var currentWaterLevel = this.GetBaseCellWaterLevel(index);
                    if (currentWaterLevel == -1f)
                    {
                        continue;
                    }

                    var item = this.TargetWaterLevels[index];
                    if (item.IsInitialized == false)
                    {
                        item.SetCurrentValue(currentWaterLevel);
                    }

                    item.InterpolateWater(Time.unscaledDeltaTime);

                    this.SetBaseCellWaterLevel(index, item.GetValue());

                    if (item.IsFinished())
                    {
                        this.RemoveToActiveLevels.Add(index);
                    }
                }

                if (this.RemoveToActiveLevels.Count > 0)
                {
                    foreach (var index in this.RemoveToActiveLevels)
                    {
                        this.ActiveLevels.Remove(index);
                    }

                    this.RemoveToActiveLevels.Clear();
                }
            }
        }

        /**
         *
         * Su seviyesini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SetBaseCellWaterLevel(ushort index, float waterLevel)
        {
            this.BaseFloodSim.cellWaterLevel[index] = waterLevel;
        }

        /**
         *
         * Su seviyesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float GetBaseCellWaterLevel(ushort index)
        {
            return index < this.BaseFloodSim.cellWaterLevel.Length ? this.BaseFloodSim.cellWaterLevel[index] : -1f;
        }

        /**
         *
         * Sınıf yokedilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnDestroy()
        {
            this.ActiveLevels.Clear();
            this.TargetWaterLevels.Clear();
            this.RemoveToActiveLevels.Clear();
        }
    }

    public class CellWaterLevelItem
    {
        /**
         *
         * Index nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ushort Index { get; set; }

        /**
         *
         * CurrentValue nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float CurrentValue { get; set; }

        /**
         *
         * TargetValue nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */

        public float TargetValue { get; set; }

        /**
         *
         * InterpolateValue nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float InterpolateValue { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CellWaterLevelItem()
        {
            this.Reset();
        }

        /**
         *
         * Sınıf başlatıldı mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsInitialized
        { 
            get 
            {
                return this.CurrentValue != -1f;
            }
        }

        /**
         *
         * Sınıfı başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Start(ushort index, float targetValue)
        {
            this.Index = index;
            this.TargetValue = targetValue;
        }

        /**
         *
         * CurrentValue değerini temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetCurrentValue(float currentValue)
        {
            this.CurrentValue = currentValue;
        }

        /**
         *
         * Su seviyesine enterpolasyon yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void InterpolateWater(float deltaTime)
        {
            this.InterpolateValue += deltaTime;
        }

        /**
         *
         * Tamamlandı mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsFinished()
        {
            return this.InterpolateValue >= 1f;
        }

        /**
         *
         * Mevcut değeri döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float GetValue()
        {
            return Mathf.Lerp(this.CurrentValue, this.TargetValue, this.InterpolateValue);
        }

        /**
         *
         * Verileri temizler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Reset()
        {
            this.Index            = 0;
            this.CurrentValue     = -1f;
            this.TargetValue      = 0f;
            this.InterpolateValue = 0f;
        }
    }
}


