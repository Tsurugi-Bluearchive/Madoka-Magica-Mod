using RoR2;
using MadokaMagica.Modules.Achievements;

namespace MadokaMagica.Zilu.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class ZiluMasteryAchievements : BaseMasteryAchievement
    {
        public const string identifier = ZiluSurvivor.MAMI_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = ZiluSurvivor.MAMI_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => ZiluSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}