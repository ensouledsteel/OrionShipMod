using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using HarmonyLib;
using OrionShipMod.Artifacts;
using Microsoft.Extensions.Logging;

namespace Orion
{
    public class OrionShipManifest : IShipPartManifest, IShipManifest, IStartershipManifest, ISpriteManifest, ICardManifest, IDeckManifest, IArtifactManifest
    {
        public DirectoryInfo? ModRootFolder { get; set; }
        public DirectoryInfo? GameRootFolder { get; set; }
        public string Name => "EnsouledSteel.Orion.OrionShipManifest";

        public static ExternalSprite? OrionChassisEmptySprite;
        public static ExternalSprite? OrionSquaddieSprite;
        public static ExternalSprite? OrionSquaddieInactiveSprite;
        public static ExternalSprite? OrionLeaderSprite;
        public static ExternalSprite? OrionBorderSprite;
        public static ExternalSprite? OrionFormationSprite;
        public static ExternalSprite? OrionEmptySprite;

        public static ExternalSprite? OrionTacticsSprite;
        public static ExternalSprite? OrionSuperioritySprite;
        public static ExternalSprite? OrionControlBaySprite;

        public static ExternalShip? OrionShip;

        public static ExternalPart? OrionEmpty;
        public static ExternalPart? OrionSquaddieLeft;
        public static ExternalPart? OrionSquaddieRight;
        public static ExternalPart? OrionLeader;

        public static ExternalArtifact? OrionTactics;
        public static ExternalArtifact? OrionSuperiority;
        public static ExternalArtifact? OrionControlBay;

        public static ExternalDeck? OrionDeck { get; private set; }
        public static ExternalCard? OrionFormation { get; private set; }

        public IEnumerable<string> Dependencies => new string[0];

        IEnumerable<DependencyEntry> IManifest.Dependencies => new DependencyEntry[0];

        public ILogger? Logger { get; set; }

