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
using EntityStates;
using MadokaMagica.MamiTamoe.SkillStates;

namespace MadokaMagica.MamiTamoe.Pickupables
{
    public class MamiGun : PickupableBase
    {
        public bool impactedworld;
        public Rigidbody thisBody;
        public MamiGunPassive MasterScript;
        private MamiGunWorldCollider MamiGunWorldCollider;
        public void Awake()
        {
            thisBody = GetComponent<Rigidbody>();
            thisBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
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
            base.OnDrop(dropPosition + new Vector3(Random.Range(-5f, 5f) , transform.position.y + 20f, Random.Range(-7f, 7f)));
        }

        public void OnTriggerStay(Collider collision)
        {
            var mami = collision.gameObject.GetComponent<MamiGunPassive>();
            if (mami != null)
            {
                collision.gameObject.GetComponent<MamiGunPassive>().PickingUpGun(this);
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
            }
        }

        public override bool CanBePickedUpBy(GameObject picker)
        {
            return base.CanBePickedUpBy(picker);
        }
    }
}
