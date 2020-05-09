using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

namespace Defender.Prototype
{
    public class ShieldHaptic : MonoBehaviour
    {
        [SerializeField] private bool LeftShield;
        [SerializeField] private ParticleSystem collideParticle;

        public UnityEngine.Events.UnityEvent OnCorrectHit;
        public UnityEngine.Events.UnityEvent OnWrongHit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("LM"))
            {
                if (LeftShield)
                {
                    OnCorrectHit.Invoke();
                    OVRInput.SetControllerVibration(0.1f, 1, OVRInput.Controller.LTouch);
                    Invoke("HapticLOff", 0.03f);
                }
                else
                {
                    OnWrongHit.Invoke();
                    OVRInput.SetControllerVibration(0.1f, 1, OVRInput.Controller.RTouch);
                    Invoke("HapticROff", 0.6f);
                }

                collideParticle.Play();
            }
            else if (other.CompareTag("M"))
            {
                if (LeftShield)
                {
                    OnWrongHit.Invoke();
                    OVRInput.SetControllerVibration(0.1f, 1, OVRInput.Controller.LTouch);
                    Invoke("HapticLOff", 0.6f);
                }
                else
                {
                    OnCorrectHit.Invoke();
                    OVRInput.SetControllerVibration(0.1f, 1, OVRInput.Controller.RTouch);
                    Invoke("HapticROff", 0.03f);
                }

                collideParticle.Play();
            }
        }

        void HapticLOff()
        {
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        }

        void HapticROff()
        {
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        }
    }
}