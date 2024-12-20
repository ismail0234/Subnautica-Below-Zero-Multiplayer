namespace Subnautica.Client.Synchronizations.Processors.World
{
    using Subnautica.Events.EventArgs;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;

    public class EntitySpawnProcessor
    {
        /**
         *
         * Nesne spawn olurken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEntitySpawning(EntitySpawningEventArgs ev)
        {
            /*
                UWE.PrefabDatabase.TryGetPrefabFilename(ev.ClassId, out var filename);
                Log.Error("OnEntitySpawning Id: " + ev.UniqueId + ", Type: " + ev.TechType + ", ClassId: " + ev.ClassId + ", Level: " + ev.Level + ", IsRestricted: " + Network.StaticEntity.IsRestricted(ev.UniqueId) + ", Entity: " + filename);
            */

            if (Network.StaticEntity.IsRestricted(ev.UniqueId)) 
            {
                ev.IsAllowed = false;
            }
        }

        /**
         *
         * Nesne spawn olduktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEntitySpawned(EntitySpawnedEventArgs ev)
        {
          //  UWE.PrefabDatabase.TryGetPrefabFilename(ev.ClassId, out var filename);
           // Log.Error("OnEntitySpawned Id: " + ev.UniqueId + ", Type: " + ev.TechType + ", ClassId: " + ev.ClassId + ", Level: " + ev.Level + ", IsRestricted: " + Network.StaticEntity.IsRestricted(ev.UniqueId) + ", Entity: " + filename);
            if (ev.TechType != TechType.None && !Network.StaticEntity.IsRestricted(ev.UniqueId))
            {
              //  Log.Error("ENTITY ID: " + ev.UniqueId + ", TechType: " + ev.TechType);

                var entity = Network.StaticEntity.GetEntity(ev.UniqueId);
                if (entity != null)
                {
                    WorldEntityProcessor.ExecuteProcessor(entity, 0, true);
                }
            }
        }
    }
}