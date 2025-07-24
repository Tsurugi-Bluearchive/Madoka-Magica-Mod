using MadokaMagica.Megumin.SkillStates;
using MadokaMagica.Megumin.SkillStates.BaseStates;


namespace MadokaMagica.Megumin.Content
{
    public static class MeguminStates
    {
        public static void Init()
        {

            Modules.Content.AddEntityState(typeof(BloodSacExplosion));

            Modules.Content.AddEntityState(typeof(CastMiniExplosion));

            Modules.Content.AddEntityState(typeof(SpawnGun));

            Modules.Content.AddEntityState(typeof(CastBigExplosion));

            Modules.Content.AddEntityState(typeof(MeguminCharacterMain));
        }
    }
}
