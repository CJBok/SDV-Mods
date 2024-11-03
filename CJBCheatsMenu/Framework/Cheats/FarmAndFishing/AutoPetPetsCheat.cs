using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.FarmAndFishing;

/// <summary>A cheat which automatically pets pet animals.</summary>
internal class AutoPetPetsCheat : BaseCheat
{
    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public override IEnumerable<OptionsElement> GetFields(CheatContext context)
    {
        yield return new CheatsOptionsCheckbox(
            label: I18n.Farm_AutoPetPets(),
            value: context.Config.AutoPetPets,
            setValue: value => context.Config.AutoPetPets = value
        );
    }

    /// <inheritdoc />
    public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
    {
        needsInput = false;
        needsUpdate = context.Config.AutoPetPets;
        needsRendering = false;
    }

    /// <inheritdoc />
    public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
    {
        if (!e.IsOneSecond || !Context.IsWorldReady)
            return;

        // Some mods (like A New Dream) add custom pets outside the farm which react to being pet. Only pet pets in
        // the standard locations (i.e. farm and farmhouses) to avoid issues like repeating dialogues.
        Farm farm = Game1.getFarm();
        this.ApplyInLocation(farm);
        foreach (Building building in farm.buildings)
        {
            if (building.GetIndoors() is FarmHouse home)
                this.ApplyInLocation(home);
        }
    }


    /*********
    ** Private methods
    *********/
    /// <summary>Pet all pets in a location.</summary>
    /// <param name="location">The location to search for pets.</param>
    private void ApplyInLocation(GameLocation location)
    {
        if (location.characters.Count > 0)
        {
            foreach (NPC character in location.characters)
            {
                if (character is Pet pet && (!pet.lastPetDay.TryGetValue(Game1.player.UniqueMultiplayerID, out int lastPetDay) || lastPetDay != Game1.Date.TotalDays))
                    pet.checkAction(Game1.player, pet.currentLocation);
            }
        }
    }
}
