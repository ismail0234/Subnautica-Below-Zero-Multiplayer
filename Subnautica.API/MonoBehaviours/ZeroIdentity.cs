namespace Subnautica.API.MonoBehaviours
{
    using UnityEngine;

    public class ZeroIdentity : UniqueIdentifier
    {
        public override bool ShouldCreateEmptyObject()
        {
            return false;
        }

        public override bool ShouldMergeObject()
        {
            return false;
        }

        public override bool ShouldOverridePrefab()
        {
            return false;
        }

        public override bool ShouldSerialize(Component comp)
        {
            return true;
        }

        public override bool ShouldStoreClassId()
        {
            return false;
        }

        public void Awake()
        {
            if (string.IsNullOrEmpty(this.Id))
            {
                this.Id = "";    
            }
        }
    }
}