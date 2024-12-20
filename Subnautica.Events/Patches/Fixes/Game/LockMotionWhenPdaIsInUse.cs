namespace Subnautica.Events.Patches.Fixes.Game
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(ArmsController), nameof(ArmsController.SetUsingPda))]
    public class LockMotionWhenPdaIsInUse_Arms
    {
        private static void Prefix(bool isUsing)
        {
            if (Network.IsMultiplayerActive && World.IsLoaded && isUsing)
            {
                FPSInputModule.current.lockMovement = true;
            }

        }
    }

    [HarmonyPatch(typeof(PDA), nameof(PDA.Deactivated))]
    public class LockMotionWhenPdaIsInUse_PDA
    {
        private static void Prefix()
        {
            if (Network.IsMultiplayerActive && World.IsLoaded)
            {
                FPSInputModule.current.lockMovement = false;
            }
        }
    }

    [HarmonyPatch(typeof(uGUI_InputGroup), nameof(uGUI_InputGroup.LockMovement))]
    public class LockMotionWhenPdaIsInUse_Input
    {
        private static bool Prefix(bool state)
        {
            if (!Network.IsMultiplayerActive || !World.IsLoaded)
            {
                return true;
                
            }

            if (global::Player.main.pda.isInUse)
            {
                FPSInputModule.current.lockMovement = true;
            }
            else
            {
                FPSInputModule.current.lockMovement = state;
            }
            
            return false;
        }
    }
}