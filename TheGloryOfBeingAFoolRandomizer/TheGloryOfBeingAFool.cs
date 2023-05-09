using UE = UnityEngine;
using IC = ItemChanger;
using ICInternal = ItemChanger.Internal;
using ICUIDefs = ItemChanger.UIDefs;

namespace TheGloryOfBeingAFoolRandomizer
{
    internal class TheGloryOfBeingAFool : IC.AbstractItem
    {
        public TheGloryOfBeingAFool()
        {
            name = Name;
            UIDef = new ICUIDefs.MsgUIDef()
            {
                name = new IC.BoxedString("The Glory of Being a Fool"),
                shopDesc = new IC.BoxedString("Some bugs are so hungry for glory, any glory, that they would seek this trophy and wear it on their cloaks."),
                sprite = new FoolSprite()
            };
        }

        public override void GiveImmediate(IC.GiveInfo info)
        {
            if (Cheer != null)
            {
                ICInternal.SoundManager.PlayClipAtPoint(Cheer, HeroController.instance.transform.position);
            }
        }

        internal static UE.AudioClip Cheer;

        internal const string Name = "The_Glory_of_Being_a_Fool";
    }
}