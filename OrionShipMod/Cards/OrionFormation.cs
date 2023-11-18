using Orion;
using OrionShipMod.CardActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrionShipMod.Cards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A })]
    internal class OrionFormation: Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            return new List<CardAction>()
            {
                new AssaultFormation()
            };
        }
        public override CardData GetData(State state)
        {
            string description = Loc.GetLocString(OrionShipManifest.OrionFormation?.DescLocKey ?? throw new Exception("Card not found."));
            if (upgrade == Upgrade.A)
                description = Loc.GetLocString(OrionShipManifest.OrionFormation?.DescALocKey ?? throw new Exception("Card not found."));

            return new CardData
            {
                cost = 1,
                retain = true,
                recycle = true,
                temporary = true,
                description = description
            };
        }
    }
}
