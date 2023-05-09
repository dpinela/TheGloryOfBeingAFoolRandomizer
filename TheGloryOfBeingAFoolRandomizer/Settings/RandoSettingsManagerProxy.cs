using RSM = RandoSettingsManager.SettingsManagement;
using RSMVersioning = RandoSettingsManager.SettingsManagement.Versioning;

namespace TheGloryOfBeingAFoolRandomizer
{
    internal class RandoSettingsManagerProxy : RSM.RandoSettingsProxy<ModSettings, Signature>
    {
        internal Func<ModSettings> getter;
        internal Action<ModSettings> setter;

        public override string ModKey => nameof(TheGloryOfBeingAFoolRandomizer);

        public override RSMVersioning.VersioningPolicy<Signature> VersioningPolicy => new StructuralVersioningPolicy() { settingsGetter = this.getter };

        public override bool TryProvideSettings(out ModSettings? sent)
        {
            sent = getter();
            return sent.Enabled;
        }

        public override void ReceiveSettings(ModSettings? received)
        {
            setter(received ?? new());
        }
    }
}