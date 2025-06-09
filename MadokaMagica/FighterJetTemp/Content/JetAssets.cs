﻿using RoR2;
using UnityEngine;
using MadokaMagica.Modules;
using RoR2.Projectile;
using MadokaMagica.MamiTamoe.Pickupables.MamiGun;

namespace MadokaMagica.Jet.Content
{
    public static class ZiluAssets
    {
        // particle effects
        public static GameObject swordSwingEffect;
        public static GameObject swordHitImpactEffect;

        public static GameObject bombExplosionEffect;

        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;

        //projectiles
        public static GameObject bombProjectilePrefab;

        public static GameObject MamiGun;

        public static GameObject MamiGunEffect;

        private static AssetBundle _assetBundle;

        public static MamiGun MamiGunScript;

        public static GameObject MamiGunTracer;

        public static void Init(AssetBundle assetBundle)
        {

            _assetBundle = assetBundle;

            swordHitSoundEvent = Modules.Content.CreateAndAddNetworkSoundEventDef("HenrySwordHit");

            CreateEffects();

            CreateProjectiles();

            CreateMamiGun();
        }

        #region effects
        private static void CreateEffects()
        {
            CreateBombExplosionEffect();

            swordSwingEffect = _assetBundle.LoadEffect("HenrySwordSwingEffect", true);
            swordHitImpactEffect = _assetBundle.LoadEffect("ImpactHenrySlash");
        }

        private static void CreateBombExplosionEffect()
        {
            bombExplosionEffect = _assetBundle.LoadEffect("BombExplosionEffect", "HenryBombExplosion");

            if (!bombExplosionEffect)
                return;

            var shakeEmitter = bombExplosionEffect.AddComponent<ShakeEmitter>();
            shakeEmitter.amplitudeTimeDecay = true;
            shakeEmitter.duration = 0.5f;
            shakeEmitter.radius = 200f;
            shakeEmitter.scaleShakeRadiusWithLocalScale = false;

            shakeEmitter.wave = new Wave
            {
                amplitude = 1f,
                frequency = 40f,
                cycleOffset = 0f
            };

        }
        #endregion effects

        #region projectiles
        private static void CreateProjectiles()
        {
            CreateBombProjectile();
            Modules.Content.AddProjectilePrefab(bombProjectilePrefab);
        }

        private static void CreateMamiGun()
        {

            MamiGun = _assetBundle.LoadAsset<GameObject>("MamiGun");
            MamiGun.AddComponent<MamiGun>();
            var worldCollision = MamiGun.transform.Find("WorldCollider").gameObject;
            worldCollision.AddComponent<MamiGunWorldCollider>();



            //MamiGunTracer = _assetBundle.LoadEffect("MamiGunTracer");
            //MamiGunTracer.AddComponent<BulletRotato>();
        }
        private static void CreateBombProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            bombProjectilePrefab = Asset.CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            Object.Destroy(bombProjectilePrefab.GetComponent<ProjectileImpactExplosion>());
            var bombImpactExplosion = bombProjectilePrefab.AddComponent<ProjectileImpactExplosion>();

            bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.blastDamageCoefficient = 1f;
            bombImpactExplosion.falloffModel = BlastAttack.FalloffModel.None;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = bombExplosionEffect;
            bombImpactExplosion.lifetimeExpiredSound = Modules.Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            var bombController = bombProjectilePrefab.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("HenryBombGhost") != null)
                bombController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("HenryBombGhost");

            bombController.startSound = "";
        }
        #endregion projectiles
    }
}
