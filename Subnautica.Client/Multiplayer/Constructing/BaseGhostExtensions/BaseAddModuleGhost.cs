namespace Subnautica.Client.Multiplayer.Constructing.BaseGhostExtensions
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Construction;

    public static class BaseAddModuleGhost
    {
        /**
         *
         * Yapının konumunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool UpdateMultiplayerPlacement(this global::BaseAddModuleGhost baseGhost, bool updatePlacement, out bool positionFound, out bool geometryChanged, BaseAddModuleGhostComponent component)
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

            var faceCell = component.FaceStart.Cell.ToInt3();
            var normCell = baseGhost.targetBase.NormalizeCell(faceCell);
            var face2    = new Base.Face(faceCell - baseGhost.targetBase.GetAnchor(), component.FaceStart.Direction);

            if (!baseGhost.anchoredFace.HasValue || baseGhost.anchoredFace.Value != face2)
            {
                baseGhost.anchoredFace = new Base.Face?(face2);

                var targetCell  = baseGhost.targetBase.GetCell(normCell);
                var cellSize    = Base.CellSize[(int) targetCell];
                geometryChanged = baseGhost.UpdateSize(cellSize);

                baseGhost.ghostBase.CopyFrom(baseGhost.targetBase, new Int3.Bounds(normCell, normCell + cellSize - 1), normCell * -1);

                var face3 = new Base.Face(faceCell - normCell, component.FaceStart.Direction);
                baseGhost.ghostBase.SetFaceType(face3, baseGhost.faceType);
                baseGhost.ghostBase.ClearMasks();
                baseGhost.ghostBase.SetFaceMask(face3, true);
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