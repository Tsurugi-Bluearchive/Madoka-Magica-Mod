using MadokaMagica.Megumin.SkillStates;
using MadokaMagica.Megumin.SkillStates.BaseStates;


namespace MadokaMagica.Megumin.Content
{
    public static class MeguminStates
    {
        public static void Init()
        {

            Modules.Content.AddEntityState(typeof(Idontknowwhattonamethis));

            Modules.Content.AddEntityState(typeof(ExplosionCast));

            Modules.Content.AddEntityState(typeof(SpawnGun));

            Modules.Content.AddEntityState(typeof(OutofMana));

            Modules.Content.AddEntityState(typeof(MeguminCharacterMain));
        }
    }
}
