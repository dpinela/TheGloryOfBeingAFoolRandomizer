using RSMVersioning = RandoSettingsManager.SettingsManagement.Versioning;

namespace TheGloryOfBeingAFoolRandomizer
{
    internal class StructuralVersioningPolicy : RSMVersioning.VersioningPolicy<Signature>
    {
        internal Func<ModSettings> settingsGetter;

        public override Signature Version => new() { FeatureSet = FeatureSetForSettings(settingsGetter()) };

        private static List<string> FeatureSetForSettings(ModSettings rs) =>
            SupportedFeatures.Where(f => f.feature(rs)).Select(f => f.name).ToList();

        public override bool Allow(Signature s) => s.FeatureSet.All(name => SupportedFeatures.Any(sf => sf.name == name));

        private static List<(Predicate<ModSettings> feature, string name)> SupportedFeatures = new()
        {
            (rs => rs.Enabled, "ItemAndLocation")
        };
    }

    internal struct Signature
    {
        public List<string> FeatureSet;
    }
}