using MadokaMagica.Modules;
using Rewired.Utils;
using RoR2;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;
using EntityStates;
using MadokaMagica.MamiTamoe.SkillStates;

namespace MadokaMagica.MamiTamoe.Pickupables.MamiGun
{
    public class MamiGun : PickupableBase
    {
        public bool impactedworld;
        public Rigidbody thisBody;
        public MamiGunPassive MasterScript;
        private MamiGunWorldCollider MamiGunWorldCollider;
        private void FetchInitVars()
        {
            thisBody = GetComponent<Rigidbody>();
            thisBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            this.Pickup = this.gameObject;
            this.WorldCollider = this.gameObject.transform.Find("WorldCollider")?.gameObject;
        }
        private void InitThisPickupbale()
        {
            this.gameObject.layer = LayerIndex.pickups.intVal;
            this.PickupableName = this.gameObject.name;
            this.PickupType = "Mami Gun";
        }
        private float ClampedRange()
        {
            if (Random.Range(0, 1) > 0)
                return Random.Range(5, 10);
            return Random.Range(-10, -5);
        }
        //MamiGun.cs Code Start
        public void Awake()
        {
            FetchInitVars();
            InitThisPickupbale();
        }
        //MamiGun.cs Pickupable Base Configuration
        public override PickupableBase OnPickup(GameObject picker, long stacksize)
        {
            return base.OnPickup(picker, stacksize);
        }

        //MamiGun.cs OnDrop()
        public override void OnDrop(Vector3 dropPosition)
        {
            base.OnDrop(dropPosition + new Vector3(ClampedRange(), transform.position.y + 20f, ClampedRange()));
        }
        //MamiGun.cs Trigger Logic
        public void OnTriggerStay(Collider collision)
        {
            var mami = collision.gameObject.GetComponent<MamiGunPassive>();
            if (mami != null) collision.gameObject.GetComponent<MamiGunPassive>().PickingUpGun(this);
        }

        public override void AddMe(GameObject add)
        {
            base.AddMe(add);
        }

        public void FixedUpdate()
        {
            if (impactedworld && WorldCollider != null)
                Destroy(this.WorldCollider);
        }

        public override bool CanBePickedUpBy(GameObject picker)
        {
            return base.CanBePickedUpBy(picker);
        }
    }
}
