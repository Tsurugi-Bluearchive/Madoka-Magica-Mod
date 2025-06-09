using RoR2;
using UnityEngine;

namespace MadokaMagica.Jet.Content
{

    public static class ZiluBuffs
    {
        // armor buff gained during roll
        public static BuffDef ziluCorruption;
        public static void Init(AssetBundle assetBundle)
        {
            ziluCorruption = Modules.Content.CreateAndAddBuff("ZuluCorruption",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);
        }
    }
}
