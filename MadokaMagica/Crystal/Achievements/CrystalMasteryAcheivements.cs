using RoR2;
using MadokaMagica.Modules.Achievements;

namespace MadokaMagica.Crystal.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class CrystalMasteryAchievements : BaseMasteryAchievement
    {
        public const string identifier = CrystalSurvivor.CRYSTAL_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = CrystalSurvivor.CRYSTAL_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => CrystalSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}