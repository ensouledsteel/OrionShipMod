using OrionShipMod.CardActions;
using System.Collections.Generic;

namespace OrionShipMod.Artifacts
{
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.EventOnly }, unremovable = true)]
    internal class OrionTactics: Artifact
    {
        private HashSet<Deck> playedDecks = new HashSet<Deck>();
        private bool leftActive = true;
        public virtual bool isValidDeck(State state, Deck deck)
        {
            // grab the list of current pilot decks
            List<Deck?> validDecks =
                state
                .characters
                .Select(character => character.deckType)
                .ToList();
            return validDecks.Contains(deck);
        }

        public override int? GetDisplayNumber(State s) => new int?(playedDecks.Count());
        public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
        {
            // Don't do anything if we've reached tactics threshold
            if (playedDecks.Count >= 3)
                return;

            // default squaddie toggle
            leftActive = !leftActive;
            ATogglePart toggleAction = new ATogglePart();
            toggleAction.partType = PType.cannon;
            toggleAction.timer = 0;

            combat.Queue(toggleAction);

            // if the most recent card was from a pilot deck, increase the counts
            if (!isValidDeck(state, deck))
                return;

            playedDecks.Add(deck);

            if (playedDecks.Count < 3)
                return;

            Pulse();
            combat.QueueImmediate(new OrionSquadToggle());
        }
        public override void OnPlayerTakeNormalDamage(State state, Combat combat)
        {
            var missingStatuses = state.characters.Select((Character c) => c.deckType switch
            {
                Deck.dizzy => Status.missingDizzy,
                Deck.peri => Status.missingPeri,
                Deck.riggs => Status.missingRiggs,
                Deck.goat => Status.missingIsaac,
                Deck.eunice => Status.missingDrake,
                Deck.hacker => Status.missingMax,
                Deck.shard => Status.missingBooks,
                Deck.colorless => Status.missingCat,
                _ => Status.energyLessNextTurn
            }).ToList();

            combat.QueueImmediate(new AStatus
            {
                status = missingStatuses[state.rngAi.NextInt() % missingStatuses.Count],
                statusAmount = 1,
                targetPlayer = true
            });
        }
        public override void OnTurnStart(State state, Combat combat) => Reset(state);
        public override void OnCombatEnd(State state) => Reset(state);

        private void Reset(State state)
        {
            bool toggled = leftActive;
            foreach (Part part in state.ship.parts)
                if (part.type == PType.cannon)
                {
                    part.active = toggled;
                    toggled = !toggled;
                }

            playedDecks = new HashSet<Deck> { };
        }
    }
}