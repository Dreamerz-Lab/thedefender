using UnityEngine.EventSystems;
using UnityEngine;

namespace Defender.Utility {
	public class ButtonEnterSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {
		public void OnPointerEnter(PointerEventData eventData) {
			Core.AudioManager.instance.PointerEnterAudio();
		}

		public void OnPointerClick(PointerEventData eventData) {
			Core.AudioManager.instance.PointerExitAudio();
		}
	}
}