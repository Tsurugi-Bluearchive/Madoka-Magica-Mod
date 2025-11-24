using MadokaMagica.Megumin.SkillStates;
using MadokaMagica.Megumin.SkillStates.BaseStates;


namespace MadokaMagica.Crystal.Content
{
    public static class CrystalStates
    {
        public static void Init()
        {

            Modules.Content.AddEntityState(typeof(BloodSacExplosion));

            Modules.Content.AddEntityState(typeof(CastMiniExplosion));

            Modules.Content.AddEntityState(typeof(SpawnGun));

            Modules.Content.AddEntityState(typeof(OutofMana));

            Modules.Content.AddEntityState(typeof(MeguminCharacterMain));
        }
    }
}
