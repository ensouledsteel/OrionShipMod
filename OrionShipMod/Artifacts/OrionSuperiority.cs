using OrionShipMod.CardActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OrionShipMod.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] { ArtifactPool.Boss }, unremovable = true)]
    internal class OrionSuperiority: Artifact
    {
        private HashSet<Deck> playedDecks = new HashSet<Deck>();
        private bool leftActive = true;

        public override void OnReceiveArtifact(State s)
        {
            // Get rid of Tactics
            foreach (Artifact artifact in s.artifacts)
                if (artifact is OrionTactics)
                    artifact.OnRemoveArtifact(s);

            s.artifacts.RemoveAll((Artifact r) => r is OrionTactics);

            // TODO visual upgrade
        }

        public override int? GetDisplayNumber(State s) => new int?(playedDecks.Count());
        public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
        {
            //tbh this should be more DRY but this is more readable

            // Don't do anything if we've reached tactics threshold
            if (playedDecks.Count >= 3)
                return;

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
