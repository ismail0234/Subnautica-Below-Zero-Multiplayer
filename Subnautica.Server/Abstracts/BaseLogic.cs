namespace Subnautica.Server.Abstracts
{
    public abstract class BaseLogic
    {
        /**
         *
         * Sınıfı başlatır
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual void OnStart()
        {

        }

        /**
         *
         * Belirli aralıklarla tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual void OnUpdate(float deltaTime)
        {

        }

        /**
         *
         * Belirli aralıklarla tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual void OnAsyncUpdate()
        {

        }

        /**
         *
         * Belirli aralıklarla tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual void OnFixedUpdate(float fixedDeltaTime)
        {

        }

        /**
         *
         * Belirli aralıklarla tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public virtual void OnUnscaledFixedUpdate(float fixedDeltaTime)
        {

        }
    }
}
