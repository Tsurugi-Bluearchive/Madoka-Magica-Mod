using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using UnityEngine;

namespace HenryMod.Characters.Survivors.Henry.SkillStates
{
    public class JetMainState : GenericCharacterMain
    {
        private float jetSpeed = 10f;
        public bool afterBurning;
        private Vector3 momentum;
        private Vector3 momentumVector;
        private float drag = 10f;
        private float redirectionAmiplitude = 2f;
        private Vector3 stallVelocity;
        private float cobraAuthority;
        private Vector3 controlAuthority = new Vector3(30, 30, 30);
        private float afterBurnerAmplitude = 2f;
        private float engineHeat;
        private Ray lookVector => GetAimRay();
        private Vector3 ClampToAuthority(float x, float y, float z) {
            return new Vector3(Mathf.Clamp(x, -controlAuthority.x, controlAuthority.x), 
                Mathf.Clamp(y, -controlAuthority.y, controlAuthority.y),
                Mathf.Clamp(z, controlAuthority.y, controlAuthority.z)); 
        }
            
        private void DetectAfterburning()
        {
            if (inputBank.jump.justPressed)
            {
                afterBurning = true;
            }
            else if (inputBank.jump.justReleased)
            {
                afterBurning = false;
            }
        }
        //JetMainState.cs Code Start
        public override void FixedUpdate()
        {
            DetectAfterburning();

            //JetMainState.cs Afterburning
            if (afterBurning && engineHeat < 5f)
            {
                engineHeat += Time.fixedDeltaTime;
            }
            else if (!afterBurning)
            {
                engineHeat -= Time.fixedDeltaTime / 2;
            }

            Vector3 lookAngle = lookVector.GetPoint(jetSpeed);
            Vector3 direction = (lookAngle - moveVector).normalized;
            direction = ClampToAuthority(direction.x, direction.y, direction.z);
            Quaternion rotation = Quaternion.LookRotation(direction);
            
            characterBody.characterMotor.UpdateRotation(ref rotation, 0.4f);
            momentum = characterBody.characterMotor.velocity;
            momentum = momentum + (jetSpeed * Vector3.forward);
            characterMotor.velocity = momentum;
        }
    }
}