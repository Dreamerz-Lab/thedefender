using UnityEngine;

namespace Defender.Utility {
	public class HapticUtility : MonoBehaviour {
		public static HapticUtility instance;

		private void Awake() {
			if (instance == null) {
				instance = this;
				DontDestroyOnLoad(this);
				return;
			}

			if (instance != null)
				Destroy(this.gameObject);
		}

		public void HapticOn(OVRInput.Controller controller, float frequency = 0.1f, float amplitude = 1f, float duration = 0.03f) {
			if (!Data.GameSettingsData.instance.isHapticOn)
				return;

			OVRInput.SetControllerVibration(frequency, amplitude, controller);

			if (controller == OVRInput.Controller.LTouch) {
				Invoke(nameof(HapticLOff), duration);
			} else if (controller == OVRInput.Controller.RTouch) {
				Invoke(nameof(HapticROff), duration);
			}
		}

		void HapticLOff() {
			OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
		}

		void HapticROff() {
			OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
		}
	}
}