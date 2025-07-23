using RoR2;
using MadokaMagica.Modules.Achievements;
using MadokaMagica.Megumin;

namespace MadokaMagica.Megumin.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class MegmuminMasteryAchievements : BaseMasteryAchievement
    {
        public const string identifier = CrystalSurvivor.MEGUMIN_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = CrystalSurvivor.MEGUMIN_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => CrystalSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}