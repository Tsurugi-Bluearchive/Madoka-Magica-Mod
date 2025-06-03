using MadokaMagica.MamiTamoe.BaseStates;
using MadokaMagica.MamiTamoe.SkillStates;
using MadokaMagica.Modules;
using EntityStates;


namespace MadokaMagica.MamiTamoe
{
    public static class MamiStates
    {
        public static void Init()
        {
            
            Modules.Content.AddEntityState(typeof(CeaselessBarrage));

            Modules.Content.AddEntityState(typeof(PrecisionStrkie));

            Modules.Content.AddEntityState(typeof(SpawnGun));

            Modules.Content.AddEntityState(typeof(PrecisionBlast));

            Modules.Content.AddEntityState(typeof(MamiCharacterMain));
        }
    }
}
