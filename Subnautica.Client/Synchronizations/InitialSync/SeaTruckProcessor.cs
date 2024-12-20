namespace Subnautica.Client.Synchronizations.InitialSync
{
    using System.Linq;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.MonoBehaviours.World;

    using UnityEngine;

    public class SeaTruckProcessor
    {
        /**
         *
         * SeaTruck Bağlantı kontrol sayısı.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static int ConnectionCheckCount { get; set; } = 2 + 1;

        /**
         *
         * SeaTruck bağlantılarını ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnConnectionIntialized()
        {
            if (Network.Session.Current.SeaTruckConnections != null)
            {
                foreach (var connection in Network.Session.Current.SeaTruckConnections.ToList())
                {
                    var frontModule = Network.Identifier.GetComponentByGameObject<global::SeaTruckSegment>(connection.Key);
                    var backModule  = Network.Identifier.GetComponentByGameObject<global::SeaTruckSegment>(connection.Value);
                    if (frontModule && backModule)
                    {
                        using (EventBlocker.Create(ProcessType.SeaTruckConnection))
                        {
                            frontModule.frontConnection.SetConnectedTo(backModule.rearConnection);

                            if (backModule.isMainCab)
                            {
                                backModule.OnConnectionChanged();
                            }
                        }
                    }
                }

                for (int i = 0; i < ConnectionCheckCount; i++)
                {
                    foreach (var connection in Network.Session.Current.SeaTruckConnections)
                    {
                        var backModule = Network.Identifier.GetComponentByGameObject<global::SeaTruckSegment>(connection.Key);
                        if (backModule)
                        {
                            var targetPosition = (backModule.frontConnection.GetConnection().connectionPoint.position - backModule.frontConnection.connectionPoint.position + backModule.transform.position);
                            if ((targetPosition - backModule.transform.position).sqrMagnitude > 0.01f)
                            {
                                backModule.transform.position = targetPosition;
                                backModule.transform.rotation = backModule.frontConnection.GetConnection().transform.rotation;
                            }

                            if (Vector3.Dot(backModule.transform.forward, backModule.frontConnection.GetConnection().transform.forward) > 0.899999976158142f && (backModule.frontConnection.GetConnection().connectionPoint.position - backModule.frontConnection.connectionPoint.position).magnitude < 0.00999999977648258f)
                            {
                                backModule.updateDockedPosition = false;
                            }
                        }
                    }
                }
            }
        }

        /**
         *
         * Moonpool Expansion Kuyruk bağlantılarını ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnMoonpoolExpansionTailsInitialized()
        {
            if (Network.Session.Current.Constructions != null)
            {
                foreach (var construction in Network.Session.Current.Constructions.Where(q => q.TechType == TechType.BaseMoonpoolExpansion && q.ConstructedAmount == 1f))
                {
                    var dockingBay = Network.Identifier.GetComponentByGameObject<global::VehicleDockingBay>(construction.UniqueId);
                    if (dockingBay == null)
                    {
                        continue;
                    }

                    var vehicleDockingBay = dockingBay.gameObject.EnsureComponent<MultiplayerVehicleDockingBay>();
                    if (vehicleDockingBay == null)
                    {
                        continue;
                    }

                    vehicleDockingBay.ExpansionManager.OnHandleLoading(true);
                }
            }
        }
    }
}