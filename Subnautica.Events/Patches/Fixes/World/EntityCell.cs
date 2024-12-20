namespace Subnautica.Events.Patches.Fixes.World
{
    using System.Collections;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Client.Extensions;

    [HarmonyPatch(typeof(global::EntityCell), nameof(global::EntityCell.AwakeAsync))]
    public class EntityCell
    {
        private static IEnumerator Postfix(IEnumerator values, ProtobufSerializer serializer)
        {
            if (Network.IsMultiplayerActive)
            {
                serializer.SetCellModeActive(true);
            }

            yield return values;

            if (Network.IsMultiplayerActive)
            {
                serializer.SetCellModeActive(false);
            }
        }
    }
}
