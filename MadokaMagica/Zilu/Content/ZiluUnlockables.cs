using MadokaMagica.Zilu.Achievements;
using RoR2;
using UnityEngine;

namespace MadokaMagica.Zilu.Content
{
    public static class ZiluUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                ZiluMasteryAchievements.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(ZiluMasteryAchievements.identifier),
                ZiluSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
