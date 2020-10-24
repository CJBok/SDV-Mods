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
        public static string Labels_SortByName()
        {
            return I18n.GetByKey("labels.sort-by-name");
        }

        /// <summary>Get a translation equivalent to "Sort: Type".</summary>
        public static string Labels_SortByType()
        {
            return I18n.GetByKey("labels.sort-by-type");
        }

        /// <summary>Get a translation equivalent to "Sort: ID".</summary>
        public static string Labels_SortById()
        {
            return I18n.GetByKey("labels.sort-by-id");
        }

        /// <summary>Get a translation equivalent to "All".</summary>
        public static string Tabs_All()
        {
            return I18n.GetByKey("tabs.all");
        }

        /// <summary>Get a translation equivalent to "Tools & Equipment".</summary>
        public static string Tabs_Equipment()
        {
            return I18n.GetByKey("tabs.equipment");
        }

        /// <summary>Get a translation equivalent to "Seeds & Crops".</summary>
        public static string Tabs_Crops()
        {
            return I18n.GetByKey("tabs.crops");
        }

        /// <summary>Get a translation equivalent to "Fish & Bait & Trash".</summary>
        public static string Tabs_Fishing()
        {
            return I18n.GetByKey("tabs.fishing");
        }

        /// <summary>Get a translation equivalent to "Forage & Fruits".</summary>
        public static string Tabs_Forage()
        {
            return I18n.GetByKey("tabs.forage");
        }

        /// <summary>Get a translation equivalent to "Artifacts & Minerals".</summary>
        public static string Tabs_ArtifactsAndMinerals()
        {
            return I18n.GetByKey("tabs.artifacts-and-minerals");
        }

        /// <summary>Get a translation equivalent to "Resources & Crafting".</summary>
        public static string Tabs_ResourcesAndCrafting()
        {
            return I18n.GetByKey("tabs.resources-and-crafting");
        }

        /// <summary>Get a translation equivalent to "Artisan & Cooking".</summary>
        public static string Tabs_ArtisanAndCooking()
        {
            return I18n.GetByKey("tabs.artisan-and-cooking");
        }

        /// <summary>Get a translation equivalent to "Animal & Monster".</summary>
        public static string Tabs_AnimalAndMonster()
        {
            return I18n.GetByKey("tabs.animal-and-monster");
        }

        /// <summary>Get a translation equivalent to "Decorating".</summary>
        public static string Tabs_Decorating()
        {
            return I18n.GetByKey("tabs.decorating");
        }

        /// <summary>Get a translation equivalent to "Misc".</summary>
        public static string Tabs_Miscellaneous()
        {
            return I18n.GetByKey("tabs.miscellaneous");
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get a translation by its key.</summary>
        /// <param name="key">The translation key.</param>
        /// <param name="tokens">An object containing token key/value pairs. This can be an anonymous object (like <c>new { value = 42, name = "Cranberries" }</c>), a dictionary, or a class instance.</param>
        private static Translation GetByKey(string key, object tokens = null)
        {
            if (I18n.Translations == null)
                throw new InvalidOperationException($"You must call {nameof(I18n)}.{nameof(I18n.Init)} from the mod's entry method before reading translations.");
            return I18n.Translations.Get(key, tokens);
        }
    }
}

