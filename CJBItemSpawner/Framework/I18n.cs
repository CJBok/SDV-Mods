using System;
using System.CodeDom.Compiler;
using System.Diagnostics.CodeAnalysis;
using StardewModdingAPI;

namespace CJBItemSpawner.Framework
{
    /// <summary>Get translations from the mod's <c>i18n</c> folder.</summary>
    /// <remarks>This is auto-generated from the <c>i18n/default.json</c> file when the T4 template is saved.</remarks>
    [GeneratedCode("TextTemplatingFileGenerator", "1.0.0")]
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Deliberately named for consistency and to match translation conventions.")]
    internal static class I18n
    {
        /*********
        ** Fields
        *********/
        /// <summary>The mod's translation helper.</summary>
        private static ITranslationHelper Translations;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="translations">The mod's translation helper.</param>
        public static void Init(ITranslationHelper translations)
        {
            I18n.Translations = translations;
        }

        /// <summary>Get a translation equivalent to "Sort: Name".</summary>
        public static string Sort_ByName()
        {
            return I18n.GetByKey("sort.by-name");
        }

        /// <summary>Get a translation equivalent to "Sort: Type".</summary>
        public static string Sort_ByType()
        {
            return I18n.GetByKey("sort.by-type");
        }

        /// <summary>Get a translation equivalent to "Sort: ID".</summary>
        public static string Sort_ById()
        {
            return I18n.GetByKey("sort.by-id");
        }

        /// <summary>Get a translation equivalent to "All".</summary>
        public static string Filter_All()
        {
            return I18n.GetByKey("filter.all");
        }

        /// <summary>Get a translation equivalent to "Artisan & cooking".</summary>
        public static string Filter_ArtisanAndCooking()
        {
            return I18n.GetByKey("filter.artisan-and-cooking");
        }

        /// <summary>Get a translation equivalent to "Crafting: products".</summary>
        public static string Filter_Crafting_Products()
        {
            return I18n.GetByKey("filter.crafting.products");
        }

        /// <summary>Get a translation equivalent to "Crafting: resources".</summary>
        public static string Filter_Crafting_Resources()
        {
            return I18n.GetByKey("filter.crafting.resources");
        }

        /// <summary>Get a translation equivalent to "Decor: furniture".</summary>
        public static string Filter_Decor_Furniture()
        {
            return I18n.GetByKey("filter.decor.furniture");
        }

        /// <summary>Get a translation equivalent to "Decor: other".</summary>
        public static string Filter_Decor_Other()
        {
            return I18n.GetByKey("filter.decor.other");
        }

        /// <summary>Get a translation equivalent to "Equipment: boots".</summary>
        public static string Filter_EquipmentBoots()
        {
            return I18n.GetByKey("filter.equipment-boots");
        }

        /// <summary>Get a translation equivalent to "Equipment: clothes".</summary>
        public static string Filter_EquipmentClothes()
        {
            return I18n.GetByKey("filter.equipment-clothes");
        }

        /// <summary>Get a translation equivalent to "Equipment: hats".</summary>
        public static string Filter_EquipmentHats()
        {
            return I18n.GetByKey("filter.equipment-hats");
        }

        /// <summary>Get a translation equivalent to "Equipment: rings".</summary>
        public static string Filter_EquipmentRings()
        {
            return I18n.GetByKey("filter.equipment-rings");
        }

        /// <summary>Get a translation equivalent to "Equipment: tools".</summary>
        public static string Filter_EquipmentTools()
        {
            return I18n.GetByKey("filter.equipment-tools");
        }

        /// <summary>Get a translation equivalent to "Equipment: weapons".</summary>
        public static string Filter_EquipmentWeapons()
        {
            return I18n.GetByKey("filter.equipment-weapons");
        }

        /// <summary>Get a translation equivalent to "Farm: animal drops".</summary>
        public static string Filter_FarmAnimalDrops()
        {
            return I18n.GetByKey("filter.farm-animal-drops");
        }

        /// <summary>Get a translation equivalent to "Farm: crops".</summary>
        public static string Filter_FarmCrops()
        {
            return I18n.GetByKey("filter.farm-crops");
        }

        /// <summary>Get a translation equivalent to "Farm: seeds".</summary>
        public static string Filter_FarmSeeds()
        {
            return I18n.GetByKey("filter.farm-seeds");
        }

        /// <summary>Get a translation equivalent to "Fish".</summary>
        public static string Filter_Fish()
        {
            return I18n.GetByKey("filter.fish");
        }

        /// <summary>Get a translation equivalent to "Minerals & artifacts".</summary>
        public static string Filter_MineralsAndArtifacts()
        {
            return I18n.GetByKey("filter.minerals-and-artifacts");
        }

        /// <summary>Get a translation equivalent to "Misc".</summary>
        public static string Filter_Miscellaneous()
        {
            return I18n.GetByKey("filter.miscellaneous");
        }

        /// <summary>Get a translation by its key.</summary>
        /// <param name="key">The translation key.</param>
        /// <param name="tokens">An object containing token key/value pairs. This can be an anonymous object (like <c>new { value = 42, name = "Cranberries" }</c>), a dictionary, or a class instance.</param>
        public static Translation GetByKey(string key, object tokens = null)
        {
            if (I18n.Translations == null)
                throw new InvalidOperationException($"You must call {nameof(I18n)}.{nameof(I18n.Init)} from the mod's entry method before reading translations.");
            return I18n.Translations.Get(key, tokens);
        }
    }
}

