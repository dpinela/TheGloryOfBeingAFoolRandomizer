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
                shopDesc = new IC.BoxedString("???"),
                sprite = new IC.ItemChangerSprite("ShopIcons.WanderersJournal")
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