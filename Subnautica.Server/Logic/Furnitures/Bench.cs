namespace Subnautica.Server.Logic.Furnitures
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts;
    using Subnautica.Server.Core;

    using Metadata = Subnautica.Network.Models.Metadata;

    public class Bench : BaseLogic
    {
        /**
         *
         * Sınıfı başlatır
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnStart()
        {
            foreach (var construction in this.GetChairs())
            {
                var bench = construction.Value.EnsureComponent<Metadata.Bench>();
                if (bench.IsSitdown)
                {
                    bench.Standup();
                }
            }
        }

        /**
         *
         * Sandalyeleri döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<KeyValuePair<string, ConstructionItem>> GetChairs()
        {
            return Server.Instance.Storages.Construction.Storage.Constructions.Where(q => q.Value.ConstructedAmount == 1f && API.Features.TechGroup.Chairs.Contains(q.Value.TechType)).ToList();
        }
    }
}
