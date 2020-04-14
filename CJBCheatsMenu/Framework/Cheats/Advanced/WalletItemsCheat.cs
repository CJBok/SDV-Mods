using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Advanced
{
    /// <summary>A cheat which toggles wallet items.</summary>
    internal class WalletItemsCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            return this.SortFields(
                new CheatsOptionsCheckbox(Game1.content.LoadString(@"Strings\StringsFromCSFiles:SkillsPage.cs.11587"), Game1.player.canUnderstandDwarves, value => Game1.player.canUnderstandDwarves = value),
                new CheatsOptionsCheckbox(Game1.content.LoadString(@"Strings\StringsFromCSFiles:SkillsPage.cs.11588"), Game1.player.hasRustyKey, value => Game1.player.hasRustyKey = value),
                new CheatsOptionsCheckbox(Game1.content.LoadString(@"Strings\StringsFromCSFiles:SkillsPage.cs.11589"), Game1.player.hasClubCard, value => Game1.player.hasClubCard = value),
                new CheatsOptionsCheckbox(Game1.content.LoadString(@"Strings\StringsFromCSFiles:SkillsPage.cs.11590"), Game1.player.hasSpecialCharm, value => Game1.player.hasSpecialCharm = value),
                new CheatsOptionsCheckbox(Game1.content.LoadString(@"Strings\StringsFromCSFiles:SkillsPage.cs.11591"), Game1.player.hasSkullKey, value => Game1.player.hasSkullKey = value),
                new CheatsOptionsCheckbox(Game1.content.LoadString(@"Strings\Objects:MagnifyingGlass"), Game1.player.hasMagnifyingGlass, value => Game1.player.hasMagnifyingGlass = value),
                new CheatsOptionsCheckbox(Game1.content.LoadString(@"Strings\Objects:DarkTalisman"), Game1.player.hasDarkTalisman, value => Game1.player.hasDarkTalisman = value),
                new CheatsOptionsCheckbox(Game1.content.LoadString(@"Strings\Objects:MagicInk"), Game1.player.hasMagicInk, value => Game1.player.hasMagicInk = value),
                new CheatsOptionsCheckbox(Game1.content.LoadString(@"Strings\Objects:BearPaw"), this.HasEvent(2120303), value => this.SetEvent(2120303, value)),
                new CheatsOptionsCheckbox(Game1.content.LoadString(@"Strings\Objects:SpringOnionBugs"), this.HasEvent(3910979), value => this.SetEvent(3910979, value))
            );
        }
    }
}
