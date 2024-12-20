namespace Subnautica.Client.MonoBehaviours.General
{
    using System.Collections.Generic;

    using Subnautica.Client.Abstracts;

    using UnityEngine;

    public class ProcessorBehaviour : MonoBehaviour
    {
        /**
          *
          * Update İşlemcilerini barındırır.
          *
          * @author Ismail <ismaiil_0234@hotmail.com>
          *
          */
        private List<BaseProcessor> UpdateProcessors { get; set; } = new List<BaseProcessor>();

        /**
          *
          * LateUpdate İşlemcilerini barındırır.
          *
          * @author Ismail <ismaiil_0234@hotmail.com>
          *
          */
        private List<BaseProcessor> LateUpdateProcessors { get; set; } = new List<BaseProcessor>();

        /**
          *
          * FixedUpdate İşlemcilerini barındırır.
          *
          * @author Ismail <ismaiil_0234@hotmail.com>
          *
          */
        private List<BaseProcessor> FixedUpdateProcessors { get; set; } = new List<BaseProcessor>();

        /**
          *
          * Dispose İşlemcilerini barındırır.
          *
          * @author Ismail <ismaiil_0234@hotmail.com>
          *
          */
        private List<BaseProcessor> DisposeProcessors { get; set; } = new List<BaseProcessor>();

        /**
         *
         * Her karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Update()
        {
            foreach (var processor in this.UpdateProcessors)
            {
                processor.OnUpdate();
            }
        }

        /**
         *
         * Her kare sonunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void LateUpdate()
        {
            foreach (var processor in this.LateUpdateProcessors)
            {
                processor.OnLateUpdate();
            }
        }

        /**
         *
         * Her Sabit karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void FixedUpdate()
        {
            foreach (var processor in this.FixedUpdateProcessors)
            {
                processor.OnFixedUpdate();
            }
        }

        /**
         *
         * Her Sabit karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnDestroy()
        {
            foreach (var processor in this.DisposeProcessors)
            {
                processor.OnDispose();
            }
        }

        /**
         *
         * Update İşlemcisi ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void AddUpdateProcessor(BaseProcessor processors)
        {
            this.UpdateProcessors.Add(processors);
        }

        /**
         *
         * LateUpdate İşlemcisi ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void AddLateUpdateProcessor(BaseProcessor processors)
        {
            this.LateUpdateProcessors.Add(processors);
        }

        /**
         *
         * FixedUpdate İşlemcisi ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void AddFixedUpdateProcessor(BaseProcessor processors)
        {
            this.FixedUpdateProcessors.Add(processors);
        }

        /**
         *
         * Dipose İşlemcisi ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void AddDisposeProcessor(BaseProcessor processors)
        {
            this.DisposeProcessors.Add(processors);
        }
    }
}
