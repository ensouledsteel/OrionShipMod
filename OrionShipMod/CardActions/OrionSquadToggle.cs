using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrionShipMod.CardActions
{
    internal class OrionSquadToggle: CardAction
    {
        public override void Begin(G g, State state, Combat c)
        {
            foreach (Part part in state.ship.parts)
                if (part.type == PType.cannon)
                    part.active = true;
        }
    }
}
