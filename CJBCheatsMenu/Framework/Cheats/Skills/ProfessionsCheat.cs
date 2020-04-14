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
                this.GetField(context, "combat.fighter", Farmer.fighter),
                this.GetField(context, "combat.scout", Farmer.scout),
                this.GetField(context, "combat.acrobat", Farmer.acrobat),
                this.GetField(context, "combat.brute", Farmer.brute),
                this.GetField(context, "combat.defender", Farmer.defender),
                this.GetField(context, "combat.desperado", Farmer.desperado),
                this.GetField(context, "farming.rancher", Farmer.rancher),
                this.GetField(context, "farming.tiller", Farmer.tiller),
                this.GetField(context, "farming.agriculturist", Farmer.agriculturist),
                this.GetField(context, "farming.artisan", Farmer.artisan),
                this.GetField(context, "farming.coopmaster", Farmer.butcher), // butcher = coopmaster
                this.GetField(context, "farming.shepherd", Farmer.shepherd),
                this.GetField(context, "fishing.fisher", Farmer.fisher),
                this.GetField(context, "fishing.trapper", Farmer.trapper),
                this.GetField(context, "fishing.angler", Farmer.angler),
                this.GetField(context, "fishing.luremaster", Farmer.mariner), // mariner = luremaster (??)
                this.GetField(context, "fishing.mariner", Farmer.baitmaster), // baitmaster = mariner (??)
                this.GetField(context, "fishing.pirate", Farmer.pirate),
                this.GetField(context, "foraging.forester", Farmer.forester),
                this.GetField(context, "foraging.gatherer", Farmer.gatherer),
                this.GetField(context, "foraging.botanist", Farmer.botanist),
                this.GetField(context, "foraging.lumberjack", Farmer.lumberjack),
                this.GetField(context, "foraging.tapper", Farmer.tapper),
                this.GetField(context, "foraging.tracker", Farmer.tracker),
                this.GetField(context, "mining.geologist", Farmer.geologist),
                this.GetField(context, "mining.miner", Farmer.miner),
                this.GetField(context, "mining.blacksmith", Farmer.blacksmith),
                this.GetField(context, "mining.excavator", Farmer.excavator),
                this.GetField(context, "mining.gemologist", Farmer.gemologist),
                this.GetField(context, "mining.prospector", Farmer.burrower) // burrower = prospecter
            };
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get the option field to toggle a profession.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="translationKey">The unique portion of its display text translation key.</param>
        /// <param name="id">The game's profession ID.</param>
        private CheatsOptionsCheckbox GetField(CheatContext context, string translationKey, int id)
        {
            return new CheatsOptionsCheckbox(
                label: context.Text.Get($"professions.{translationKey}"),
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
