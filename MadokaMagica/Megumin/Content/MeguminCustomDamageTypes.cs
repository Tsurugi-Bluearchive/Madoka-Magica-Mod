using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using UnityEngine.Networking;
using static UnityEngine.SendMouseEvents;

namespace MadokaMagica.Megumin.Content
{

    internal class MeguminCustomDamageTypes
    {
        public static DamageAPI.ModdedDamageType HealorHurt = DamageAPI.ReserveDamageType();
    }
}
