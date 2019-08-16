using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace App.Caliset.Localization
{
    public static class CalisetLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(CalisetConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(CalisetLocalizationConfigurer).GetAssembly(),
                        "App.Caliset.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
