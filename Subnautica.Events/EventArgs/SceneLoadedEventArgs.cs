namespace Subnautica.Events.EventArgs
{
    using UnityEngine.SceneManagement;
    using System;

    public class SceneLoadedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SceneLoadedEventArgs(Scene scene)
        {
            Scene = scene;
        }

        /**
         *
         * Olayın çalıştırılıp/çalıştırılmayacağı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Scene Scene { get; set; }
    }
}
