using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Defender.Core {
	public class AudioManager : MonoBehaviour {
		public static AudioManager instance;
		//Audio That will play on any Menu Open/Close
		[SerializeField] private AudioSource PointerAudio;
		[SerializeField] private AudioSource MenuOpenAudio;
		[SerializeField] private AudioSource MenuCloseAudio;

		private void Awake() {
#if UNITY_EDITOR
			if (MenuOpenAudio == null || MenuCloseAudio == null) {
				Debug.LogError("Menu Open/Close Audio is Null, It must be referenced");
				UnityEditor.EditorApplication.isPaused = true;
			}
#endif

			if (instance == null) {
				instance = this;
				DontDestroyOnLoad(this);

				PointerAudio.loop = false;
				PointerAudio.playOnAwake = false;
				return;
			}

			if (instance != null) {
				Destroy(this);
				return;
			}
		}

		#region PUBLIC_METHODS
		public void PointerEnterAudio() {
			if (!Data.GameSettingsData.instance.isMusicOn)
				return;

			PointerAudio.volume = 0.15f;
			PointerAudio.pitch = 1f;
			PointerAudio.Play();
		}

		public void PointerExitAudio() {
			if (!Data.GameSettingsData.instance.isMusicOn)
				return;

			PointerAudio.volume = 0.3f;
			PointerAudio.pitch = 1.3f;
			PointerAudio.Play();
		}

		/// <summary>
		/// This function plays Menu Open Audio with Pitch 1
		/// </summary>
		public void PlayMenuOpen() {
			if (!Data.GameSettingsData.instance.isMusicOn)
				return;

			MenuOpenAudio.Play();
		}

		/// <summary>
		/// This function plays Menu close Audio with Pitch 0.8
		/// </summary>
		public void PlayMenuClose() {
			if (!Data.GameSettingsData.instance.isMusicOn)
				return;

			MenuCloseAudio.Play();
		}
		#endregion
	}
}