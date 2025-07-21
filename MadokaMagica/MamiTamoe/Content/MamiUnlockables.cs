using MadokaMagica.MamiTamoe.Achievements;
using RoR2;
using UnityEngine;

namespace MadokaMagica.MamiTamoe.Content
{
    public static class MamiUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                MamiMasteryAchievements.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(MamiMasteryAchievements.identifier),
                MamiSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
