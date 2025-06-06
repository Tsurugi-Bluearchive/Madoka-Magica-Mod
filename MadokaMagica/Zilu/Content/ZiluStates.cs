using MadokaMagica.Zilu.SkillStates;
using MadokaMagica.Zilu.SkillStates.BaseStates;


namespace MadokaMagica.Zilu.Content
{
    public static class ZiluStates
    {
        public static void Init()
        {

            Modules.Content.AddEntityState(typeof(CeaselessBarrage));

            Modules.Content.AddEntityState(typeof(PrecisionStrkie));

            Modules.Content.AddEntityState(typeof(SpawnGun));

            Modules.Content.AddEntityState(typeof(PrecisionBlast));

            Modules.Content.AddEntityState(typeof(ZiluCharacterMain));
        }
    }
}
