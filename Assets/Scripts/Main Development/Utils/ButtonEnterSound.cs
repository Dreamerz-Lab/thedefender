using UnityEngine.EventSystems;
using UnityEngine;

namespace Defender.Utility {
	public class ButtonEnterSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {
		private AudioSource PointerEnterAudio;

		private void Start() {
			PointerEnterAudio = UI.UIManager.instace.PointerAudio;

#if UNITY_EDITOR
			if (PointerEnterAudio == null)
				Debug.LogError("UI manager requires to have a Pointer Audio");
#endif

			PointerEnterAudio.loop = false;
			PointerEnterAudio.playOnAwake = false;
		}

		public void OnPointerEnter(PointerEventData eventData) {
			PointerEnterAudio.volume = 0.15f;
			PointerEnterAudio.pitch = 1f;
			PointerEnterAudio.Play();
		}

		public void OnPointerClick(PointerEventData eventData) {
			PointerEnterAudio.volume = 0.3f;
			PointerEnterAudio.pitch = 1.3f;
			PointerEnterAudio.Play();
		}
	}
}