namespace Subnautica.Client.Multiplayer.Constructing.BaseGhostExtensions
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Construction;

    public static class BaseAddPartitionGhost
    {
        /**
         *
         * Yapının konumunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool UpdateMultiplayerPlacement(this global::BaseAddPartitionGhost baseGhost, bool updatePlacement, out bool positionFound, out bool geometryChanged, BaseAddPartitionGhostComponent component)
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

            geometryChanged = baseGhost.UpdateDirection(component.FaceStart.Direction);

            var faceCell    = component.FaceStart.Cell.ToInt3();
            var normCell    = baseGhost.targetBase.NormalizeCell(faceCell);
            var targetCell  = baseGhost.targetBase.GetCell(normCell);
            var cellSize    = Base.CellSize[(int)targetCell];
            var sourceRange = Int3.Bounds.Union(new Int3.Bounds(faceCell, faceCell), new Int3.Bounds(normCell, normCell + cellSize - 1));

            geometryChanged |= baseGhost.UpdateSize(sourceRange.size);

            var anchoredCell = faceCell - baseGhost.targetBase.GetAnchor();
            if (!baseGhost.anchoredCell.HasValue || baseGhost.anchoredCell.Value != anchoredCell)
            {
                baseGhost.anchoredCell = new Int3?(anchoredCell);
                baseGhost.ghostBase.CopyFrom(baseGhost.targetBase, sourceRange, sourceRange.mins * -1);
                baseGhost.ghostBase.ClearMasks();

                var face = new Base.Face(faceCell - normCell, baseGhost.partitionDirection);
                baseGhost.ghostBase.SetFaceMask(face, true);
                baseGhost.ghostBase.SetFaceType(face, Base.FaceType.Partition);
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