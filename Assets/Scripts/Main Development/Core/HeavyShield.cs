using UnityEngine;

namespace Defender.Core {
    /// <summary>
    /// Once a Heavy Shield has been activated,
    /// the mesh and Lightning Particle will be handled by this script
    /// </summary>
    public class HeavyShield : MonoBehaviour {
        //Player Shields
        [Header("References For Center Placement")]
        [SerializeField] private Transform[] GeneralShields;
        [InspectorName("OVR Rig")][SerializeField] private Transform OVRrig;

        [Space(6)]
        [Header("Heavy Shield")]
		//The Heavy Shield
        [SerializeField] private Transform HeavyShieldMesh;
		//UI Image Bar for Shield Count and Timer
        private UnityEngine.UI.Image ShieldTimer_Img;
		//timer for Deactivating Shield
        private float DisableTimer;

		#region UNITYCALLBACKS
		/// <summary>
		/// UNITY START CALLBACK
		/// </summary>
		private void Start() {
            ShieldTimer_Img = UI.UIManager.instace.HeavyShieldCount_Img;
        }

		/// <summary>
		/// Invokes when the Heavy Shield is activated
		/// Starts the Timer for disabling the Shield
		/// </summary>
		private void OnEnable() {
            DisableTimer = GameManager.instance.HeavyShieldTimer;

            //Invokes the Disable Logic
            Invoke(nameof(ShieldDisableStateUpdate), DisableTimer);
        }

        /// <summary>
        /// Invokes when anything Collides with the Shield
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other) {
            //WaveManager.PutBackToPool();
            Destroy(other.gameObject);
        }

        /// <summary>
        /// Update the HeavyShieldMesh to stay at the Center
        /// </summary>
        private void Update() {
            //Center Position
            PositionShieldAtCenter();

			//Updates UI Shield Timer
            ShieldTimer_Img.fillAmount -= Time.deltaTime / DisableTimer * 0.25f;
        }
		#endregion

		#region PrivateFunctions
		/// <summary>
		/// Sets the Shield at the Center of two Controllers
		/// </summary>
		private void PositionShieldAtCenter() {
            Vector3 _center = new Vector3();

			//Iterates through the Shields and finds the Center
            for (int i = 0; i < GeneralShields.Length; i++) {
                _center += GeneralShields[i].position;
            }

            _center /= 2f;

			//Updates the Shield Position and Rotation
            HeavyShieldMesh.SetPositionAndRotation(_center, Quaternion.Euler(new Vector3(0f, OVRrig.eulerAngles.y, 0f)));
        }

		/// <summary>
		/// Disables the Shield, and Updates the Manager and UI
		/// </summary>
		private void ShieldDisableStateUpdate() {
            //Update Shield Active State
            GameManager.instance.isHeavyShieldActive = false;
			//Decreases Shield Count
            GameManager.instance.HeavyShieldCount--;

            //Update Mesh Active State
            HeavyShieldMesh.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
		#endregion
	}
}