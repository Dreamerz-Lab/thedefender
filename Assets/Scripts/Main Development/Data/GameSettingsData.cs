using UnityEngine;

namespace Defender.Data {
    public class GameSettingsData : MonoBehaviour {
        public static GameSettingsData instance;

		public const string PRIVACY_POLICY_ACCEPTED = "PPA";
		public const string MUSIC_SETTINGS = "MS";
		public const string HAPTIC_SETTINGS = "HS";

		[SerializeField] private bool _isMusicOn;
		public bool isMusicOn {
			get {
				return _isMusicOn;
			}
		}
		private AudioListener audioListener;

		[SerializeField] private bool _isHapticOn;
		public bool isHapticOn {
			get {
				return _isHapticOn;
			}	
		}

		private void Awake() {
			if (instance != null) {
				DestroyImmediate(gameObject);
				return;
			}

			instance = this;
			DontDestroyOnLoad(this);

			if (audioListener == null)
				audioListener = Camera.main.GetComponent<AudioListener>();
		}

		private void Start() {
			if (!PlayerPrefs.HasKey(MUSIC_SETTINGS))
				PlayerPrefs.SetInt(MUSIC_SETTINGS, 1);

			if (!PlayerPrefs.HasKey(HAPTIC_SETTINGS))
				PlayerPrefs.SetInt(HAPTIC_SETTINGS, 1);

			_isMusicOn = PlayerPrefs.GetInt(MUSIC_SETTINGS, 1) == 1 ?  true : false;
			_isHapticOn = PlayerPrefs.GetInt(HAPTIC_SETTINGS, 1) == 1 ? true : false;
		}

		public void ToggleMusicSettings(bool isToggle) {
			_isMusicOn = isToggle;

			if (isToggle) {
				PlayerPrefs.SetInt(MUSIC_SETTINGS, 1);
			} else {
				PlayerPrefs.SetInt(MUSIC_SETTINGS, 0);
			}
		}

		public void ToggleHapticSettings(bool isToggle) {
			_isHapticOn = isToggle;

			if (isToggle) {
				PlayerPrefs.SetInt(HAPTIC_SETTINGS, 1);
			} else {
				PlayerPrefs.SetInt(HAPTIC_SETTINGS, 0);
				
			}
		}
	}
}