using System;
using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Components;
using StardewValley;
using StardewValley.Enchantments;
using StardewValley.Menus;
using StardewValley.Tools;

namespace CJBCheatsMenu.Framework.Cheats.PlayerAndTools
{
    /// <summary>A cheat which sets the enchantments for the selected tool.</summary>
    internal class ToolEnchantmentsCheat : BaseCheat
    {
        /*********
        ** Public methods
        *********/
        /// <inheritdoc />
        public override IEnumerable<OptionsElement> GetFields(CheatContext context)
        {
            Tool tool = Game1.player.CurrentTool;

            // no tool selected
            if (tool is null)
                return [new DescriptionElement(I18n.ToolEnchantments_SelectTool(), splitLinesIfNeeded: false)];

            // no enchantments available
            BaseEnchantment[] enchantments = this.GetValidEnchantments(tool).ToArray();
            if (enchantments.Length == 0)
                return [new DescriptionElement(I18n.ToolEnchantments_NoneForTool(tool.DisplayName), splitLinesIfNeeded: false)];

            // else add checkboxes
            return [
                new DescriptionElement(I18n.ToolEnchantments_ForTool(tool.DisplayName)),
                ..this.SortFields(
                    enchantments
                        .Select(enchantment => this.GetField(tool, enchantment))
                        .ToArray()
                )
            ];
        }

        /// <inheritdoc />
        public override void OnConfig(CheatContext context, out bool needsInput, out bool needsUpdate, out bool needsRendering)
        {
            needsInput = false;
            needsUpdate = context.Config.NoFriendshipDecay;
            needsRendering = false;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get a field to display.</summary>
        private OptionsElement GetField(Tool tool, BaseEnchantment enchantment)
        {
            string displayName = enchantment.GetDisplayName();
            bool hasEnchantment = tool.enchantments.Any(other => enchantment.GetName() == other.GetName());

            return new CheatsOptionsCheckbox(
                label: displayName,
                value: hasEnchantment,
                setValue: value =>
                {
                    if (value)
                    {
                        // avoid tool.AddEnchantment here, which may remove other enchantments
                        tool.enchantments.Add(enchantment);
                        enchantment.ApplyTo(tool, Game1.player);
                    }
                    else
                        tool.RemoveEnchantment(enchantment);
                }
            );
        }

        /// <summary>Get the enchantments which can be applied to a tool.</summary>
        /// <param name="tool">The tool whose enchantments to edit.</param>
        /// <remarks>The innate enchantments are derived from <see cref="MeleeWeapon.attemptAddRandomInnateEnchantment"/>.</remarks>>
        private IEnumerable<BaseEnchantment> GetValidEnchantments(Tool tool)
        {
            // explicit enchantments
            IEnumerable<BaseEnchantment> enchantments = BaseEnchantment.GetAvailableEnchantmentsForItem(tool);

            // 'innate' enchantments
            if (tool is MeleeWeapon weapon)
            {
                int weaponLevel = weapon.getItemLevel();

                enchantments = [
                    ..enchantments,
                    new DefenseEnchantment { Level = Math.Max(1, Math.Min(2, weaponLevel / 2 + 1)) },
                    new LightweightEnchantment { Level = 5 },
                    new SlimeGathererEnchantment(),
                    new AttackEnchantment { Level = Math.Max(1, Math.Min(5, weaponLevel / 2 + 1)) },
                    new CritEnchantment { Level = Math.Max(1, Math.Min(3, (weaponLevel - 1) / 3)) },
                    new WeaponSpeedEnchantment { Level = Math.Max(1, Math.Min(Math.Max(1, 4 - weapon.speed.Value), weaponLevel - 1)) },
                    new SlimeSlayerEnchantment(),
                    new CritPowerEnchantment { Level = Math.Max(1, Math.Min(3, (weaponLevel - 1) / 3)) }
                ];
            }

            // tool's current enchantments
            if (tool.enchantments.Count > 0)
            {
                enchantments = tool.enchantments
                    .Concat(enchantments)
                    .DistinctBy(p => p.GetName());
            }

            return enchantments;
        }
    }
}
