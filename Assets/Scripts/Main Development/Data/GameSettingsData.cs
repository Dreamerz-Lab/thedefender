using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Defender.Data {
    public class GameSettingsData : MonoBehaviour {
        public static GameSettingsData instance;

		public const string PRIVACY_POLICY_ACCEPTED = "PPA";

		private bool _isMusicOn;
		public bool isMusicOn {
			get {
				return _isMusicOn;
			}
			set {
				_isMusicOn = value;
				audioListener.enabled = value;
			}
		}
		private AudioListener audioListener;

		public bool isHapticOn;

		private void Awake() {
			instance = this;

			if(audioListener == null)
				audioListener = GetComponent<AudioListener>();
		}
	}
}