namespace OrionShipMod.CardActions
{
    internal class AssaultFormation: CardAction
    {
        List<Part> empties = new List<Part>();
        List<Part> cannons = new List<Part>();
        public override void Begin(G g, State s, Combat c)
        {
            // thanks shuffling event for making this harder...
            // for the sake of "simplicity", assault formation will
            // move the cannons to the empty spaces nearest to the center
            var partsList = s.ship.parts;
            cannons = partsList
                .Where(part => part.type == PType.cannon)
                .ToList();
            empties = partsList
                .Where(part => part.type == PType.empty && part.skin == "@mod_part:EnsouledSteel.Orion.OrionShip.OrionEmpty")
                .ToList();

            for (int i = 0; i < cannons.Count; i++)
            {
                var cannon = cannons[i];
                var empty = empties[i];

                var emptySkin = empty.skin;
                empty.type = cannon.type;
                empty.active = cannon.active;
                empty.skin = cannon.skin;
                empty.damageModifier = cannon.damageModifier;

                cannon.type = PType.empty;
                cannon.active = true;
                cannon.skin = emptySkin;
                cannon.damageModifier = PDamMod.none;
            }
        }
    }
}
