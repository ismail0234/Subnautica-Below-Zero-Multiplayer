namespace Subnautica.Client.Synchronizations.Processors.Inventory
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;

    using System.Collections;

    using UnityEngine;
    
    using UWE;

    using ServerModel = Subnautica.Network.Models.Server;


    public class EquipmentProcessor : NormalProcessor
    {
        /**
         *
         * Zamanlanmış veri gönderim durumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool IsSending { get; set; } = false;

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            return true;
        }

        /**
         *
         * Oyuncu bir eşyayı kuşandığında tetiklenir.
         * Oyuncu bir eşyayı üzerinden çıkardığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnProcessEquipment()
        {
            if (!IsSending && !EventBlocker.IsEventBlocked(ProcessType.InventoryEquipment))
            {
                UWE.CoroutineHost.StartCoroutine(SendServerData());
            }
        }

        /**
         *
         * Zamanlanmış veriyi sunucuya gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator SendServerData()
        {
            IsSending = true;

            yield return new WaitForSecondsRealtime(1f);

            IsSending = false;

            ServerModel.InventoryEquipmentArgs result = new ServerModel.InventoryEquipmentArgs()
            {
                Equipments     = GetEquipments(),
                EquipmentSlots = global::Inventory.main.equipment.SaveEquipment()
            };

            NetworkClient.SendPacket(result);
        }


        /**
         *
         * Ekipman verilerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static byte[] GetEquipments()
        {
            using (PooledObject<ProtobufSerializer> serializer = ProtobufSerializerPool.GetProxy())
            {
                return StorageHelper.Save(serializer, global::Inventory.main.equipmentRoot);
            }
        }
    }
}