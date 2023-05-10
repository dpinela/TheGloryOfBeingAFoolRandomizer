using UE = UnityEngine;
using IC = ItemChanger;

namespace TheGloryOfBeingAFoolRandomizer
{
    internal class TheGloryOfBeingAFool : IC.AbstractItem
    {
        public TheGloryOfBeingAFool()
        {
            name = Name;
            UIDef = new IC.UIDefs.MsgUIDef()
            {
                name = new IC.BoxedString("The Glory of Being a Fool"),
                shopDesc = new IC.BoxedString("Some bugs are so hungry for glory, any glory, that they would seek this trophy and wear it on their cloaks."),
                sprite = new FoolSprite()
            };
            tags = new() { MetadataTag() };
        }

        private static IC.Tag MetadataTag()
        {
            var t = new IC.Tags.InteropTag();
            t.Message = "RandoSupplementalMetadata";
            t.Properties["ModSource"] = nameof(TheGloryOfBeingAFoolRandomizer);
            t.Properties["PinSpriteKey"] = "Lore";
            return t;
        }

        public override void GiveImmediate(IC.GiveInfo info)
        {
            if (Cheer != null)
            {
                IC.Internal.SoundManager.PlayClipAtPoint(Cheer, HeroController.instance.transform.position);
            }
        }

        internal static UE.AudioClip Cheer;

        internal const string Name = "The_Glory_of_Being_a_Fool";
    }
}