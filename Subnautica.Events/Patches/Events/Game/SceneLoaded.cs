namespace Subnautica.Events.Patches.Events.Game
{
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    
    using System;

    using UnityEngine.SceneManagement;

    public class SceneLoaded
    {
        public static void Run(Scene scene, LoadSceneMode loadMode)
        {
            try
            {
                SceneLoadedEventArgs args = new SceneLoadedEventArgs(scene);

                Handlers.Game.OnSceneLoaded(args);
            }
            catch (Exception e)
            {
                Log.Error($"SceneLoaded.Run: {e}\n{e.StackTrace}");
            }
        }
    }
}
