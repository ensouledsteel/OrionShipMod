using OrionShipMod.CardActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrionShipMod.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common)]
    internal class OrionFormation: Card
    {
        internal static Spr card_sprite = Spr.cards_Shield;

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
            retain = true,
            buoyant = true
        };
        public override string Name() => "Form Up!";
    }
}
