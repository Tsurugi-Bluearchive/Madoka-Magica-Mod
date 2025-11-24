using MadokaMagica.MamiTamoe;
using UnityEngine;
using RoR2;
using R2API;
using System.Runtime.CompilerServices;

namespace MadokaMagica.Crystal.Content
{
    public static class CrystalBuffs
    {
        // armor buff gained during roll
        public static DotController.DotIndex PrimaryOverCharge = DotAPI.RegisterDotDef(0.25f, 0.25f, DamageColorIndex.SuperBleed, OverchargePrimaryDebuff);
        public static DotController.DotIndex HealBuffIndex = DotAPI.RegisterDotDef(0.25f, 0.25f, DamageColorIndex.SuperBleed, HealBuff);
        public static BuffDef OverchargePrimaryDebuff;
        public static DamageType magicMissleDamage;
        public static BuffDef HealBuff;
        public static void Init(AssetBundle assetBundle)
        {
            OverchargePrimaryDebuff = Modules.Content.CreateAndAddBuff("PrimaryOverchargeDOT",
            CrystalAssets.P_Overcharge,
            Color.red,
            false,
            true);

            HealBuff = Modules.Content.CreateAndAddBuff("healBuff",
            CrystalAssets.P_Overcharge,
            Color.red,
            false,
            false);
        }
        

    }
}
