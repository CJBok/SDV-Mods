using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Skills
{
    /// <summary>A cheat which toggles player professions.</summary>
    internal class ProfessionsCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            return new[]
            {
                this.GetField(I18n.Professions_Combat_Fighter(), Farmer.fighter),
                this.GetField(I18n.Professions_Combat_Scout(), Farmer.scout),
                this.GetField(I18n.Professions_Combat_Acrobat(), Farmer.acrobat),
                this.GetField(I18n.Professions_Combat_Brute(), Farmer.brute),
                this.GetField(I18n.Professions_Combat_Defender(), Farmer.defender),
                this.GetField(I18n.Professions_Combat_Desperado(), Farmer.desperado),

                this.GetField(I18n.Professions_Farming_Rancher(), Farmer.rancher),
                this.GetField(I18n.Professions_Farming_Tiller(), Farmer.tiller),
                this.GetField(I18n.Professions_Farming_Agriculturist(), Farmer.agriculturist),
                this.GetField(I18n.Professions_Farming_Artisan(), Farmer.artisan),
                this.GetField(I18n.Professions_Farming_Coopmaster(), Farmer.butcher), // butcher = coopmaster
                this.GetField(I18n.Professions_Farming_Shepherd(), Farmer.shepherd),

                this.GetField(I18n.Professions_Fishing_Fisher(), Farmer.fisher),
                this.GetField(I18n.Professions_Fishing_Trapper(), Farmer.trapper),
                this.GetField(I18n.Professions_Fishing_Angler(), Farmer.angler),
                this.GetField(I18n.Professions_Fishing_Luremaster(), Farmer.mariner), // mariner = luremaster (??)
                this.GetField(I18n.Professions_Fishing_Mariner(), Farmer.baitmaster), // baitmaster = mariner (??)
                this.GetField(I18n.Professions_Fishing_Pirate(), Farmer.pirate),

                this.GetField(I18n.Professions_Foraging_Forester(), Farmer.forester),
                this.GetField(I18n.Professions_Foraging_Gatherer(), Farmer.gatherer),
                this.GetField(I18n.Professions_Foraging_Botanist(), Farmer.botanist),
                this.GetField(I18n.Professions_Foraging_Lumberjack(), Farmer.lumberjack),
                this.GetField(I18n.Professions_Foraging_Tapper(), Farmer.tapper),
                this.GetField(I18n.Professions_Foraging_Tracker(), Farmer.tracker),

                this.GetField(I18n.Professions_Mining_Geologist(), Farmer.geologist),
                this.GetField(I18n.Professions_Mining_Miner(), Farmer.miner),
                this.GetField(I18n.Professions_Mining_Blacksmith(), Farmer.blacksmith),
                this.GetField(I18n.Professions_Mining_Excavator(), Farmer.excavator),
                this.GetField(I18n.Professions_Mining_Gemologist(), Farmer.gemologist),
                this.GetField(I18n.Professions_Mining_Prospector(), Farmer.burrower) // burrower = prospecter
            };
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get the option field to toggle a profession.</summary>
        /// <param name="label">The option label text.</param>
        /// <param name="id">The game's profession ID.</param>
        private CheatsOptionsCheckbox GetField(string label, int id)
        {
            return new CheatsOptionsCheckbox(
                label: label,
                value: this.GetProfession(id),
                setValue: value => this.SetProfession(id, value)
            );
        }

        /// <summary>Get whether the player has the given profession.</summary>
        /// <param name="id">The profession ID.</param>
        private bool GetProfession(int id)
        {
            return Game1.player.professions.Contains(id);
        }

        /// <summary>Toggle a player profession.</summary>
        /// <param name="id">The profession ID.</param>
        /// <param name="enable">Whether to enable the profession (else disable).</param>
        /// <remarks>Derived from <see cref="LevelUpMenu.getImmediateProfessionPerk"/>.</remarks>
        private void SetProfession(int id, bool enable)
        {
            // skip if done
            if (enable == this.GetProfession(id))
                return;

            // get health bonus for profession
            int healthBonus = id switch
            {
                Farmer.fighter => 15,
                Farmer.defender => 25,
                _ => 0
            };

            // apply
            Farmer player = Game1.player;
            if (enable)
            {
                player.maxHealth += healthBonus;
                player.health += healthBonus;
                player.professions.Add(id);
            }
            else
            {
                player.health -= healthBonus;
                player.maxHealth -= healthBonus;
                player.professions.Remove(id);
            }
            LevelUpMenu.RevalidateHealth(player);
        }
    }
}
