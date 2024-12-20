namespace Subnautica.API.Features.Creatures.MonoBehaviours
{
    using Subnautica.API.Extensions;
    using Subnautica.Network.Structures;

    using System.Collections;

    using UnityEngine;

    public class CrashFishMonobehaviour : BaseMultiplayerCreature
    {
        /**
         *
         * Crash sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::Crash Crash { get; set; }

        /**
         *
         * CrashHome sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::CrashHome CrashHome { get; set; }

        /**
         *
         * Sınıf uyanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            this.Crash = this.GetComponent<global::Crash>();
        }

        /**
         *
         * Sınıf uyanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEnable()
        {
            this.Crash.OnState(Crash.State.Resting, true);
            this.Crash.GetAnimator().SetBool(AnimatorHashID.attacking, false);
            this.Crash.StopCalmDown();
            this.Crash.attackSound.Stop();
            this.Crash.inflateSound.Stop();

            this.CrashHome = null;

            this.FixedUpdate();
            this.Update();
        }


        /**
         *
         * Sınıf uyanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnDisable()
        {
            this.CrashHome = null;
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
            if (this.Crash.IsResting())
            {
                if (this.Crash.useRigidbody.isKinematic == false)
                {
                    this.Crash.useRigidbody.SetKinematic();
                }
            }

            if (this.CrashHome)
            {
                var isResting = this.Crash.IsResting();
                if (isResting != this.CrashHome.prevClosed)
                {
                    if (!isResting)
                    {
                        global::Utils.PlayFMODAsset(this.CrashHome.openSound, this.CrashHome.transform, 10f);
                    }

                    this.CrashHome.animator.SetBool(AnimatorHashID.attacking, !isResting);
                    this.CrashHome.prevClosed = isResting;
                }
            }
        }

        /**
         *
         * Her sabit karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void FixedUpdate()
        {
            if (this.CrashHome == null && this.Crash.IsResting())
            {
                this.RegisterCrashHome();
            }
        }

        /**
         *
         * CrashHome kaydını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void RegisterCrashHome()
        {
            var crashHome = this.FindCrashHome();
            if (crashHome)
            {
                this.CrashHome = crashHome;
                this.CrashHome.animator.SetBool(AnimatorHashID.attacking, false);

                this.transform.SetParent(this.CrashHome.transform, false);
                this.transform.localPosition = Vector3.zero;
                this.transform.localRotation = Quaternion.Euler(-90f, 0.0f, 0.0f);
                this.transform.SetParent(null);

                this.SpawnHomeItem();
            }
        }

        /**
         *
         * CrashHome için nesne yumurtlar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SpawnHomeItem()
        {
            this.StartCoroutine(this.SpawnItemAsync(this.GetRandomItem()));
        }

        /**
         *
         * ASYNC Nesne yumurtlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private IEnumerator SpawnItemAsync(TechType techType)
        {
            yield return new WaitForSecondsRealtime(1.5f);

            if (this.CrashHome && this.IsHomeItemExists() == false)
            {
                var request = CraftData.GetPrefabForTechTypeAsync(techType);

                yield return request;

                var prefab = request.GetResult();
                if (prefab)
                {
                    var gameObject = UnityEngine.Object.Instantiate(prefab, this.transform.position, Quaternion.identity);
                    gameObject.SetActive(true);

                    if (this.CrashHome)
                    {
                        gameObject.transform.SetParent(this.CrashHome.transform, false);
                        gameObject.transform.localPosition = Vector3.zero;
                        gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                        gameObject.transform.SetParent(null);
                    }
                }
            }
        }

        /**
         *
         * Rastgele nesne döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private TechType GetRandomItem()
        {
            if (Tools.GetRandomInt(0, 100) < 70)
            {
                return TechType.CrashPowder;
            }

            return TechType.CrashEgg;   
        }

        /**
         *
         * Ev nesnesi mevcut mu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsHomeItemExists()
        {
            int num = UWE.Utils.OverlapSphereIntoSharedBuffer(this.CrashHome.transform.position, 1.5f);
            
            for (int index = 0; index < num; ++index)
            {
                var gameObject = UWE.Utils.sharedColliderBuffer[index].gameObject;
                if (gameObject == null)
                {
                    continue;
                }

                var techType = CraftData.GetTechType(gameObject);
                if (techType == TechType.Sulphur || techType == TechType.CrashEgg || techType == TechType.CrashPowder)
                {
                    if (ZeroVector3.Distance(this.CrashHome.transform.position, gameObject.transform.position) <= 0.1f)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /**
         *
         * CrashHome bulur ve döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::CrashHome FindCrashHome()
        {
            int num = UWE.Utils.OverlapSphereIntoSharedBuffer(this.transform.position, 1.5f);

            for (int index = 0; index < num; ++index)
            {
                var crashHome = UWE.Utils.sharedColliderBuffer[index].GetComponentInParent<global::CrashHome>();
                if (crashHome)
                {
                    return crashHome;
                }
            }

            return null;
        }
    }
}
