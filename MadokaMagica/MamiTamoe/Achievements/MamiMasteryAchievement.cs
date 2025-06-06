using RoR2;
using MadokaMagica.Modules.Achievements;
using MadokaMagica.MamiTamoe;

namespace MadokaMagica.MamiTamoe.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class ZiluMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = MamiSurvivor.MAMI_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = MamiSurvivor.MAMI_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => MamiSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}