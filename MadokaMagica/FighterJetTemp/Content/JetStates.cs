using MadokaMagica.Jet.SkillStates;
using MadokaMagica.Jet.SkillStates.BaseStates;
using MadokaMagica.Jet;
using MadokaMagica.Zilu.SkillStates.BaseStates;

namespace MadokaMagica.Jet.Content
{
    public static class ZiluStates
    {
        public static void Init()
        {

            Modules.Content.AddEntityState(typeof(GatlingGun));

            Modules.Content.AddEntityState(typeof(PrecisionStrkie));

            Modules.Content.AddEntityState(typeof(Missile));

            Modules.Content.AddEntityState(typeof(CurseWitTeleport));

            Modules.Content.AddEntityState(typeof(JetCharacterMain));
        }
    }
}
