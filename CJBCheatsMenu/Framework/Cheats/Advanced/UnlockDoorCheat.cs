using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Components;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Advanced
{
    /// <summary>A cheat which unlocks a locked door.</summary>
    internal class UnlockDoorCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <inheritdoc />
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            // adventurer's guild
            yield return new CheatsOptionsCheckbox(
                label: I18n.Flags_Unlocked_Guild(),
                value: this.HasFlag("guildMember"),
                setValue: value => this.SetFlag(value, "guildMember")
            );

            // NPC rooms
            foreach (NPC npc in this.GetSocialCharacters().OrderBy(p => p.displayName))
            {
                yield return new CheatsOptionsCheckbox(
                    label: I18n.Flags_Unlocked_Room(name: npc.displayName),
                    value: this.HasFlag($"doorUnlock{npc.Name}"),
                    setValue: value => this.SetFlag(value, $"doorUnlock{npc.Name}")
                );
            }
        }
    }
}
