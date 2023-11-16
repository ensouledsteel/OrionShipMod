using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OrionShipMod.Artifacts
{
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss }, unremovable = true)]
    internal class OrionSuperiority: OrionTactics
    {
        public override void OnReceiveArtifact(State s)
        {
            string artifactType = "@mod_info:EnsouledSteel.Orion.OrionTactics";
            foreach (Artifact artifact in s.artifacts)
            {
                if (artifact.Key() == artifactType)
                {
                    artifact.OnRemoveArtifact(s);
                }
            }

            s.artifacts.RemoveAll((Artifact r) => r.Key() == artifactType);
            // TODO visual upgrade

            //foreach (Part part in s.ship.parts)
            //{
            //    if (part.skin == "wing_ares")
            //    {
            //        part.skin = "wing_aresV2";
            //    }
            //}
        }

        public override bool isValidDeck(State state, Deck deck)
        {
            return true;
        }
    }
}
