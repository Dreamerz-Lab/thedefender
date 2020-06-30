using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Defender.Core {
    /// <summary>
	/// When a Missile or anything hits Player Body
	/// </summary>
    public class PlayerBody : MonoBehaviour {
		[SerializeField] private LayerMask ObstacleLayer;
		/// <summary>
		/// OnTrigger 
		/// </summary>
		/// <param name="other"></param>
		private void OnTriggerEnter(Collider other) {
			Utility.HapticUtility.instance.HapticOn(OVRInput.Controller.LTouch, 0.2f, 1, 0.06f);
			Utility.HapticUtility.instance.HapticOn(OVRInput.Controller.RTouch, 0.2f, 1, 0.06f);

			if (!other.CompareTag("OBS")) {
				GameManager.instance.TakeDamage();
				Destroy(other.gameObject);
			} else {
				GameManager.instance.TakeDamage(2f);
			}
		}
	}
}