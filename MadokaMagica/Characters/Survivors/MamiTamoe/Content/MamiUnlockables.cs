using HenryMod.Survivors.Henry.Achievements;
using RoR2;
using UnityEngine;

namespace HenryMod.Survivors.Henry
{
    public static class MamiUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                MamiMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(MamiMasteryAchievement.identifier),
                MamiSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
