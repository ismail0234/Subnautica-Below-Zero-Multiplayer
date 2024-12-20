namespace Subnautica.Client.Multiplayer.Constructing.BaseGhostExtensions
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Construction;

    public static class BaseAddPartitionDoorGhost
    {
        /**
         *
         * Yapının konumunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool UpdateMultiplayerPlacement(this global::BaseAddPartitionDoorGhost baseGhost, bool updatePlacement, out bool positionFound, out bool geometryChanged, BaseAddPartitionDoorGhostComponent component)
        {
            positionFound = false;
            geometryChanged = false;

            if (updatePlacement == false || component == null || component.TargetBaseId.IsNull() || component.FaceStart.Cell == null)
            {
                baseGhost.targetBase = null;

                geometryChanged = baseGhost.SetupInvalid();
                return false;
            }

            baseGhost.targetBase = Network.Identifier.GetComponentByGameObject<global::Base>(component.TargetBaseId);
            if (baseGhost.targetBase == null)
            {
                geometryChanged = baseGhost.SetupInvalid();
                return false;
            }

            baseGhost.targetBase.SetPlacementGhost(baseGhost);

            var faceCell    = component.FaceStart.Cell.ToInt3();
            var normCell    = baseGhost.targetBase.NormalizeCell(faceCell);
            var targetCell  = baseGhost.targetBase.GetCell(normCell);
            var cellSize    = Base.CellSize[(int)targetCell];
            var sourceRange = Int3.Bounds.Union(new Int3.Bounds(faceCell, faceCell), new Int3.Bounds(normCell, normCell + cellSize - 1));

            geometryChanged = baseGhost.UpdateSize(sourceRange.size);

            var face1 = new Base.Face(faceCell - baseGhost.targetBase.GetAnchor(), component.FaceStart.Direction);
            if (!baseGhost.anchoredFace.HasValue || baseGhost.anchoredFace.Value != face1)
            {
                baseGhost.anchoredFace = new Base.Face?(face1);
                baseGhost.ghostBase.CopyFrom(baseGhost.targetBase, sourceRange, sourceRange.mins * -1);
                baseGhost.ghostBase.ClearMasks();

                var cell2 = faceCell - normCell;
                var face2 = new Base.Face(cell2, component.FaceStart.Direction);
                baseGhost.ghostBase.SetFaceType(face2, Base.FaceType.PartitionDoor);
                baseGhost.ghostBase.SetFaceMask(face2, true);

                foreach (var horizontalDirection in Base.HorizontalDirections)
                {
                    if (baseGhost.targetBase.GetFace(new Base.Face(faceCell, horizontalDirection)) == Base.FaceType.Partition)
                    {
                        baseGhost.ghostBase.SetFaceMask(new Base.Face(cell2, horizontalDirection), true);
                    }
                }

                baseGhost.RebuildGhostGeometry();

                geometryChanged = true;
            }

            var constructableBase = baseGhost.GetComponentInParent<ConstructableBase>();
            constructableBase.transform.position = baseGhost.targetBase.GridToWorld(normCell);
            constructableBase.transform.rotation = baseGhost.targetBase.transform.rotation;

            positionFound = true;
            return true;
        }
    }
}