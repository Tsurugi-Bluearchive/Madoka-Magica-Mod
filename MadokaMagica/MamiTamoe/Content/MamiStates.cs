using MadokaMagica.MamiTamoe.SkillStates;
using MadokaMagica.MamiTamoe.SkillStates.BaseStates;


namespace MadokaMagica.MamiTamoe.Content
{
    public static class MamiStates
    {
        public static void Init()
        {

            Modules.Content.AddEntityState(typeof(CeaselessBarrage));

            Modules.Content.AddEntityState(typeof(PrecisionStrkie));

            Modules.Content.AddEntityState(typeof(SpawnGun));

            Modules.Content.AddEntityState(typeof(PrecisionBlast));
        }
    }
}
