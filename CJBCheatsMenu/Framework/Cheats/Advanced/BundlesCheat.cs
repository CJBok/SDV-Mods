using System;
using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Components;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Advanced
{
    /// <summary>A cheat which toggles community center and JojaMart bundles.</summary>
    internal class BundlesCheat : BaseCheat
    {
        /*********
        ** Fields
        *********/
        /// <summary>Maps JojaMart completion flags to their Community Center equivalent.</summary>
        private readonly IDictionary<string, string> JojaMartCompletionFlags = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            ["jojaBoilerRoom"] = "ccBoilerRoom",
            ["jojaCraftsRoom"] = "ccCraftsRoom",
            ["jojaFishTank"] = "ccFishTank",
            ["jojaPantry"] = "ccPantry",
            ["jojaVault"] = "ccVault"
        };

        /// <summary>Maps Community Center completion flags to their area ID.</summary>
        private readonly Dictionary<string, int> CommunityCenterCompletionFlags = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
        {
            ["ccBoilerRoom"] = CommunityCenter.AREA_BoilerRoom,
            ["ccBulletin"] = CommunityCenter.AREA_Bulletin,
            ["ccCraftsRoom"] = CommunityCenter.AREA_CraftsRoom,
            ["ccFishTank"] = CommunityCenter.AREA_FishTank,
            ["ccPantry"] = CommunityCenter.AREA_Pantry,
            ["ccVault"] = CommunityCenter.AREA_Vault
        };


        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            // unlock community center door
            yield return new CheatsOptionsCheckbox(
                label: context.Text.Get("flags.community-center.door-unlocked"),
                value: this.HasFlag("ccDoorUnlock"),
                setValue: value => this.SetFlag(value, "ccDoorUnlock")
            );

            // JojaMart membership
            yield return new CheatsOptionsCheckbox(
                label: context.Text.Get("flags.jojamart.membership"),
                value: this.HasFlag("JojaMember"),
                setValue: value => this.SetCommunityCenterFlags(value, "JojaMember")
            );

            // individual bundles
            var bundleFields = this.SortFields(
                this.GetBundleField("BoilerRoom", "Boiler", "ccBoilerRoom"),
                this.GetBundleField("BulletinBoard", "Bulletin", "ccBulletin"),
                this.GetBundleField("CraftsRoom", "Crafts", "ccCraftsRoom"),
                this.GetBundleField("FishTank", "FishTank", "ccFishTank"),
                this.GetBundleField("Pantry", "Pantry", "ccPantry"),
                this.GetBundleField("Vault", "Vault", "ccVault")
            );
            foreach (var field in bundleFields)
                yield return field;

            // abandoned JojaMart bundle
            yield return this.GetBundleField("AbandonedJojaMart", null, "ccMovieTheater");
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get an option field to toggle a community center bundle.</summary>
        /// <param name="areaName">The name used in the translation key for the bundle name.</param>
        /// <param name="rewardName">The name used in the translation key for the reward name (or <c>null</c> to show '???').</param>
        /// <param name="flag">The game flag to toggle.</param>
        private CheatsOptionsCheckbox GetBundleField(string areaName, string rewardName, string flag)
        {
            return new CheatsOptionsCheckbox(
                label: this.GetJunimoRewardText(areaName, rewardName),
                value: this.HasFlag(flag),
                setValue: value => this.SetCommunityCenterFlags(value, flag)
            );
        }

        /// <summary>Get the display text for a toggle to mark a Community Center or JojaMart bundle complete.</summary>
        /// <param name="areaName">The name used in the translation key for the bundle name.</param>
        /// <param name="rewardName">The name used in the translation key for the reward name (or <c>null</c> to show '???').</param>
        private string GetJunimoRewardText(string areaName, string rewardName = null)
        {
            return $"{Game1.content.LoadString($@"Strings\Locations:CommunityCenter_AreaName_{areaName}")} ({(rewardName != null ? Game1.content.LoadString($@"Strings\UI:JunimoNote_Reward{rewardName}") : "???")})";
        }

        /// <summary>Set whether the player has the given mail flag, and automatically fix issues related to community center flag changes.</summary>
        /// <param name="enable">Whether to add the flag, as opposed to removing it.</param>
        /// <param name="flags">The mail flags to set.</param>
        private void SetCommunityCenterFlags(bool enable, params string[] flags)
        {
            // set initial flags
            foreach (string flag in flags)
                this.SetFlag(enable, flag);

            // adjust game to reflect changes
            {
                bool allAreasDone = this.CommunityCenterCompletionFlags.Keys.All(this.HasFlag);
                bool isJoja = this.HasFlag("JojaMember");

                // fix completion flags
                this.SetFlag(allAreasDone, "ccComplete");
                foreach (var pair in this.JojaMartCompletionFlags)
                {
                    bool areaDone = isJoja && this.HasFlag(pair.Value);
                    this.SetFlag(areaDone, pair.Key);
                }

                // mark areas complete
                if (Game1.getLocationFromName("CommunityCenter") is CommunityCenter communityCenter)
                {
                    foreach (var pair in this.CommunityCenterCompletionFlags)
                    {
                        if (communityCenter.areasComplete.Length > pair.Value)
                            communityCenter.areasComplete[pair.Value] = this.HasFlag(pair.Key);
                    }
                }

                // restore movie theater if player is already in town
                if (this.HasFlag("ccMovieTheater") && Game1.getLocationFromName(nameof(AbandonedJojaMart)) is AbandonedJojaMart mart)
                    mart.restoreAreaCutscene();
            }
        }
    }
}