        private void PatchBossArtifacts(Harmony harmony)
        {
            var blocked_artifacts_method = typeof(ArtifactReward).GetMethod("GetBlockedArtifacts", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            var add_boss_artifact_method = typeof(OrionShipManifest).GetMethod("AddBossArtifact", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            harmony.Patch(blocked_artifacts_method, postfix: new HarmonyMethod(add_boss_artifact_method));
        }

        private static void AddBossArtifact(State s, ref HashSet<Type> __result)
        {
            if (s.ship.key != "EnsouledSteel.Orion.OrionShip.StarterShip")
            {
                __result.Add(typeof(OrionSuperiority));
            }
        }

        public void LoadManifest(ISpriteRegistry spriteRegistry)
        {
            if (ModRootFolder == null) throw new Exception("ROOT FOLDER GONE, EEEEEEDIOT");
            {
                var path = Path.Combine(
                    ModRootFolder.FullName,
                    "Sprites",
                    Path.GetFileName("chassis_empty.png"));

                OrionChassisEmptySprite = new ExternalSprite(
                    "EnsouledSteel.Orion.OrionChassisEmptySprite",
                    new FileInfo(path));

                if (!spriteRegistry.RegisterArt(OrionChassisEmptySprite))
                    throw new Exception("Cannot register sprite.");
            }
            {
                var path = Path.Combine(
                    ModRootFolder.FullName,
                    "Sprites",
                    Path.GetFileName("orion_empty.png"));

                OrionEmptySprite = new ExternalSprite(
                    "EnsouledSteel.Orion.OrionEmptySprite",
                    new FileInfo(path));

                if (!spriteRegistry.RegisterArt(OrionEmptySprite))
                    throw new Exception("Cannot register sprite.");
            }
            {
                var path = Path.Combine(
                    ModRootFolder.FullName,
                    "Sprites",
                    Path.GetFileName("orion_squaddie.png"));

                OrionSquaddieSprite = new ExternalSprite(
                    "EnsouledSteel.Orion.OrionSquaddieSprite",
                    new FileInfo(path));

                if (!spriteRegistry.RegisterArt(OrionSquaddieSprite))
                    throw new Exception("Cannot register sprite.");
            }
            {
                var path = Path.Combine(
                    ModRootFolder.FullName,
                    "Sprites",
                    Path.GetFileName("orion_squaddie_inactive.png"));

                OrionSquaddieInactiveSprite = new ExternalSprite(
                    "EnsouledSteel.Orion.OrionSquaddieInactiveSprite",
                    new FileInfo(path));

                if (!spriteRegistry.RegisterArt(OrionSquaddieInactiveSprite))
                    throw new Exception("Cannot register sprite.");
            }
            {
                var path = Path.Combine(
                    ModRootFolder.FullName,
                    "Sprites",
                    Path.GetFileName("orion_leader.png"));
                
                OrionLeaderSprite = new ExternalSprite(
                    "EnsouledSteel.Orion.OrionLeaderSprite",
                    new FileInfo(path));
                
                if (!spriteRegistry.RegisterArt(OrionLeaderSprite))
                    throw new Exception("Cannot register sprite.");
            }
            {
                var path = Path.Combine(
                    ModRootFolder.FullName,
                    "Sprites",
                    Path.GetFileName("tactics.png"));

                OrionTacticsSprite = new ExternalSprite(
                    "EnsouledSteel.Orion.OrionTacticsSprite",
                    new FileInfo(path));

                if (!spriteRegistry.RegisterArt(OrionTacticsSprite))
                    throw new Exception("Cannot register sprite.");
            }
            {
                var path = Path.Combine(
                    ModRootFolder.FullName,
                    "Sprites",
                    Path.GetFileName("superiority.png"));

                OrionSuperioritySprite = new ExternalSprite(
                    "EnsouledSteel.Orion.OrionSuperioritySprite",
                    new FileInfo(path));

                if (!spriteRegistry.RegisterArt(OrionSuperioritySprite))
                    throw new Exception("Cannot register sprite.");
            }
            {
                var path = Path.Combine(
                    ModRootFolder.FullName,
                    "Sprites",
                    Path.GetFileName("control_bay.png"));

                OrionControlBaySprite = new ExternalSprite(
                    "EnsouledSteel.Orion.OrionControlBaySprite",
                    new FileInfo(path));

                if (!spriteRegistry.RegisterArt(OrionControlBaySprite))
                    throw new Exception("Cannot register sprite.");
            }
            {
                var path = Path.Combine(
                    ModRootFolder.FullName,
                    "Sprites",
                    Path.GetFileName("orion_border.png"));

                OrionBorderSprite = new ExternalSprite(
                    "EnsouledSteel.Orion.OrionBorderSprite",
                    new FileInfo(path));

                if (!spriteRegistry.RegisterArt(OrionBorderSprite))
                    throw new Exception("Cannot register sprite.");
            }
            {
                var path = Path.Combine(
                    ModRootFolder.FullName,
                    "Sprites",
                    Path.GetFileName("orion_formation.png"));

                OrionFormationSprite = new ExternalSprite(
                    "EnsouledSteel.Orion.OrionFormationSprite",
                    new FileInfo(path));

                if (!spriteRegistry.RegisterArt(OrionFormationSprite))
                    throw new Exception("Cannot register sprite.");
            }
        }

        private static System.Drawing.Color OrionColor = System.Drawing.Color.FromArgb(52, 61, 76);
        public void LoadManifest(IDeckRegistry registry)
        {
            OrionDeck = new ExternalDeck(
                "EnsouledSteel.Orion.OrionDeck",
                OrionColor,
                System.Drawing.Color.Black,
                OrionFormationSprite ?? throw new Exception("Could not load Orion Formation Sprite"),
                OrionBorderSprite ?? throw new Exception("Could not load Orion Border Sprite"),
                null);
            registry.RegisterDeck(OrionDeck);
        }

        public void LoadManifest(ICardRegistry registry)
        {
            OrionFormation = new ExternalCard(
                "EnsouledSteel.Orion.OrionFormation", 
                typeof(OrionShipMod.Cards.OrionFormation), 
                OrionFormationSprite ?? throw new Exception("Could not load Orion Formation Sprite"), 
                OrionDeck);
            // We are nasty lil devils, the switching back is actually handled in our artifact code, not the card
            // This is bad, but its a last minute balance change
            OrionFormation.AddLocalisation("Form Up!", "Switch formation until the start of your next turn.", "Switch formation.");
            registry.RegisterCard(OrionFormation);
        }

        public void LoadManifest(IArtifactRegistry registry)
        {
            var harmony = new Harmony("EnsouledSteel.Orion.OrionShipManifest");
            PatchBossArtifacts(harmony);

            {
                OrionTactics = new ExternalArtifact(
                    "EnsouledSteel.Orion.OrionTactics",
                    typeof(OrionTactics),
                    OrionTacticsSprite ?? throw new Exception("Could not load Orion Tactics Sprite"),
                    Array.Empty<ExternalGlossary>());

                OrionTactics.AddLocalisation("Tactics", "When you play a card from every crewmate's deck on a turn, turn on both cannons. At the start of combat, gain a <c=card>Form Up!</c> <c=downside>When hit, a random crewmate goes missing.</c>");
                registry.RegisterArtifact(OrionTactics);
            }
            {
                OrionSuperiority = new ExternalArtifact(
                    "EnsouledSteel.Orion.OrionSuperiority",
                    typeof(OrionSuperiority),
                    OrionSuperioritySprite ?? throw new Exception("Could not load Orion Superiority Sprite"),
                    Array.Empty<ExternalGlossary>());

                OrionSuperiority.AddLocalisation("Superiority", "(Orion exclusive artifact!)\nReplaces <c=artifact>TACTICS</c>. When you play 3 cards on your turn, turn on both cannons. At the start of combat, gain a <c=card>Form Up! A</c>.\n<c=downside>When hit, a random crewmate goes missing.</c>");
                registry.RegisterArtifact(OrionSuperiority);
            }
            {
                OrionControlBay = new ExternalArtifact(
                    "EnsouledSteel.Orion.OrionControlBay",
                    typeof(OrionControlBay),
                    OrionControlBaySprite ?? throw new Exception("Could not load Orion Control Bay Sprite"),
                    Array.Empty<ExternalGlossary>());

                OrionControlBay.AddLocalisation("Control Bay", "On your turn, your cockpit functions as a missile bay.");
                registry.RegisterArtifact(OrionControlBay);
            }
        }

        public void LoadManifest(IShipPartRegistry registry)
        {
            OrionEmpty = new ExternalPart(
                "EnsouledSteel.Orion.OrionShip.OrionEmpty",
                new Part()
                {
                    active = false,
                    damageModifier = PDamMod.none,
                    type = PType.empty,
                    invincible = false
                },
                OrionEmptySprite ?? throw new Exception("Could not load Orion Empty Sprite"),
                OrionEmptySprite ?? throw new Exception("Could not load Orion Empty Inactive Sprite"));

            OrionSquaddieLeft = new ExternalPart(
                "EnsouledSteel.Orion.OrionShip.OrionSquaddieLeft",
                new Part()
                {
                    active = true,
                    damageModifier = PDamMod.none,
                    type = PType.cannon,
                    invincible = false
                },
                OrionSquaddieSprite ?? throw new Exception("Could not load Orion Squaddie Sprite"),
                OrionSquaddieInactiveSprite ?? throw new Exception("Could not load Orion Squaddie Inactive Sprite"));

            OrionSquaddieRight = new ExternalPart(
                "EnsouledSteel.Orion.OrionShip.OrionSquaddieRight",
                new Part()
                {
                    active = false,
                    damageModifier = PDamMod.none,
                    type = PType.cannon,
                    invincible = false
                },
                OrionSquaddieSprite ?? throw new Exception("Could not load Orion Squaddie Sprite"), 
                OrionSquaddieInactiveSprite ?? throw new Exception("Could not load Orion Squaddie Inactive Sprite"));

            OrionLeader = new ExternalPart(
                "EnsouledSteel.Orion.OrionShip.OrionLeader",
                new Part()
                {
                    active = true,
                    damageModifier = PDamMod.none,
                    type = PType.cockpit,
                    invincible = false
                },
                OrionLeaderSprite ?? throw new Exception("Could not load Orion Leader Sprite"));

            registry.RegisterPart(OrionEmpty);
            registry.RegisterPart(OrionSquaddieLeft);
            registry.RegisterPart(OrionSquaddieRight);
            registry.RegisterPart(OrionLeader);
        }

        public void LoadManifest(IShipRegistry shipRegistry)
        {
            OrionShip = new ExternalShip(
                "EnsouledSteel.Orion.OrionShip.Ship",
                new Ship()
                {
                    baseDraw = 5,
                    baseEnergy = 3,
                    heatTrigger = 3,
                    heatMin = 0,
                    hull = 6,
                    hullMax = 6,
                    shieldMaxBase = 3
                },
                new ExternalPart[] {
                    OrionSquaddieLeft ?? throw new Exception("Could not load OrionSquaddie"), 
                    OrionEmpty ?? throw new Exception("Could not load OrionEmpty"), 
                    OrionLeader ?? throw new Exception("Could not load OrionLeader"), 
                    OrionEmpty, 
                    OrionSquaddieRight ?? throw new Exception("Could not load OrionSquaddie"),
                },
                OrionChassisEmptySprite,
                null
                );

            shipRegistry.RegisterShip(OrionShip);
        }

        public void LoadManifest(IStartershipRegistry registry)
        {
            if (OrionShip == null)
                return;
            var starter = new ExternalStarterShip(
                "EnsouledSteel.Orion.OrionShip.StarterShip",
                OrionShip.GlobalName,
                new ExternalCard[0], 
                new ExternalArtifact[] { OrionTactics ?? throw new Exception(), OrionControlBay ?? throw new Exception() }, 
                new Type[] { typeof(DodgeColorless), typeof(BasicShieldColorless), typeof(CannonColorless) }, 
                new Type[] { typeof(ShieldPrep) },
                exclusiveArtifacts: new ExternalArtifact[] {OrionSuperiority ?? throw new Exception("Could not load Orion Superiority Artifact")});

            starter.AddLocalisation("Orion", "A squad of 3 light fighter ships - extremely fragile, but highly mobile.");

            registry.RegisterStartership(starter);
        }
    }
}