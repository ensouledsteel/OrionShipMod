using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrionShipMod.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] { ArtifactPool.EventOnly })]
    internal class OrionControlBay : Artifact
    {
        public override void OnTurnStart(State state, Combat combat)
        {
            foreach (Part part in state.ship.parts)
            {
                if (part.skin == "@mod_part:EnsouledSteel.Orion.OrionShip.OrionLeader")
                    part.type = PType.missiles;
            }
        }

        public override void OnTurnEnd(State state, Combat combat) => MakeTheCockpitNormal(state);
        public override void OnCombatEnd(State state) => MakeTheCockpitNormal(state);

        public void MakeTheCockpitNormal(State state)
        {
            foreach (Part part in state.ship.parts)
            {
                if (part.skin == "@mod_part:EnsouledSteel.Orion.OrionShip.OrionLeader")
                    part.type = PType.cockpit;
            }
        }
    }
}
