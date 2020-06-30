using UnityEngine;

namespace Defender.Core {
    /// <summary>
    /// Manages the Activation of Heavy Shield
    /// Checks for the Activation Gesture, and Activates the Heavy Shield
    /// </summary>
    public class ShieldFusion : MonoBehaviour {
		#region Variables
		//The buttons for the HeavyShield Activation
		[Header("Fusion Button")]
        [SerializeField] private OVRInput.Button[] FusionButtons;

#if UNITY_EDITOR
		//Buttons on Editor for replacement of Oculus Buttons
        [SerializeField] private KeyCode[] Editor_FusionButtons;
#endif

        //The Components to be enabled for Heavy Shield
        [Header("Power Shield")]
        [SerializeField] private GameObject ShieldParticle;
        [SerializeField] private GameObject HeavyShield;

        //The Active Status of the Heavy Shield
        private bool isHeavyShieldActive = false;
		#endregion

		#region PrivateFunctions
		/// <summary>
		/// Invokes when the Two Controllers start to collide with each other
		/// </summary>
		/// <param name="other"></param>
		private void OnTriggerEnter(Collider other) {
            isHeavyShieldActive = GameManager.instance.isHeavyShieldActive;
            CheckForActivation();
        }

        /// <summary>
        /// Invokes when the Two Controllers are colliding with each other
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerStay(Collider other) {
            CheckForActivation();
        }

        /// <summary>
        /// This function checks if the Heavy Shield can be activated
        /// </summary>
        private void CheckForActivation() {
            //If the Heavy Shield is not activated, Activation Check function is called
            if (isHeavyShieldActive)
                return;

            bool _triggerFusion = true;

            for (byte i = 0; i < FusionButtons.Length; i++) {
#if UNITY_EDITOR
                if (!Input.GetKey(Editor_FusionButtons[i]))
                    _triggerFusion = false;
#else
                if (!OVRInput.Get(FusionButtons[i]))
                    _triggerFusion = false;
#endif
            }

            if (_triggerFusion) {
                ActivateHeavyShield();
            }
        }

        /// <summary>
        /// Activates the Heavy Shield
        /// </summary>
        private void ActivateHeavyShield() {
            Utility.HapticUtility.instance.HapticOn(OVRInput.Controller.LTouch, 0.2f, 1, 0.2f);
            Utility.HapticUtility.instance.HapticOn(OVRInput.Controller.RTouch, 0.2f, 1, 0.2f);

            GameManager.instance.isHeavyShieldActive = isHeavyShieldActive = true;

            ShieldParticle.SetActive(true);
            //HeavyShield.SetActive(true);
        }
        #endregion
    }
}