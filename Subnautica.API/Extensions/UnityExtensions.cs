namespace Subnautica.API.Extensions
{
    using System;
    using System.Collections;

    using UnityEngine;
    using UnityEngine.Playables;

    using UWE;

    public static class UnityExtensions
    {
        /**
         *
         * Bir nesnenin başlatılmasını bekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */

        public static void WaitForInitialize(this GameObject gameObject, Func<GameObject, int, bool> checkAction, Action<GameObject> successAction)
        {
            CoroutineHost.StartCoroutine(WaitForInitializeAsync(gameObject, checkAction, successAction));
        }

        /**
         *
         * Director'u Çok oyunculu için çalıştırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsPlaying(this VFXController vFXController, int emitterId, bool checkEmission = false)
        {
            if (vFXController == null || emitterId >= vFXController.emitters.Length)
            {
                return false;
            }

            if (vFXController.emitters[emitterId].fxPS != null)
            {
                return vFXController.emitters[emitterId].fxPS.isPlaying || (checkEmission && vFXController.emitters[emitterId].fxPS.emission.enabled);
            }

            return false;
        }

        /**
         *
         * Hızı sıfırlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ResetForce(this Rigidbody rb)
        {
            rb.velocity        = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        /**
         *
         * Director'u Çok oyunculu için çalıştırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void MultiplayerPlay(this PlayableDirector director)
        {
            director.Stop();
            director.Play();
            director.playableGraph.GetRootPlayable(0).Play();
        }

        /**
         *
         * Çok oyunculu nesne alma sesi.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void MultiplayerPlayPickupSound(this Pickupable pickupable)
        {
            var pickupSound = global::Player.main.GetPickupSound(TechData.GetSoundType(pickupable.GetTechType()));
            if (pickupSound)
            {
                global::Utils.PlayFMODAsset(pickupSound, pickupable.transform.position, 5f);
            }
        }

        /**
         *
         * Bu oyundaki animasyonlar çok oyunculu için tasarlanmamış.
         * Bu yüzden bazı garip yöntemler kullanmaz isek animasyonlar düzgün çalışmıyor.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ImmediatelyPreparePlay(this Animator animator, string animParam, string prepareParam)
        {
            animator.speed = 50f;

            if (!string.IsNullOrEmpty(prepareParam))
            {
                animator.SetBool(prepareParam, true);
            }

            animator.SetBool(animParam, true);

            for (int i = 0; i < 10; i++)
            {
                animator.Update(Time.unscaledDeltaTime);
            }

            animator.SetBool(animParam, false);

            if (!string.IsNullOrEmpty(prepareParam))
            {
                animator.SetBool(prepareParam, false);
            }

            animator.speed = 1f;
        }

        /**
         *
         * Bu oyundaki animasyonlar çok oyunculu için tasarlanmamış.
         * Bu yüzden bazı garip yöntemler kullanmaz isek animasyonlar düzgün çalışmıyor.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ImmediatelyPlay(this Animator animator, string animParam, bool prepExists = false)
        {
            animator.speed = 50f;

            if (prepExists)
            {
                animator.SetBool(string.Format("{0}_prep", animParam), true);
            }

            animator.SetBool(animParam, true);

            for (int i = 0; i < 10; i++)
            {
                animator.Update(Time.unscaledDeltaTime);
            }

            animator.SetBool(animParam, false);

            if (prepExists)
            {
                animator.SetBool(string.Format("{0}_prep", animParam), false);
            }

            animator.speed = 1f;
        }

        /**
         *
         * Animasyonu en hızlı mod'da başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void FastPlay(this Animator animator, string animParam)
        {
            animator.speed = 50f;
            animator.Play(animParam);

            for (int i = 0; i < 10; i++)
            {
                animator.Update(Time.unscaledDeltaTime);
            }

            animator.speed = 1f;
        }

        /**
         *
         * Bir nesnenin başlatılmasını bekler (Async)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator WaitForInitializeAsync(GameObject gameObject, Func<GameObject, int, bool> checkAction, Action<GameObject> successAction)
        {
            int currentTick = 0;

            while (gameObject != null && !checkAction.Invoke(gameObject, currentTick++))
            {
                yield return new WaitForSecondsRealtime(0.1f);
            }

            if (gameObject)
            {
                successAction.Invoke(gameObject);
            }
        }

        /**
         *
         * Enterpolasyon durumunu değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SetInterpolation(this Rigidbody rigidbody, RigidbodyInterpolation interpolation)
        {
            if (rigidbody)
            {
                rigidbody.interpolation = interpolation;
            }
        }

        /**
         *
         * Nesneyi kinematic yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SetKinematic(this Rigidbody rigidbody, bool detectCollision = false)
        {
            if (rigidbody)
            {
                UWE.Utils.SetIsKinematicAndUpdateInterpolation(rigidbody, true, detectCollision);
            }
        }

        /**
         *
         * Nesneyi non kinematic yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SetNonKinematic(this Rigidbody rigidbody, bool detectCollision = false)
        {
            if (rigidbody)
            {
                UWE.Utils.SetIsKinematicAndUpdateInterpolation(rigidbody, false, detectCollision);
            }
        }

        /**
         *
         * Nesneyi yok eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Destroy(this GameObject gameObject)
        {
            UnityEngine.GameObject.Destroy(gameObject);
        }
    }
}