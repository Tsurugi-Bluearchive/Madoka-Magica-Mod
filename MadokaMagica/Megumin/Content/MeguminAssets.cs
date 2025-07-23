using RoR2;
using UnityEngine;
using MadokaMagica.Modules;
using RoR2.Projectile;
using MadokaMagica.Megumin.MeguminComponents;

namespace MadokaMagica.Megumin.Content
{
    public static class MeguminAssets
    {
        // particle effects
        public static GameObject swordSwingEffect;
        public static GameObject swordHitImpactEffect;

        public static GameObject bombExplosionEffect;

        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;

        //projectiles
        public static GameObject bombProjectilePrefab;

        public static GameObject MeguminPrimaryExplosion;

        public static GameObject MeguminGunEffect;

        private static AssetBundle _assetBundle;

        public static GameObject MeguminGunTracer;

        public static Sprite P_Overcharge;

        public static GameObject magicMissle;

        public static DamageType magicMissleDamage;
        public static void Init(AssetBundle assetBundle)
        {

            _assetBundle = assetBundle;

            swordHitSoundEvent = Modules.Content.CreateAndAddNetworkSoundEventDef("HenrySwordHit");

            InitializeHooks();

            CreateEffects();

            CreateProjectiles();
        }
        #region effects
        private static void CreateEffects()
        {
            P_Overcharge = _assetBundle.LoadAsset<Sprite>("texPOvercharge");

        }
        #endregion effects

        #region projectiles
        private static void CreateProjectiles()
        {
            magicMissle = _assetBundle.LoadAsset<GameObject>("MagicMissile");
        }
        #endregion projectiles

        #region customDamage
        private static void InitializeHooks()
        {
            MeguminHooks.Init();
        }
        #endregion
    }
}
