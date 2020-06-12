using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Defender.UI {
	public class UIManager : MonoBehaviour {
		public static UIManager instace;

		//The Heavy Shield UI Count and Timer
		public Image HeavyShieldCount_Img;

		//Audio That will play On PointerEnter,PointerClick
		public AudioSource PointerAudio;
		//Audio That will play on any Menu Open/Close
		[SerializeField] private AudioSource MenuOpenAudio;
		[SerializeField] private AudioSource MenuCloseAudio;

		[Space(6)]
		[Header("All Menu References")]
		[SerializeField] private PrivacyPolicyMenu privacyPolicy;
		[SerializeField] private MainMenu mainMenu;

		#region UNITYCALLBACK
		private void Awake() {
			instace = this;

			if(DOTween.instance == null)
				DOTween.Init();
		}

		private void Start() {
#if UNITY_EDITOR
			if(MenuOpenAudio == null || MenuCloseAudio == null) {
				Debug.LogError("Menu Open/Close Audio is Null, It must be referenced");
				UnityEditor.EditorApplication.isPaused = true;
			}
#endif

			//Open Privacy Policy Menu at first
			Invoke(nameof(InitUI), 2f);
		}
		#endregion

		#region PUBLIC_METHODS
		/// <summary>
		/// This function plays Menu Open Audio with Pitch 1
		/// </summary>
		public void PlayMenuOpen() {
			MenuOpenAudio.Play();
		}

		/// <summary>
		/// This function plays Menu close Audio with Pitch 0.8
		/// </summary>
		public void PlayMenuClose() {
			MenuCloseAudio.Play();
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