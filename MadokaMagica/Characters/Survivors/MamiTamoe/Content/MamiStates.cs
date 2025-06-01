using MadokaMagica.MamiTamoe.BaseStates;
using MadokaMagica.MamiTamoe.SkillStates;
using MadokaMagica.Modules;

namespace MadokaMagica.MamiTamoe
{
    public static class MamiStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(Scarf));

            Modules.Content.AddEntityState(typeof(PrecisionStrkie));

            Modules.Content.AddEntityState(typeof(Dash));

            Modules.Content.AddEntityState(typeof(ThrowBomb));

            Modules.Content.AddEntityState(typeof(MamiCharacterMain));
        }
    }
}
