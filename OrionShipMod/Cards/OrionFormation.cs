using Orion;
using OrionShipMod.CardActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrionShipMod.Cards
{
    [CardMeta(rarity = Rarity.common)]
    internal class OrionFormation: Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            return new List<CardAction>()
            {
                new AssaultFormation()
            };
        }
        public override CardData GetData(State state) => new CardData
        {
            cost = 1,
            art = Spr.cards_Ace,
            description = Loc.GetLocString(OrionShipManifest.OrionFormation?.DescLocKey ?? throw new Exception("Card not found."))
        };
        public override string Name() => "Form Up!";
    }
}
