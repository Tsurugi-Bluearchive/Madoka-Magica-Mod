using MadokaMagica.Jet.Achievements;
using RoR2;
using UnityEngine;

namespace MadokaMagica.Jet.Content
{
    public static class ZiluUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                JetMasteryAchievements.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(JetMasteryAchievements.identifier),
                JetSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
