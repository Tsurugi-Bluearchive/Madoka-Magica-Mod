using EntityStates;
using EntityStates.VoidRaidCrab.Leg;
using IL.RoR2.Skills;
using MadokaMagica.MamiTamoe.BaseStates;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace MadokaMagica.Characters.UniversalBases
{
    public abstract class PickupableBase : MonoBehaviour
    {
        public GameObject Master;
        public BaseSkillState MasterScript;
        public GameObject WorldCollider;

        [SerializeField]
        public GameObject Pickup;

        public bool IsStackable;
        public long MaxStack;
        public string PickupType;
        public string PickupableName;
        public Transform pickupLocation;
        public TransportState transportState;

        protected List<GameObject> pickupAllocation = new List<GameObject>();
        public virtual void AddMe(GameObject add)
        {
            pickupAllocation.Add(add);
        }
        public virtual void SetParent(GameObject parent, BaseSkillState  parentsrcipt)
        {
            Master = parent;
            MasterScript = parentsrcipt;
        }

        public virtual void SetCollider(GameObject collider)
        {
            WorldCollider = collider;
        }

        public virtual void SetPickupable(GameObject pickup)
        {
            Pickup = pickup;
        }

        public virtual PickupableBase OnPickup(GameObject picker, long stacksize)
        {
            foreach (GameObject t in pickupAllocation)
            {
                if (IsStackable && stacksize < MaxStack && picker == t)
                {
                    return this;
                }
            }
            return null;
        }

        public virtual void OnDrop(Vector3 dropPosition)
        {
            gameObject.SetActive(true);
            transform.position = dropPosition;
        }
        public virtual bool CanBePickedUpBy(GameObject picker)
        {
            foreach (GameObject t in pickupAllocation)
            {
                if (!t.activeSelf)
                {
                    return false;
                }
                else if (t == picker)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
