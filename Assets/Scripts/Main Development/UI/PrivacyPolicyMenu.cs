using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Defender.Utility;

#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

namespace Defender.UI {
    public class PrivacyPolicyMenu : MonoBehaviour {
		#region PRIVACY_POLICY_VARIABLES
		[Space(6)]
		[Header("Privacy Policy")]
		[SerializeField] private float PrivacyPolicy_Speed;
		[Space(6)]
		[SerializeField] private AnimationCurve PrivacyPolicy_Ease;
		[SerializeField] private CanvasGroup PrivacyPolicy_BG;
		[SerializeField] private Transform PrivacyPolicy_BG_TRNS;
		[SerializeField] private TMP_Text Accept_TXT;

		[Space(6)]
		[SerializeField] private Transform AcceptBTN_TRNS;
		[SerializeField] private AnimationCurve ButtonPopUp_Ease;

		[Space(6)]
		[SerializeField] private FlareGlowSequence Top_Flare_SQNC;
		[SerializeField] private FlareGlowSequence Bottom_Flare_SQNC;

		[Space(6)]
		[SerializeField] private UnityEvent OnPrivacyPolicy_Show;
		[SerializeField] private UnityEvent OnPrivacyPolicy_Close;
		#endregion

		#region UNITYCALLBACK
		private void Start() {
#if UNITY_EDITOR
			//Validation for Null Reference on Sequences
			if (Top_Flare_SQNC == null || Bottom_Flare_SQNC == null) {
				Debug.LogError("Flare Sequence References Can't be Null, Please Check UI Manager");
				UnityEditor.EditorApplication.isPaused = true;
			}
#endif

			//Deactivate BG
			PrivacyPolicy_BG.alpha = 0f;
			PrivacyPolicy_BG.blocksRaycasts = false;
			PrivacyPolicy_BG.interactable = false;
		}
		#endregion

		#region PUBLIC_FUNCTIONS
#if UNITY_EDITOR
		[Button("Open Privacy Policy | RUNTIME")]
#endif
		/// <summary>
		/// This functions opens the Privacy Policy Window when called
		/// </summary>
		public void OpenPrivacyPolicy() {
			Accept_TXT.SetMaterialDirty();

			//Deactivate BG
			PrivacyPolicy_BG.alpha = 0f;
			PrivacyPolicy_BG.blocksRaycasts = false;
			PrivacyPolicy_BG.interactable = false;

			//Initialize Scale
			AcceptBTN_TRNS.localScale = Vector3.zero;
			PrivacyPolicy_BG_TRNS.localScale = new Vector3(0.5f, 0.5f, 0.5f);

			PrivacyPolicy_BG_TRNS.DOScale(1, PrivacyPolicy_Speed);

			//Fade In BG
			PrivacyPolicy_BG.DOFade(1f, PrivacyPolicy_Speed).SetEase(PrivacyPolicy_Ease).OnComplete(() => {
				//Set the Window Interactable
				PrivacyPolicy_BG.blocksRaycasts = true;
				PrivacyPolicy_BG.interactable = true;

				//Button Pop Up
				AcceptBTN_TRNS.DOScale(1f, PrivacyPolicy_Speed).SetEase(ButtonPopUp_Ease).SetDelay(0.2f);

				//Invoke On Complete Events
				OnPrivacyPolicy_Show.Invoke();

				Top_Flare_SQNC.PlaySequence();
				Bottom_Flare_SQNC.PlaySequence();

				//Menu Open Sound
				UIManager.instace.PlayMenuOpen();
			});
		}

#if UNITY_EDITOR
		[Button("Close Privacy Policy | RUNTIME")]
#endif
		/// <summary>
		/// This function closes the Privacy Policy Window
		/// </summary>
		public void ClosePrivacyPolicy() {
			//Menu Close Sound
			UIManager.instace.PlayMenuClose();

			//Pause Flare Sequence
			Top_Flare_SQNC.StopSequence();
			Bottom_Flare_SQNC.StopSequence();

			//Fade In BG
			PrivacyPolicy_BG.DOFade(0f, PrivacyPolicy_Speed).SetEase(PrivacyPolicy_Ease).OnComplete(() => {
				PrivacyPolicy_BG.blocksRaycasts = false;
				PrivacyPolicy_BG.interactable = false;

				PlayerPrefs.SetInt(Data.GameSettingsData.PRIVACY_POLICY_ACCEPTED, 1);

				//Invoke On Close Complete Event
				OnPrivacyPolicy_Close.Invoke();
			});
		}
		#endregion
	}
}
