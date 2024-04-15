using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.FarmAndFishing
{
    /// <summary>A cheat which automatically pets farm animals.</summary>
    internal class AutoPetAnimalsCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            yield return new CheatsOptionsCheckbox(
                label: I18n.Farm_AutoPetAnimals(),
                value: context.Config.AutoPetAnimals,
                setValue: value => context.Config.AutoPetAnimals = value
            );
        }

        /// <summary>Handle the cheat options being loaded or changed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="needsUpdate">Whether the cheat should be notified of game updates.</param>
        /// <param name="needsInput">Whether the cheat should be notified of button presses.</param>
        /// <param name="needsRendering">Whether the cheat should be notified of render ticks.</param>
        public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
        {
            needsInput = false;
            needsUpdate = context.Config.AutoPetAnimals;
            needsRendering = false;
        }

        /// <summary>Handle the player loading a save file.</summary>
        /// <param name="context">The cheat context.</param>
        /// <summary>Handle a game update if <see cref="ICheat.OnSaveLoaded"/> indicated updates were needed.</summary>
        /// <param name="e">The update event arguments.</param>
        public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
        {
            if (!e.IsOneSecond || !Context.IsWorldReady)
                return;

            Utility.ForEachLocation(
                location =>
                {
                    if (this.ShouldPetAnimalsHere(location))
                    {
                        List<FarmAnimal> animalsToPet = location.getAllFarmAnimals();
                        animalsToPet.RemoveAll(animal => animal.wasPet.Value);

                        if (animalsToPet.Count > 0)
                        {
                            int wasTime = Game1.timeOfDay;
                            Item wasTemporaryItem = Game1.player.TemporaryItem;

                            try
                            {
                                // avoid feeding hay
                                if (Game1.player.ActiveObject?.QualifiedItemId == "(O)178")
                                    Game1.player.TemporaryItem = new Object("0", 1);

                                // avoid 'trying to sleep' dialogue popup
                                if (Game1.timeOfDay >= 1900)
                                    Game1.timeOfDay = 1850;

                                // pet animals
                                foreach (FarmAnimal animal in animalsToPet)
                                    animal.pet(Game1.player);
                            }
                            finally
                            {
                                // restore previous values
                                Game1.player.TemporaryItem = wasTemporaryItem;
                                Game1.timeOfDay = wasTime;
                            }
                        }
                    }

                    return true;
                },
                includeInteriors: false
            );
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get whether to try petting any animals in a location or its buildings.</summary>
        /// <param name="location">The location to check.</param>
        private bool ShouldPetAnimalsHere(GameLocation location)
        {
            // skip if there are no animals here
            if (location.animals.Length is 0 && location.buildings.Count is 0)
                return false;

            // skip if players can't build here
            // Animals in a non-buildable location may be mod NPCs, which can react to being pet with dialogue.
            return location.IsBuildableLocation();
        }
    }
}
