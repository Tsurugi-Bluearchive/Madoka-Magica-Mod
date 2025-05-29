using HenryMod;
using HenryMod.Modules;
using MadokaMagica.Characters.UniversalBases;
using MadokaMagica.MamiTamoe.BaseStates;
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
                this.WorldCollider.layer = LayerIndex.world.intVal;
                MamiGunWorldCollider = WorldCollider.gameObject.transform.GetComponent<MamiGunWorldCollider>();
                MamiGunWorldCollider.MamiGun = this;
            }
            else
            {
                Log.Error("Did you attach WorldCollider to MamiGun?");
            }

            thisBody = Pickup.GetComponent<Rigidbody>();
            this.gameObject.layer = LayerIndex.playerBody.intVal;
            this.PickupableName = this.gameObject.name;
            this.PickupType = "Mami Gun";
            this.gameObject.SetActive(false);
        }
        public override PickupableBase OnPickup(GameObject picker, long stacksize)
        {
            return base.OnPickup(picker, stacksize);
        }

        public override void OnDrop(Vector3 dropPosition)
        {
            base.OnDrop(dropPosition + (transform.up * 10f));
        }

        public void OnTriggerStay(Collider collision)
        {
            MasterMamiSkillStates Mami = collision.gameObject.GetComponent<MasterMamiSkillStates>();
            if (Mami != null && Mami.gunCount <= Mami.gunMax)
            {
                Mami.gunCount++;
                this.gameObject.SetActive(false);
            }
        }

        public override void AddMe(GameObject add)
        {
            base.AddMe(add);
        }

        public void FixedUpdate()
        {
            if (!impactedworld)
            {
                thisBody.AddForce(Vector3.down * 0.1f);;
            }
        }

        public override bool CanBePickedUpBy(GameObject picker)
        {
            return base.CanBePickedUpBy(picker);
        }
    }
}
