namespace Subnautica.API.Features
{
    using FMOD.Studio;
    using FMODUnity;

    using Subnautica.Network.Structures;

    using System;
    using System.Collections;

    using UnityEngine;

    using UWE;

    public class FakeFMODByBenson : MonoBehaviour
    {
        /**
         *
         * Sınıf örneğini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static FakeFMODByBenson instance;

        /**
         *
         * Sınıf örneğini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static FakeFMODByBenson Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject().AddComponent<FakeFMODByBenson>();
                    instance.gameObject.SetActive(true);
                }

                return instance;
            }
        }

        /**
         *
         * Bir konumda ses çalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void PlaySound(FMODAsset asset, Transform attachedTransform, float maxDistance = 20f, Action<EventInstance> startAction = null, Func<EventInstance, Transform, bool> validAction = null)
        {
            this.StartCoroutine(this.PlaySoundAsync(asset, attachedTransform, maxDistance, startAction, validAction));
        }

        /**
         *
         * Bir konumda ses çalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void PlaySound(StudioEventEmitter eventEmitter, Transform attachedTransform, float maxDistance, Func<StudioEventEmitter, Transform, bool> validAction)
        {
            this.StartCoroutine(this.PlaySoundAsync(eventEmitter, attachedTransform, maxDistance, validAction));
        }

        /**
         *
         * Bir asenkron konumda ses çalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private IEnumerator PlaySoundAsync(FMODAsset asset, Transform attachedTransform, float maxDistance, Action<EventInstance> startAction, Func<EventInstance, Transform, bool> validAction)
        {
            if (attachedTransform && asset)
            {
                var eventInstance = FMODUWE.GetEvent(asset);
                startAction?.Invoke(eventInstance);

                eventInstance.start();
                eventInstance.release();

                while (attachedTransform && eventInstance.isValid() && eventInstance.getPlaybackState(out var state) == FMOD.RESULT.OK && (state == PLAYBACK_STATE.STARTING || state == PLAYBACK_STATE.PLAYING || state == PLAYBACK_STATE.SUSTAINING))
                {
                    if (validAction != null && !validAction.Invoke(eventInstance, attachedTransform))
                    {
                        break;
                    }

                    eventInstance.setVolume(this.GetVolume(attachedTransform, maxDistance));

                    yield return CoroutineUtils.waitForNextFrame;
                }

                eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }

        /**
         *
         * Bir asenkron konumda ses çalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private IEnumerator PlaySoundAsync(StudioEventEmitter eventEmitter, Transform attachedTransform, float maxDistance, Func<StudioEventEmitter, Transform, bool> validAction)
        {
            if (attachedTransform)
            {
                eventEmitter.Play();

                while (validAction.Invoke(eventEmitter, attachedTransform) && eventEmitter.EventInstance.getPlaybackState(out var state) == FMOD.RESULT.OK && (state == PLAYBACK_STATE.STARTING || state == PLAYBACK_STATE.PLAYING || state == PLAYBACK_STATE.SUSTAINING))
                {
                    eventEmitter.EventInstance.setVolume(this.GetVolume(attachedTransform, maxDistance));

                    yield return CoroutineUtils.waitForNextFrame;
                }

                eventEmitter.EventInstance.setVolume(1f);
                eventEmitter.Stop();
            }
        }

        /**
         *
         * Ses seviyesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float GetVolume(Transform attachedTransform, float maxDistance)
        {
            return Mathf.Max(1f - Mathf.Sqrt(ZeroVector3.Distance(attachedTransform.position, global::Player.main.transform.position)) / maxDistance, 0f);
        }
    }
}
