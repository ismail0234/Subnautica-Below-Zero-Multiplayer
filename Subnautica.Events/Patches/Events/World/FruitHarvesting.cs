namespace Subnautica.Events.Patches.Events.World
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::PickPrefab), nameof(global::PickPrefab.OnHandClick))]
    public static class FruitPlantHarvesting
    {
        private static bool Prefix(global::PickPrefab __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.gameObject.activeInHierarchy || !__instance.AllowedToPickUp() || __instance.isAddingToInventory)
            {
                return false;
            }

            var fruitPlant = __instance.GetComponentInParent<FruitPlant>();
            if (fruitPlant == null)
            {
                if (__instance.destroyOnPicked)
                {
                    var grownPlant = __instance.GetComponentInParent<GrownPlant>();
                    if (grownPlant)
                    {
                        try
                        {
                            GrownPlantHarvestingEventArgs args = new GrownPlantHarvestingEventArgs(Network.Identifier.GetIdentityId(grownPlant.seed.gameObject), grownPlant);

                            Handlers.World.OnGrownPlantHarvesting(args);

                            return args.IsAllowed;
                        }
                        catch (Exception e)
                        {
                            Log.Error($"GrownPlantHarvesting.Prefix(1): {e}\n{e.StackTrace}");
                        }
                    }
                }

                try
                {
                    FruitHarvestingEventArgs args = new FruitHarvestingEventArgs(__instance, Network.Identifier.GetIdentityId(__instance.gameObject, false), CraftData.GetTechType(__instance.gameObject), 1, 0);

                    Handlers.World.OnFruitHarvesting(args);

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"FruitPlantHarvesting.Prefix(2): {e}\n{e.StackTrace}");
                }

                return true;
            }

            try
            {
                FruitHarvestingEventArgs args = new FruitHarvestingEventArgs(__instance, Network.Identifier.GetIdentityId(fruitPlant.gameObject, false), CraftData.GetTechType(fruitPlant.gameObject), (byte)fruitPlant.fruits.Length, fruitPlant.fruitSpawnInterval);

                Handlers.World.OnFruitHarvesting(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"FruitPlantHarvesting.Prefix(3): {e}\n{e.StackTrace}");
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(global::GrownPlant), nameof(global::GrownPlant.OnHandClick))]
    public static class GrownPlantHarvesting
    {
        private static bool Prefix(global::GrownPlant __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.seed == null || __instance.seed.isSeedling || __instance.seed.pickupable == null || !global::Inventory.Get().HasRoomFor(__instance.seed.pickupable) || !__instance.seed.currentPlanter)
            {
                return false;
            }

            try
            {
                GrownPlantHarvestingEventArgs args = new GrownPlantHarvestingEventArgs(Network.Identifier.GetIdentityId(__instance.seed.gameObject), __instance);

                Handlers.World.OnGrownPlantHarvesting(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"GrownPlantHarvesting.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}