using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrionShipMod.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] { ArtifactPool.Boss })]
    internal class OrionControlBay : Artifact
    {
        public int? cockpitIndex;
        public override void OnTurnStart(State state, Combat combat)
        {
            for (int i = 0; i < state.ship.parts.Count; i++)
            {
                Part part = state.ship.parts[i];
                if (part.type == PType.cockpit)
                {
                    part.type = PType.missiles;
                    cockpitIndex = i;
                    return;
                }
            }
        }

        public override void OnTurnEnd(State state, Combat combat) => RevertCockpit(state);
        public override void OnCombatEnd(State state) => RevertCockpit(state);

        public void RevertCockpit(State state)
        {
            if (cockpitIndex != null)
                state.ship.parts[cockpitIndex ?? 0].type = PType.cockpit;
        }
    }
}
