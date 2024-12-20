namespace Subnautica.Events.Patches.Events.Player
{
    using System;
    using System.Collections.Generic;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    [HarmonyPatch(typeof(global::ArmsController), nameof(global::ArmsController.Update))]
    public static class Updated
    {
        /**
         *
         * StopwatchItem nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static readonly StopwatchItem StopwatchItem = new StopwatchItem(BroadcastInterval.PlayerUpdated);

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Postfix(global::ArmsController __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                if (StopwatchItem.IsFinished())
                {
                    StopwatchItem.Restart();

                    try
                    {
                        PlayerUpdatedEventArgs args = new PlayerUpdatedEventArgs(
                            __instance.transform.position, 
                            global::Player.main.transform.localPosition, 
                            MainCameraControl.main.viewModel.transform.rotation, 
                            GetTechTypeInHand(__instance), 
                            GetPlayerEquipments(), 
                            MainCameraControl.main.GetCameraPitch(), 
                            MainCameraControl.main.transform.forward, 
                            __instance.animator.GetFloat("FP_Emotes"),
                            global::Player.main.precursorArmsAttached,
                            global::Player.main.footStepSounds.currentSurfaceType
                        );

                        Handlers.Player.OnUpdated(args);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Updated.Postfix: {e}\n{e.StackTrace}");
                    }
                }
            }
        }

        /**
         *
         * Oyuncu elindeki eşyayı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static TechType GetTechTypeInHand(global::ArmsController __instance)
        {
            var hand = TechType.None;
            var tool = __instance.guiHand.GetTool();
            if (tool == null)
            {
                if (global::Player.main.pda.isInUse)
                {
                    hand = TechType.PDA;
                }
            }
            else if (tool.pickupable)
            {
                hand = tool.pickupable.GetTechType();
            }
            else
            {
                var pickupable = tool.GetComponent<Pickupable>();
                if (pickupable)
                {
                    hand = pickupable.GetTechType();
                }
            }

            if (hand == TechType.Hoverbike)
            {
                return TechType.SpyPenguinRemote;
            }

            if (hand.IsPoster())
            {
                return TechType.None;
            }

            return hand;
        }

        /**
         *
         * Oyuncu ekipmanlarını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static List<TechType> GetPlayerEquipments()
        {
            List<TechType> equipments = new List<TechType>()
            {
                GetEquipmentType("Head"),
                GetEquipmentType("Body"),
                GetEquipmentType("Gloves"),
                GetEquipmentType("Foots"),
                // GetEquipmentType("Tank"),
            };  

            return equipments;
        }

        /**
         *
         * Oyuncu ekipman türünü döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static TechType GetEquipmentType(string slot)
        {
            if (global::Inventory.main.equipment.equipment.TryGetValue(slot, out var inventoryItem) && inventoryItem != null)
            {
                return inventoryItem.techType;
            }

            return TechType.None;
        }
    }
}