namespace Subnautica.Client.Modules
{
    using System.Collections;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Extensions;

    using TMPro;

    using UnityEngine;

    using UWE;

    public class SleepScreen
    {   
        /**
         *
         * Singletion sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static SleepScreen Instance 
        { 
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SleepScreen();
                }

                return _Instance;
            }
        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Initialize()
        {
            if (this.SleepingTimeObject != null)
            {
                return false;
            }

            this.HandReticleParent = HandReticle.main.transform.parent;

            this.SleepingUserCountObject    = GameObject.Instantiate<GameObject>(ErrorMessage.main.prefabMessage);
            this.SleepingTimeObject         = GameObject.Instantiate<GameObject>(ErrorMessage.main.prefabMessage);

            this.SleepingUserCountComponent = SleepingUserCountObject.GetComponent<TextMeshProUGUI>();
            this.SleepingTimeComponent      = SleepingTimeObject.GetComponent<TextMeshProUGUI>();

            this.SleepingUserCountComponent.rectTransform.SetParent(ErrorMessage.main.messageCanvas, false);
            this.SleepingTimeComponent.rectTransform.SetParent(ErrorMessage.main.messageCanvas, false);

            this.SleepingUserCountComponent.rectTransform.localPosition = new Vector2(75f + ErrorMessage.main.messageCanvas.rect.x, 100f + ErrorMessage.main.messageCanvas.rect.y);
            this.SleepingUserCountComponent.fontSize = 26f;

            this.SleepingTimeComponent.rectTransform.localPosition = new Vector2(ErrorMessage.main.messageCanvas.rect.x + ErrorMessage.main.messageCanvas.rect.width - 200f, 100f + ErrorMessage.main.messageCanvas.rect.y);
            this.SleepingTimeComponent.fontSize = 26f;

            this.SleepingUserCountComponent.text = string.Empty;
            this.SleepingTimeComponent.text      = string.Empty;
            return true;
        }

        /**
         *
         * Uyuma tetiklemesini başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void StartSleeping()
        {
            this.IsSleepingStarted = true;
        }

        /**
         *
         * Yatak id değerini günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetUniqueId(string uniqueId)
        {
            this.UniqueId = uniqueId;
        }

        /**
         *
         * Yatak tarafını günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetBedSide(global::Bed.BedSide side)
        {
            this.BedSide = side;
        }

        /**
         *
         * Nesneleri pasif yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Disable()
        {
            this.IsEnabled = false;
            this.IsSleepingStarted = false;

            if (this.SleepingUserCountObject)
            {
                this.SleepingUserCountObject.SetActive(false);
            }

            if (this.SleepingTimeObject)
            {
                this.SleepingTimeObject.SetActive(false);
            }
        }

        /**
         *
         * Nesneleri aktif yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Enable()
        {
            this.IsEnabled = true;

            if (this.SleepingUserCountObject)
            {
                this.SleepingUserCountObject.SetActive(true);
            }

            if (this.SleepingTimeObject)
            {
                this.SleepingTimeObject.SetActive(true);
            }

            CoroutineHost.StartCoroutine(this.UpdateScreenAsync());
        }

        /**
         *
         * Değerleri sürekli günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public IEnumerator UpdateScreenAsync()
        {
            while (this.IsEnabled)
            {
                this.UpdateTime();
                this.UpdatePlayerCount();
                this.UpdateBedStandUp();

                yield return CoroutineUtils.waitForNextFrame;
            }
        }

        /**
         *
         * Oyuncu sayılarını günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void UpdatePlayerCount()
        {
            if (this.SleepingUserCountComponent)
            {
                this.SleepingUserCountComponent.text = string.Format("{0}/{1} Players Sleeping", Multiplayer.Furnitures.Bed.GetSleepingPlayerCount(), ZeroPlayer.GetAllPlayers().Count);
            }
        }

        /**
         *
         * Zamanı günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void UpdateTime()
        {
            if (this.SleepingTimeComponent)
            {
                var dayScalar  = DayNightCycle.main.GetDayScalar();
                var hourTime   = dayScalar * 24f;
                var minuteTime = (float)((double)hourTime % 1.0 * 60.0);

                string hourText, minuteText;
                if (hourTime < 10)
                {
                    hourText = string.Format("0{0}", Mathf.FloorToInt(hourTime));
                }
                else
                {
                    hourText = Mathf.FloorToInt(hourTime).ToString();
                }

                if (minuteTime < 10)
                {
                    minuteText = string.Format("0{0}", Mathf.FloorToInt(minuteTime));
                }
                else
                {
                    minuteText = Mathf.FloorToInt(minuteTime).ToString();
                }

                this.SleepingTimeComponent.text = string.Format("Time: {0}:{1}", hourText, minuteText);
            }
        }

        /**
         *
         * Yataktan kalk uyarısını ve işlevini çalıştırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void UpdateBedStandUp()
        {
            if (!DayNightCycle.main.IsInSkipTimeMode() && Multiplayer.Furnitures.Bed.GetSleepingPlayerCount() < ZeroPlayer.GetAllPlayers().Count)
            {
                this.UpdateHandReticleParent(true);

                HandReticle.main.SetText(HandReticle.TextType.Use, "StandUp", true, GameInput.Button.Exit);
                HandReticle.main.SetText(HandReticle.TextType.UseSubscript, string.Empty, true);

                if (GameInput.GetButtonDown(GameInput.Button.Exit))
                {
                    this.ExitInUseMode();
                }
            }
            else
            {
                this.UpdateHandReticleParent(false);
            }
        }

        /**
         *
         * Ebeveyn nesnesini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void UpdateHandReticleParent(bool status)
        {
            if (status)
            {
                if (ErrorMessage.main.messageCanvas.transform != HandReticle.main.transform.parent)
                {
                    HandReticle.main.transform.SetParent(ErrorMessage.main.messageCanvas.transform);
                }
            }
            else
            {
                if (this.HandReticleParent && this.HandReticleParent != HandReticle.main.transform.parent)
                {
                    HandReticle.main.transform.SetParent(this.HandReticleParent);
                }
            }
        }

        /**
         *
         * Yataktan kalkma işlevini başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void ExitInUseMode()
        {
            if (this.UniqueId.IsNotNull())
            {
                var bed = this.GetBed();
                if (bed)
                {
                    this.IsEnabled = false;

                    switch (this.BedSide)
                    {
                        case global::Bed.BedSide.Right:
                            bed.cinematicController = bed.rightLieDownCinematicController;
                            bed.currentStandUpCinematicController = bed.rightStandUpCinematicController;
                            bed.animator.transform.localPosition = bed.rightAnimPosition;
                            break;
                        default:
                            bed.cinematicController = bed.leftLieDownCinematicController;
                            bed.currentStandUpCinematicController = bed.leftStandUpCinematicController;
                            bed.animator.transform.localPosition = bed.leftAnimPosition;
                            break;
                    }

                    bed.ExitInUseMode(global::Player.main);
                }
            }
        }

        /**
         *
         * Yatağı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::Bed GetBed()
        {
            return Network.Identifier.GetComponentByGameObject<global::Bed>(this.UniqueId);
        }

        /**
         *
         * Tüm verileri temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Dispose()
        {
            this.IsEnabled = false;
            this.IsSleepingStarted = false;
            this.UniqueId = null;

            World.DestroyGameObject(this.SleepingUserCountObject);
            World.DestroyGameObject(this.SleepingTimeObject);
        }

        /**
         *
         * Zaman nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private GameObject SleepingTimeObject { get; set; }

        /**
         *
         * Kullanıcı Sayı nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private GameObject SleepingUserCountObject { get; set; }

        /**
         *
         * Zaman bileşenini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private TextMeshProUGUI SleepingUserCountComponent { get; set; }

        /**
         *
         * Kullanıcı Sayı bileşenini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private TextMeshProUGUI SleepingTimeComponent { get; set; }

        /**
         *
         * Yatak Id'yi barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * HandReticle Parent nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Transform HandReticleParent { get; set; }

        /**
         *
         * Yatak tarafını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::Bed.BedSide BedSide { get; set;}

        /**
         *
         * Singletion sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static SleepScreen _Instance;

        /**
         *
         * Şuan Aktif mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsEnabled { get; private set; }

        /**
         *
         * Uyuma tetiklendi mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsSleepingStarted { get; private set; }
    }
}