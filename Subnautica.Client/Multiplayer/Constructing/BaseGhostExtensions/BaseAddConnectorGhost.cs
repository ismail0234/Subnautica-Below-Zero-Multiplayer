namespace Subnautica.Client.Multiplayer.Constructing.BaseGhostExtensions
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Construction;

    public static class BaseAddConnectorGhost
    {
        /**
         *
         * Yapının konumunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool UpdateMultiplayerPlacement(this global::BaseAddConnectorGhost baseGhost, bool updatePlacement, out bool positionFound, out bool geometryChanged, BaseAddConnectorGhostComponent component)
        {
            positionFound = false;
            geometryChanged = false;

            if (component == null || component.TargetBaseId.IsNull() || component.FaceCell == null)
            {
                baseGhost.targetBase = null;
                return false;
            }

            baseGhost.targetBase = Network.Identifier.GetComponentByGameObject<global::Base>(component.TargetBaseId);
            if (baseGhost.targetBase == null)
            {
                return false;
            }

            baseGhost.targetBase.SetPlacementGhost(baseGhost);
            baseGhost.targetOffset = component.FaceCell.ToInt3();

            var constructableBase = baseGhost.GetComponentInParent<ConstructableBase>();
            constructableBase.transform.position = baseGhost.targetBase.GridToWorld(baseGhost.targetOffset);
            constructableBase.transform.rotation = baseGhost.targetBase.transform.rotation;

            positionFound = true;
            return true;
        }
    }
}