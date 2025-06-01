using MadokaMagica.MamiTamoe.BaseStates;
using MadokaMagica.MamiTamoe.SkillStates;
using MadokaMagica.Modules;

namespace MadokaMagica.MamiTamoe
{
    public static class MamiStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(Collect));

            Modules.Content.AddEntityState(typeof(PrecisionStrkie));

            Modules.Content.AddEntityState(typeof(Dash));

            Modules.Content.AddEntityState(typeof(PrecisionBlast));

            Modules.Content.AddEntityState(typeof(MamiCharacterMain));
        }
    }
}
