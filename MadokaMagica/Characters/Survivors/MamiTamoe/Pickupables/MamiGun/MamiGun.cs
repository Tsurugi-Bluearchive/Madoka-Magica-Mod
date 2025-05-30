using MadokaMagica;
using MadokaMagica.Modules;
using MadokaMagica.Characters.UniversalBases;
using MadokaMagica.MamiTamoe.BaseStates;
using Rewired.Utils;
using RoR2;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace MadokaMagica.MamiTamoe.Pickupables
{
    public class MamiGun : PickupableBase
    {
        public bool impactedworld;
        Rigidbody thisBody;
        private MamiGunWorldCollider MamiGunWorldCollider;
        public void Awake()
        {
            thisBody = GetComponent<Rigidbody>();
            this.MasterScript = this.gameObject.GetComponent<MasterMamiSkillStates>();
            this.Pickup = this.gameObject;

            this.WorldCollider = this.gameObject.transform.Find("WorldCollider")?.gameObject;
            if (this.WorldCollider != null)
            {
                this.WorldCollider.gameObject.layer = LayerIndex.defaultLayer.intVal;
                MamiGunWorldCollider = WorldCollider.gameObject.transform.GetComponent<MamiGunWorldCollider>();
                MamiGunWorldCollider.MamiGun = this;
            }
            else
            {

                Log.Error("Did you attach WorldCollider to MamiGun?");
            }

            thisBody = Pickup.GetComponent<Rigidbody>();
            this.gameObject.layer = LayerIndex.pickups.intVal;
            this.PickupableName = this.gameObject.name;
            this.PickupType = "Mami Gun";
        }
        public override PickupableBase OnPickup(GameObject picker, long stacksize)
        {
            return base.OnPickup(picker, stacksize);
        }

        public override void OnDrop(Vector3 dropPosition)
        {
            base.OnDrop(dropPosition + new Vector3(Random.Range(-10f, 10f), transform.position.y + 20f, Random.Range(-7f, 7f)));
        }

        public void OnTriggerStay(Collider collision)
        {
            MasterMamiSkillStates Mami = collision.gameObject.GetComponent<MasterMamiSkillStates>();
            if (Mami != null && Mami.gunCount <= Mami.gunMax)
            {
                Mami.gunCount++;
            }
        }

        public override void AddMe(GameObject add)
        {
            base.AddMe(add);
        }

        public void FixedUpdate()
        {
            if (impactedworld && WorldCollider != null)
            {
                Destroy(this.WorldCollider);
                Log.Debug("Destroyed worldcollider");
            }
        }

        public override bool CanBePickedUpBy(GameObject picker)
        {
            return base.CanBePickedUpBy(picker);

        }
    }
}
