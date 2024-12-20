namespace Subnautica.Events.Patches.Events.Game
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::SubNameInput), nameof(global::SubNameInput.OnDeselect))]
    public class SubNameInputDeselected
    {
        private static void Prefix(global::SubNameInput __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                var detail = SubnameInputDetail.GetInformation(__instance.gameObject);
                if (detail.TechType == TechType.None)
                {
                    return;
                }

                try
                {
                    SubNameInputDeselectedEventArgs args = new SubNameInputDeselectedEventArgs(detail.UniqueId, detail.TechType, __instance.inputField.text, __instance.colorData[0].image.color, __instance.colorData[1].image.color, __instance.colorData[2].image.color, __instance.colorData[3].image.color);

                    Handlers.Game.OnSubNameInputDeselected(args);
                }
                catch (Exception e)
                {
                    Log.Error($"SubNameInputDeselected.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}