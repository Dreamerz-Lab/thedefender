using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Defender.UI {
	public class UIManager : MonoBehaviour {
		public static UIManager instance;

		[Space(6)]
		[Header("All Menu References")]
		[SerializeField] private PrivacyPolicyMenu privacyPolicy;
		[SerializeField] private MainMenu mainMenu;

		#region UNITYCALLBACK
		private void Awake() {
			instance = this;

			if(DOTween.instance == null)
				DOTween.Init();
		}

		private void Start() {
			//Open Privacy Policy Menu at first
			Invoke(nameof(InitUI), 2f);
		}
		#endregion

		#region PRIVATE_METHODS
		private void InitUI() {
			if (PlayerPrefs.HasKey(Data.GameSettingsData.PRIVACY_POLICY_ACCEPTED) && PlayerPrefs.GetInt(Data.GameSettingsData.PRIVACY_POLICY_ACCEPTED) == 1)
				mainMenu.OpenButtonGroup();
			else
				privacyPolicy.OpenPrivacyPolicy();
		}
		#endregion
	}
}