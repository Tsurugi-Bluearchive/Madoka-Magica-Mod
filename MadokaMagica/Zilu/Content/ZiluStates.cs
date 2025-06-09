using MadokaMagica.Zilu.SkillStates;
using MadokaMagica.Zilu.SkillStates.BaseStates;


namespace MadokaMagica.Zilu.Content
{
    public static class ZiluStates
    {
        public static void Init()
        {

            Modules.Content.AddEntityState(typeof(CurseWitCounter));

            Modules.Content.AddEntityState(typeof(PrecisionStrkie));

            Modules.Content.AddEntityState(typeof(Missile));

            Modules.Content.AddEntityState(typeof(CurseWitTeleport));

            Modules.Content.AddEntityState(typeof(JetCharacterMain));
        }
    }
}
