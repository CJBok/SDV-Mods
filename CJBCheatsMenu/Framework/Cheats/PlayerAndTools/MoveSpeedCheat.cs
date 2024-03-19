using System.Collections.Generic;
using CJBCheatsMenu.Framework.Components;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buffs;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Cheats.PlayerAndTools
{
    /// <summary>A cheat which increases the player movement speed.</summary>
    internal class MoveSpeedCheat : BaseCheat
    {
        /*********
        ** Fields
        *********/
        /// <summary>The unique ID for the movement buff.</summary>
        private const string BuffId = "CJBok.CheatsMenu/MoveSpeed";

        /// <summary>The duration to set for the movement buff.</summary>
        private const int BuffDuration = 60000;

        /// <summary>The minimum duration remaining on the buff timer before it should be renewed.</summary>
        private const int BuffMinDurationBeforeRenew = 10000; // this should be >10% of BuffDuration to avoid the buff icon blinking before it's renewed


        /*********
        ** Public methods
        *********/
        /// <summary>Get the config UI fields to show in the cheats menu.</summary>
        /// <param name="context">The cheat context.</param>
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            yield return new CheatsOptionsSlider(
                label: I18n.Player_MovementSpeed(),
                value: context.Config.MoveSpeed,
                maxValue: 10,
                setValue: value => context.Config.MoveSpeed = value,
                format: val => val == 0
                    ? I18n.Player_MovementSpeed_Default()
                    : val.ToString()
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
            needsUpdate = context.Config.MoveSpeed > 0;
            needsRendering = false;

            Game1.player.buffs.Remove(MoveSpeedCheat.BuffId); // remove buff if disabled, or reset on next tick if still enabled
        }

        /// <summary>Handle a game update if <see cref="ICheat.OnSaveLoaded"/> indicated updates were needed.</summary>
        /// <param name="context">The cheat context.</param>
        /// <param name="e">The update event arguments.</param>
        public override void OnUpdated(CheatContext context, UpdateTickedEventArgs e)
        {
            // ignore in cutscenes
            if (Game1.eventUp || !Context.IsWorldReady)
                return;

            // ignore if walking
            bool running = Game1.player.running;
            bool runEnabled = running || Game1.options.autoRun != Game1.isOneOfTheseKeysDown(Game1.GetKeyboardState(), Game1.options.runButton); // auto-run enabled and not holding walk button, or disabled and holding run button
            if (!runEnabled)
                return;

            // add or extend buff
            if (!Game1.player.buffs.AppliedBuffs.TryGetValue(MoveSpeedCheat.BuffId, out Buff? buff) || buff.millisecondsDuration <= MoveSpeedCheat.BuffMinDurationBeforeRenew)
            {
                Game1.player.applyBuff(
                    new Buff(
                        id: MoveSpeedCheat.BuffId,
                        source: "CJB Cheats Menu",
                        displaySource: I18n.ModName(),
                        duration: MoveSpeedCheat.BuffDuration,
                        effects: new BuffEffects
                        {
                            Speed = { context.Config.MoveSpeed }
                        }
                    )
                );
            }
        }
    }
}
