using MadokaMagica.Megumin.Achievements;
using RoR2;
using UnityEngine;

namespace MadokaMagica.Megumin.Content
{
    public static class MeguminUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                MegmuminMasteryAchievements.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(MegmuminMasteryAchievements.identifier),
                CrystalSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
