using RoR2;
using MadokaMagica.Modules.Achievements;
using MadokaMagica.Jet.SkillStates;

namespace MadokaMagica.Jet.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class JetMasteryAchievements : BaseMasteryAchievement
    {
        public const string identifier = JetSurvivor.JET_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = JetSurvivor.JET_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => JetSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}