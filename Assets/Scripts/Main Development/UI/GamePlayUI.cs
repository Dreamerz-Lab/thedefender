using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Defender.UI {
    public class GamePlayUI : MonoBehaviour {
        public static GamePlayUI instance;

        //The Heavy Shield UI Count and Timer
        public Image HeavyShieldCount_Img;
		public Image PlayerHealthCount_Img;

		[SerializeField] private GameObject UILaser;

		public PauseMenu pauseMenu;
		public GameOverMenu gameOverMenu;

		#region UNITYCALLBACK
		private void Awake() {
			instance = this;
		}

		private void Start() {
			UILaser.SetActive(false);
		}

		private void Update() {
#if UNITY_EDITOR
			if (Input.GetKeyDown(KeyCode.Escape)) {
				Core.GameManager.instance.ToggleGamePause();
			}

			if (Input.GetKeyDown(KeyCode.Space)) {
				Core.GameManager.instance.GameEnd();
			}
#else
			if (OVRInput.GetDown(pauseMenu.PauseMenuButton) && !gameOverMenu.isMenuOpen) {
				Core.GameManager.instance.ToggleGamePause();
			}
#endif
		}
		#endregion

		public void ToggleLaserUI() {
			UILaser.SetActive(!UILaser.activeSelf);
		}

		public void ToggleLaserUI(bool activeSelf) {
			UILaser.SetActive(activeSelf);
		}
	}
}