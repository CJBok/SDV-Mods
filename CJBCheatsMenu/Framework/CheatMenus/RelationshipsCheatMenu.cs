using System;
using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.CheatMenus
{
    internal class RelationshipsCheatMenu : CheatMenu
    {
        /// <summary>
        /// Unique id for the cheat menu.
        /// </summary>
        public override string Id => "CBJCheatsMenu_RelationshipsCheatMenu";

        /// <summary>
        /// The title of the cheat menu (used for tab name).
        /// </summary>
        public override string Title => this.I18n.Get("tabs.relationships");

        /// <summary>
        /// Constructs a cheat menu for relationships.
        /// </summary>
        /// <param name="config">The user defined preferences.</param>
        /// <param name="cheats">Helper module that has various cheat utilities.</param>
        /// <param name="i18n">Helper module for internationalization.</param>
        public RelationshipsCheatMenu(ModConfig config, Cheats cheats, StardewModdingAPI.ITranslationHelper i18n)
            : base(config, cheats, i18n)
        {

        }

        /// <summary>
        /// The options rendered within this cheat menu.
        /// </summary>
        public override List<Menu.IOptionGroup> OptionGroups
        {
            get
            {
                List<Menu.IOptionGroup> optionGroups = new List<Menu.IOptionGroup>();

                Menu.OptionGroup relationshipsOptionGroup = new Menu.OptionGroup($"{this.I18n.Get("relationships.title")}:");
                relationshipsOptionGroup.Options.Add(new Menu.OptionCheckbox(this.I18n.Get("relationships.give-gifts-anytime"), this.Config.AlwaysGiveGift, value => this.Config.AlwaysGiveGift = value));
                relationshipsOptionGroup.Options.Add(new Menu.OptionCheckbox(this.I18n.Get("relationships.no-decay"), this.Config.NoFriendshipDecay, value => this.Config.NoFriendshipDecay = value));
                relationshipsOptionGroup.Options.Add(new Menu.Option($"{this.I18n.Get("relationships.friends")}:"));

                foreach (StardewValley.NPC npc in StardewValley.Utility.getAllCharacters())
                {
                    if (npc.CanSocialize)
                    {
                        relationshipsOptionGroup.Options.Add(new NPCHeartPicker(npc, this.Cheats));
                    }
                }
                optionGroups.Add(relationshipsOptionGroup);

                return optionGroups;
            }
        }

        /// <summary>
        /// A heart picker that sets the NPCs relationship value.
        /// </summary>
        public class NPCHeartPicker : Menu.IOptionHeartPicker
        {
            private const int MaxValue = 10;

            public StardewValley.NPC NPC { get; set; }

            private Cheats Cheats { get; set; }

            public NPCHeartPicker(StardewValley.NPC npc, Cheats cheats)
            {
                this.NPC = npc;
                this.Cheats = cheats;
            }

            public int Value
            {
                get
                {
                    int heartLevel = 0;
                    if (StardewValley.Game1.player.friendshipData.TryGetValue(this.NPC.Name, out StardewValley.Friendship friendship))

                    {
                        heartLevel = Math.Min(NPCHeartPicker.MaxValue, friendship.Points / StardewValley.NPC.friendshipPointsPerHeartLevel);
                    }

                    return Math.Max(0, heartLevel);
                }
                set
                {
                    if (StardewValley.Game1.player.friendshipData.TryGetValue(this.NPC.Name, out StardewValley.Friendship friendship))
                    {
                        friendship.Points = value * StardewValley.NPC.friendshipPointsPerHeartLevel;
                        this.Cheats.UpdateFriendship(this.NPC, friendship.Points);
                    }
                }
            }

            public string Label => NPC.displayName;

            public bool Disabled => false;
        }
    }
}
