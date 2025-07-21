using RoR2;
using UnityEngine;
using MadokaMagica.Modules;
using RoR2.Projectile;

namespace MadokaMagica.Megumin.Content
{
    public static class MegmuminAssets
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

        public static void Init(AssetBundle assetBundle)
        {

            _assetBundle = assetBundle;

            swordHitSoundEvent = Modules.Content.CreateAndAddNetworkSoundEventDef("HenrySwordHit");

            CreateEffects();

            CreateProjectiles();
        }

        #region effects
        private static void CreateEffects()
        {
        }
        #endregion effects

        #region projectiles
        private static void CreateProjectiles()
        {
            MeguminPrimaryExplosion = _assetBundle.LoadAsset<GameObject>("MeguminPrimaryProjectile");
        }
        #endregion projectiles
    }
}
