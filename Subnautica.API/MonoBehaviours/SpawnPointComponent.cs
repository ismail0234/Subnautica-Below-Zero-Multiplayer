namespace Subnautica.API.MonoBehaviours
{
    using System.Collections;

    using Subnautica.API.Extensions;
    using Subnautica.Network.Models.WorldStreamer;

    using UnityEngine;

    public class SpawnPointComponent : MonoBehaviour
    {
        /**
         *
         * Coroutine nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Coroutine Coroutine;

        /**
         *
         * Slot'u barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroSpawnPoint SpawnPoint { get; set; }

        /**
         *
         * IsAutoRespawnRunning nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAutoRespawnRunning { get; set; }

        /**
         *
         * Aktif hale gelince tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetSpawnPoint(ZeroSpawnPoint spawnPoint)
        {
            this.SpawnPoint = spawnPoint;
            this.SpawnToggle();
        }

        /**
         *
         * Nesne doğduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Start()
        {

        }

        /**
         *
         * Aktif hale gelince tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEnable()
        {
            if (this.SpawnPoint != null)
            {
                this.DisableCoroutine();
                this.RegisterResourceTracker();
                this.DrillableHealthSync();
                this.HealthSync();

                if (!this.IsRespawnable())
                {
                    this.gameObject.SetActive(false);
                }
            }
        }

        /**
         *
         * Pasif hale gelince tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnDisable()
        {
            this.DisableCoroutine();
            this.UnRegisterResourceTracker();

            if (this.IsRespawnActive())
            {
                this.StartAutoRespawn();
            }
        }

        /**
         *
         * Nesne yokedilince tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnDestroy()
        {
            this.DisableCoroutine();
        }

        /**
         *
         * Nesneyi yumurtlar veya pasif hale getirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */ 
        public void SpawnToggle()
        {
            if (!this.IsRespawnActive())
            {
                this.gameObject.SetActive(false);
            }
            else if (!this.IsRespawnable())
            {
                this.gameObject.SetActive(false);
                this.StartAutoRespawn();
            }
            else
            {
                this.gameObject.SetActive(true);
            }

            this.DrillableHealthSync();
            this.HealthSync();
        }

        /**
         *
         * Otomatik respawnı başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void StartAutoRespawn()
        {
            this.DisableCoroutine();

            if (this.IsRespawnActive())
            {
                this.IsAutoRespawnRunning = true;
                this.Coroutine = UWE.CoroutineHost.StartCoroutine(this.AutoRespawnAsync());
            }
        }

        /**
         *
         * Otomatik canlandırma işlemini uygular.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public IEnumerator AutoRespawnAsync()
        {
            while (this != null && this.IsRespawnActive() && !this.IsRespawnable())
            {
                if (this.RespawnLeftTime() < 2f)
                {
                    yield return UWE.CoroutineUtils.waitForNextFrame;
                }
                else
                {
                    yield return new WaitForSecondsRealtime(1f);
                }
            }

            if (this != null)
            {
                this.IsAutoRespawnRunning = false;

                if (this.IsRespawnActive())
                {
                    this.gameObject?.SetActive(true);
                }
            }
        }

        /**
         *
         * Aktif coroutine nesnesini siler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void DisableCoroutine(bool forceDisable = false)
        {
            if (this.IsAutoRespawnRunning && this.Coroutine != null)
            {
                this.IsAutoRespawnRunning = false;

                UWE.CoroutineHost.StopCoroutine(this.Coroutine);
            }
        }

        /**
         *
         * Canlan durumunun aktif/pasif döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsRespawnActive()
        {
            if (this.gameObject == null || this.SpawnPoint == null || this.SpawnPoint.IsRespawnActive() == false)
            {
                return false;
            }

            var totalAmount = UWE.Utils.OverlapSphereIntoSharedBuffer(this.gameObject.transform.position, 1.5f);
            for (int index = 0; index < totalAmount; ++index)
            {
                var gameObject = UWE.Utils.sharedColliderBuffer[index].gameObject;
                if (gameObject == null)
                {
                    continue;
                }
                
                if (gameObject.GetComponentInParent<Base>())
                {
                    return false;
                }
            }

            return true;
        }

        /**
         *
         * Canlanıp canlanmayacağını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsRespawnable()
        {
            return this.SpawnPoint != null && this.SpawnPoint.IsRespawnable(DayNightCycle.main.timePassedAsFloat);
        }

        /**
         *
         * Kalan yeniden doğma zamanını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float RespawnLeftTime()
        {
            return this.SpawnPoint.NextRespawnTime - DayNightCycle.main.timePassedAsFloat;
        }

        /**
         *
         * Kaynak takibi için kaydeder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void RegisterResourceTracker()
        {
            if (this.gameObject.TryGetComponent<ResourceTracker>(out var resourceTracker))
            {
                resourceTracker.Register();
            }
        }

        /**
         *
         * Kaynak takibi için kaydı siler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void UnRegisterResourceTracker()
        {
            if (this.gameObject.TryGetComponent<ResourceTracker>(out var resourceTracker))
            {
                resourceTracker.Unregister();
            }
        }

        /**
         *
         * Nesnenin mevcut sağlığını max olarak ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void HealthSync()
        {
            if (this.gameObject.TryGetComponent<global::LiveMixin>(out var liveMixin))
            {
                liveMixin.ResetHealth();
            }
        }

        /**
         *
         * Delinebilen nesne sağlığını günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void DrillableHealthSync(bool isSpawnFx = false)
        {
            if (this.gameObject.TryGetComponent<global::Drillable>(out var drillable))
            {
                drillable.SetHealth(this.SpawnPoint.Health, this.IsRespawnable(), isSpawnFx);
            }
        }
    }
}
