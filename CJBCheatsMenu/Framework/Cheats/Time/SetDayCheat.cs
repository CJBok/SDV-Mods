using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.Time;

/// <summary>A cheat which sets the current day.</summary>
internal class SetDayCheat : BaseDateCheat
{
    /*********
    ** Public methods
    *********/
    /// <inheritdoc />
    public override IEnumerable<OptionsElement> GetFields(CheatContext context)
    {
        yield return new CheatsOptionsSlider(
            label: I18n.Date_Day(),
            value: Game1.dayOfMonth,
            minValue: 1,
            maxValue: 28,
            setValue: this.SafelySetDay,
            width: 100
        );
    }


    /*********
    ** Private methods
    *********/
    /// <summary>Safely transition to the given day.</summary>
    /// <param name="day">The day.</param>
    private void SafelySetDay(int day)
    {
        this.SafelySetDate(day, Game1.currentSeason, Game1.year);
    }
}
