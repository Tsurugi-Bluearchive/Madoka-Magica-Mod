using MadokaMagica.Crystal.Achievements;
using RoR2;
using UnityEngine;

namespace MadokaMagica.Crystal.Content
{
    public static class CrystalUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                CrystalMasteryAchievements.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(CrystalMasteryAchievements.identifier),
                CrystalSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
