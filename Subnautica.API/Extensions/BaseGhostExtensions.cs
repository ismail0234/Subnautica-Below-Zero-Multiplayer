namespace Subnautica.API.Extensions
{
    using Subnautica.Network.Models.Construction.Shared;
    using Subnautica.Network.Models.Construction;
    using Subnautica.API.Features;

    using UnityEngine;

    public static class BaseGhostExtensions
    {
        /**
         *
         * Yapı hayalet bileşenini bulur ve döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static BaseGhostComponent GetBaseGhostComponent(this GameObject gameObject)
        {
            if (gameObject.TryGetComponent<global::BaseAddFaceGhost>(out var faceGhost))
            {
                return faceGhost.GetGhostComponent();
            }

            if (gameObject.TryGetComponent<global::BaseAddLadderGhost>(out var ladderGhost))
            {
                return ladderGhost.GetGhostComponent();
            }

            if (gameObject.TryGetComponent<global::BaseAddBulkheadGhost>(out var bulkheadGhost))
            {
                return bulkheadGhost.GetGhostComponent();
            }

            if (gameObject.TryGetComponent<global::BaseAddPartitionGhost>(out var partitionGhost))
            {
                return partitionGhost.GetGhostComponent();
            }

            if (gameObject.TryGetComponent<global::BaseAddPartitionDoorGhost>(out var partitionDoorGhost))
            {
                return partitionDoorGhost.GetGhostComponent();
            }

            if (gameObject.TryGetComponent<global::BaseAddModuleGhost>(out var moduleGhost))
            {
                return moduleGhost.GetGhostComponent();
            }

            if (gameObject.TryGetComponent<global::BaseAddCellGhost>(out var cellGhost))
            {
                return cellGhost.GetGhostComponent();
            }

            if (gameObject.TryGetComponent<global::BaseAddCorridorGhost>(out var corridorGhost))
            {
                return corridorGhost.GetGhostComponent();
            }

            if (gameObject.TryGetComponent<global::BaseAddConnectorGhost>(out var connectorGhost))
            {
                return connectorGhost.GetGhostComponent();
            }

            if (gameObject.TryGetComponent<global::BaseAddMapRoomGhost>(out var mapRoomGhost))
            {
                return mapRoomGhost.GetGhostComponent();
            }

            if (gameObject.TryGetComponent<global::BaseAddWaterPark>(out var waterParkGhost))
            {
                return waterParkGhost.GetGhostComponent();
            }

            return null;
        }

        /**
         *
         * Yapı hayalet bileşeni döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static BaseGhostComponent GetGhostComponent(this global::BaseAddFaceGhost baseGhost)
        {
            if (baseGhost.targetBase == null)
            {
                return null;
            }

            var face = Network.Temporary.GetProperty<global::Base.Face>(baseGhost.targetBase.gameObject.GetIdentityId(), "FaceStart");

            return new BaseAddFaceGhostComponent()
            {
                TargetBaseId = baseGhost.targetBase.gameObject.GetIdentityId(),
                Face = new BaseFaceComponent()
                {
                    Cell      = face.cell.ToZeroInt3(),
                    Direction = face.direction
                },
            };
        }

        /**
         *
         * Yapı hayalet bileşeni döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static BaseGhostComponent GetGhostComponent(this global::BaseAddLadderGhost baseGhost)
        {
            if (baseGhost.targetBase == null)
            {
                return null;
            }

            var faceStart = Network.Temporary.GetProperty<global::Base.Face>(baseGhost.targetBase.gameObject.GetIdentityId(), "FaceStart");
            var faceEnd   = Network.Temporary.GetProperty<global::Base.Face>(baseGhost.targetBase.gameObject.GetIdentityId(), "FaceEnd");

            return new BaseAddLadderGhostComponent()
            {
                TargetBaseId = baseGhost.targetBase.gameObject.GetIdentityId(),
                FaceStart = new BaseFaceComponent()
                {
                    Cell      = faceStart.cell.ToZeroInt3(),
                    Direction = faceStart.direction,
                },
                FaceEnd = new BaseFaceComponent()
                {
                    Cell      = faceEnd.cell.ToZeroInt3(),
                    Direction = faceEnd.direction,
                },
            };
        }

        /**
         *
         * Yapı hayalet bileşeni döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static BaseGhostComponent GetGhostComponent(this global::BaseAddBulkheadGhost baseGhost)
        {
            if (baseGhost.targetBase == null)
            {
                return null;
            }

            var face = Network.Temporary.GetProperty<global::Base.Face>(baseGhost.targetBase.gameObject.GetIdentityId(), "FaceStart");

            return new BaseAddBulkheadGhostComponent()
            {
                TargetBaseId = baseGhost.targetBase.gameObject.GetIdentityId(),
                FaceStart    = new BaseFaceComponent()
                {
                    Cell      = face.cell.ToZeroInt3(),
                    Direction = face.direction
                },
            };
        }

        /**
         *
         * Yapı hayalet bileşeni döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static BaseGhostComponent GetGhostComponent(this global::BaseAddPartitionGhost baseGhost)
        {
            if (baseGhost.targetBase == null)
            {
                return null;
            }

            var faceCell  = Network.Temporary.GetProperty<Int3>(baseGhost.targetBase.gameObject.GetIdentityId(), "FaceCell");
            var direction = Network.Temporary.GetProperty<global::Base.Direction>(baseGhost.targetBase.gameObject.GetIdentityId(), "Direction");

            return new BaseAddPartitionGhostComponent()
            {
                TargetBaseId = baseGhost.targetBase.gameObject.GetIdentityId(),
                FaceStart    = new BaseFaceComponent()
                {
                    Cell      = faceCell.ToZeroInt3(),
                    Direction = direction
                },
            };
        }

        /**
         *
         * Yapı hayalet bileşeni döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static BaseGhostComponent GetGhostComponent(this global::BaseAddPartitionDoorGhost baseGhost)
        {
            if (baseGhost.targetBase == null)
            {
                return null;
            }

            var faceCell  = Network.Temporary.GetProperty<Int3>(baseGhost.targetBase.gameObject.GetIdentityId(), "FaceCell");
            var direction = Network.Temporary.GetProperty<global::Base.Direction>(baseGhost.targetBase.gameObject.GetIdentityId(), "Direction");

            return new BaseAddPartitionDoorGhostComponent()
            {
                TargetBaseId = baseGhost.targetBase.gameObject.GetIdentityId(),
                FaceStart    = new BaseFaceComponent()
                {
                    Cell      = faceCell.ToZeroInt3(),
                    Direction = direction
                },
            };
        }

        /**
         *
         * Yapı hayalet bileşeni döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static BaseGhostComponent GetGhostComponent(this global::BaseAddModuleGhost baseGhost)
        {
            if (baseGhost.targetBase == null)
            {
                return null;
            }

            var face = Network.Temporary.GetProperty<global::Base.Face>(baseGhost.targetBase.gameObject.GetIdentityId(), "FaceStart");

            return new BaseAddModuleGhostComponent()
            {
                TargetBaseId = baseGhost.targetBase.gameObject.GetIdentityId(),
                FaceStart    = new BaseFaceComponent()
                {
                    Cell      = face.cell.ToZeroInt3(),
                    Direction = face.direction
                },
            };
        }

        /**
         *
         * Yapı hayalet bileşeni döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static BaseGhostComponent GetGhostComponent(this global::BaseAddCellGhost baseGhost)
        {
            var targetOffset  = Network.Temporary.GetProperty<Int3>(baseGhost.gameObject.GetIdentityId(), "TargetOffset");

            return new BaseAddCellGhostComponent()
            {
                TargetBaseId  = baseGhost.targetBase?.gameObject.GetIdentityId(),
                TargetOffset  = targetOffset.ToZeroInt3(),
                AboveFaceType = baseGhost.ghostBase.GetFace(new global::Base.Face(Int3.zero, global::Base.Direction.Above)),
                BelowFaceType = baseGhost.ghostBase.GetFace(new global::Base.Face(Int3.zero, global::Base.Direction.Below)),
            };
        }

        /**
         *
         * Yapı hayalet bileşeni döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static BaseGhostComponent GetGhostComponent(this global::BaseAddCorridorGhost baseGhost)
        {
            var targetOffset = Network.Temporary.GetProperty<Int3>(baseGhost.gameObject.GetIdentityId(), "TargetOffset");

            return new BaseAddCorridorGhostComponent()
            {
                TargetBaseId = baseGhost.targetBase?.gameObject.GetIdentityId(),
                TargetOffset = targetOffset.ToZeroInt3(),
            };
        }

        /**
         *
         * Yapı hayalet bileşeni döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static BaseGhostComponent GetGhostComponent(this global::BaseAddMapRoomGhost baseGhost)
        {
            var targetOffset = Network.Temporary.GetProperty<Int3>(baseGhost.gameObject.GetIdentityId(), "TargetOffset");

            return new BaseAddMapRoomGhostComponent()
            {
                TargetBaseId = baseGhost.targetBase?.gameObject.GetIdentityId(),
                TargetOffset = targetOffset.ToZeroInt3(),
            };
        }

        /**
         *
         * Yapı hayalet bileşeni döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static BaseGhostComponent GetGhostComponent(this global::BaseAddConnectorGhost baseGhost)
        {
            if (baseGhost.targetBase == null)
            {
                return null;
            }

            var faceCell = Network.Temporary.GetProperty<Int3>(baseGhost.targetBase.gameObject.GetIdentityId(), "Cell");

            return new BaseAddConnectorGhostComponent()
            {
                TargetBaseId = baseGhost.targetBase.gameObject.GetIdentityId(),
                FaceCell     = faceCell.ToZeroInt3(),
            };
        }

        /**
         *
         * Yapı hayalet bileşeni döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static BaseGhostComponent GetGhostComponent(this global::BaseAddWaterPark baseGhost)
        {
            if (baseGhost.targetBase == null)
            {
                return null;
            }

            var faceCell  = Network.Temporary.GetProperty<Int3>(baseGhost.targetBase.gameObject.GetIdentityId(), "FaceCell");
            var direction = Network.Temporary.GetProperty<global::Base.Direction>(baseGhost.targetBase.gameObject.GetIdentityId(), "Direction");

            return new BaseAddWaterParkGhostComponent()
            {
                TargetBaseId = baseGhost.targetBase.gameObject.GetIdentityId(),
                FaceStart    = new BaseFaceComponent()
                {
                    Cell      = faceCell.ToZeroInt3(),
                    Direction = direction
                },
            };
        }
    }
}


