namespace Subnautica.Client.Abstracts
{
    public class BaseProcessor
    {
        /**
         *
         * Sınıf başlatılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual void OnStart()
        {

        }

        /**
         *
         * Her karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual void OnUpdate()
        {

        }

        /**
         *
         * Her kare sonunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual void OnLateUpdate()
        {

        }

        /**
         *
         * Her Sabit karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual void OnFixedUpdate()
        {

        }

        /**
         *
         * Ana menüye dönünce tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual void OnDispose()
        {

        }

        /**
         *
         * İşlem tamamlanma durumunu değiştirir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnFinishedSuccessCallback()
        {
            this.SetFinished(true);
        }

        /**
         *
         * İşlem tamamlanma durumunu değiştirir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetFinished(bool isFinished)
        {
            this.isFinished = isFinished;
        }

        /**
         *
         * İşlem tamamlandı mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsFinished()
        {
            return this.isFinished;
        }

        /**
         *
         * Sonraki kare beklenme durumunu değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetWaitingForNextFrame(bool isWaitingForNextFrame)
        {
            this.isWaitingForNextFrame = isWaitingForNextFrame;
        }

        /**
         *
         * Sonraki kare bekleniyor mu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsWaitingForNextFrame()
        {
            return this.isWaitingForNextFrame;
        }

        /**
         *
         * İşlem tamamlanma durumunu barındırır
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool isFinished { get; set; } = false;

        /**
         *
         * Sonraki kare beklensin mi? (Sadece asenkron işlemler için)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool isWaitingForNextFrame { get; set; } = false;
    }
}
