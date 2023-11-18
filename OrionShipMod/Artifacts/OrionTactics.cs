using OrionShipMod.CardActions;
using OrionShipMod.Cards;

namespace OrionShipMod.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] { ArtifactPool.EventOnly }, unremovable = true)]
    internal class OrionTactics: Artifact
    {
        public bool isAssaultFormation = false;
        public HashSet<Deck> playedDecks = new HashSet<Deck>();
        public bool leftActive = true;
        private bool isValidDeck(State state, Deck deck)
        {
            // grab the list of current pilot decks
            List<Deck?> validDecks =
                state
                .characters
                .Select(character => character.deckType)
                .ToList();
            return validDecks.Contains(deck);
        }
        public void toggleAssaultFormation() => isAssaultFormation = !isAssaultFormation;
        public override int? GetDisplayNumber(State s) => new int?(playedDecks.Count());
        public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
        {
            // Be sure to mirror changes in Orion Superiority - kept not DRY for readability

            // Don't do anything if we've reached tactics threshold
            if (playedDecks.Count >= 3)
                return;

            // if the most recent card was from a pilot deck, increase the counts
            if (isValidDeck(state, deck))
                playedDecks.Add(deck);


            if (playedDecks.Count < 3)
            {
                // If we aren't ready to activate, toggle
                // active cannon after the current card
                leftActive = !leftActive;
                ATogglePart toggleAction = new ATogglePart();
                toggleAction.partType = PType.cannon;
                toggleAction.timer = 0;

                combat.Queue(toggleAction);
                return;
            }

            Pulse();
            combat.QueueImmediate(new OrionSquadToggle());
        }
        public override void OnPlayerTakeNormalDamage(State state, Combat combat)
        {
            // TODO handle modded characters better
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
        public override void OnCombatStart(State state, Combat combat)
        {
            combat.Queue(new AAddCard()
            {
                card = new OrionFormation(),
                destination = CardDestination.Hand
            });
        }
        public override void OnTurnStart(State state, Combat combat)
        {
            // We actually reverse Form Up! here. This is bad, but doing it
            // properly would take a status, and I wanna release.
            if (isAssaultFormation)
                combat.QueueImmediate(new AssaultFormation());
            Reset(state);
        }
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
        public override List<Tooltip>? GetExtraTooltips()
        {
            return new()
            {
                new TTCard { card = new OrionFormation { upgrade = Upgrade.None } }
            };
        }
    }
}