using OrionShipMod.CardActions;
using OrionShipMod.Cards;

namespace OrionShipMod.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] { ArtifactPool.Boss }, unremovable = true)]
    internal class OrionSuperiority: Artifact
    {
        public int playedCards = 0;
        public bool leftActive = true;

        public override void OnReceiveArtifact(State s)
        {
            // Get rid of Tactics
            foreach (Artifact artifact in s.artifacts)
                if (artifact is OrionTactics)
                    artifact.OnRemoveArtifact(s);

            s.artifacts.RemoveAll((Artifact r) => r is OrionTactics);
            // TODO visual upgrade
        }

        public override int? GetDisplayNumber(State s) => playedCards;
        public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
        {
            //tbh this should be more DRY but this is more readable
            // Don't do anything if we've reached superiority threshold
            if (playedCards >= 3)
                return;

            playedCards++;

            if (playedCards < 3)
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
        public override void OnCombatStart(State state, Combat combat)
        {
            combat.Queue(new AAddCard()
            {
                card = new OrionFormation() { upgrade = Upgrade.A },
                destination = CardDestination.Hand
            });
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

            playedCards = 0;
        }
        public override List<Tooltip>? GetExtraTooltips()
        {
            return new()
            {
                new TTCard { card = new OrionFormation { upgrade = Upgrade.A } }
            };
        }
    }
}
