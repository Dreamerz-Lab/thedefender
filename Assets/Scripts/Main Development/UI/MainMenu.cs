using System.Runtime.CompilerServices;
using UnityEngine;
using DG.Tweening;
using Defender.Utility;

#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

namespace Defender.UI {
    public class MainMenu : MonoBehaviour {
		#region BTN_PANEL_VARIABLES
		[Space(6)]
		[Header("Button Panel")]
		[SerializeField] private float Btn_Panel_Speed = 0.3f;
		[SerializeField] private CanvasGroup Btn_Panel_CG;
		[SerializeField] private RectTransform Btn_Panel_TRNS;
		[SerializeField] private AnimationCurve Btn_Panel_Ease;

		private Vector2 Btn_Panel_Pos;
		#endregion

		#region BG_PANEL_VARIABLES
		[Space(6)]
		[Header("Background Panel")]
		[SerializeField] private float MainMenu_Speed = 0.3f;
		[SerializeField] private CanvasGroup MainMenu_BG;
		[SerializeField] private Transform MainMenu_TRNS;
		[SerializeField] private AnimationCurve MainMenu_Ease;
		private bool isMainPanelOpen = false;

		[Space(6)]
		[SerializeField] private FlareGlowSequence MainMenuFlare;
		#endregion

		#region MENU_ITEM_VARIABLES
		[Space(6)]
		[Header("Other Menu Sections")]
		[SerializeField] private CanvasGroup HelpSection_CG;
		[SerializeField] private CanvasGroup SettingsSection_CG;
		[SerializeField] private CanvasGroup CreditsSection_CG;
		#endregion

		#region UNITY_CALLBACKS
		private void Start() {
#if UNITY_EDITOR
			if (MainMenuFlare == null) {
				Debug.LogError("Flare Sequence is Null, It must be referenced");
				UnityEditor.EditorApplication.isPaused = true;
			}
#endif
			//Cache Position
			Btn_Panel_Pos = Btn_Panel_TRNS.anchoredPosition;

			//Deactivate Button Group
			CanvasGroupUtils.CloseCanvasGroup(Btn_Panel_CG);
			//Deactivate BG
			CanvasGroupUtils.CloseCanvasGroup(MainMenu_BG);
			//Deactivates Other Sections
			CanvasGroupUtils.CloseCanvasGroup(HelpSection_CG);
			CanvasGroupUtils.CloseCanvasGroup(SettingsSection_CG);
			CanvasGroupUtils.CloseCanvasGroup(CreditsSection_CG);
		}
		#endregion

		#region PUBLIC_FUNCTIONS
		/// <summary>
		/// Shows the Help Section on Main Panel
		/// </summary>
		public void HelpButton() {
			if (!isMainPanelOpen) {
				OpenMainMenuBG();
				Invoke(nameof(HelpButton), MainMenu_Speed);
				return;
			}

			CanvasGroupUtils.OpenCanvasGroup(HelpSection_CG);
			CanvasGroupUtils.CloseCanvasGroup(SettingsSection_CG);
			CanvasGroupUtils.CloseCanvasGroup(CreditsSection_CG);
		}

		/// <summary>
		/// Shows the Settings Section on Main Panel
		/// </summary>
		public void SettingsButton() {
			if (!isMainPanelOpen) {
				OpenMainMenuBG();
				Invoke(nameof(SettingsButton), MainMenu_Speed);
				return;
			}

			CanvasGroupUtils.CloseCanvasGroup(HelpSection_CG);
			CanvasGroupUtils.OpenCanvasGroup(SettingsSection_CG);
			CanvasGroupUtils.CloseCanvasGroup(CreditsSection_CG);
		}

		/// <summary>
		/// Shows the Credits Section on Main Panel
		/// </summary>
		public void CreditsButton() {
			if (!isMainPanelOpen) {
				OpenMainMenuBG();
				Invoke(nameof(CreditsButton), MainMenu_Speed);
				return;
			}

			CanvasGroupUtils.CloseCanvasGroup(HelpSection_CG);
			CanvasGroupUtils.CloseCanvasGroup(SettingsSection_CG);
			CanvasGroupUtils.OpenCanvasGroup(CreditsSection_CG);
		}

#if UNITY_EDITOR
		[Button("Open MAIN MENU | RUNTIME")]
#endif
		/// <summary>
		/// This function opens the Main Button Group Panel
		/// </summary>
		public void OpenButtonGroup() {
			//Deactivate BG
			Btn_Panel_CG.alpha = 0f;
			Btn_Panel_CG.blocksRaycasts = false;
			Btn_Panel_CG.interactable = false;

			//Initialize Scale
			Btn_Panel_TRNS.anchoredPosition = new Vector2(Btn_Panel_Pos.x, Btn_Panel_Pos.y - 0.3f);
			Btn_Panel_TRNS.DOAnchorPosY(Btn_Panel_Pos.y, Btn_Panel_Speed);

			//Fade In BG
			Btn_Panel_CG.DOFade(1f, Btn_Panel_Speed).SetEase(Btn_Panel_Ease).OnComplete(() => {
				//Set the Window Interactable
				Btn_Panel_CG.blocksRaycasts = true;
				Btn_Panel_CG.interactable = true;

				//Button Pop Up
				//AcceptBTN_TRNS.DOScale(1f, MainMenu_Speed).SetEase(ButtonPopUp_Ease).SetDelay(0.2f);

				//Invoke On Complete Events
				//OnPrivacyPolicy_Show.Invoke();

				Core.AudioManager.instance.PlayMenuOpen();
				//OpenMainMenuBG();
			});
		}

#if UNITY_EDITOR
		[Button("Close MAIN MENU | RUNTIME")]
#endif
		/// <summary>
		/// This Function closes the BG with Animation
		/// </summary>
		public void CloseMainMenuBG() {
			//Menu Close Sound
			Core.AudioManager.instance.PlayMenuClose();

			//Pause Flare Sequence
			MainMenuFlare.StopSequence();

			//Deactivates Other Sections
			CanvasGroupUtils.CloseCanvasGroup(HelpSection_CG);
			CanvasGroupUtils.CloseCanvasGroup(SettingsSection_CG);
			CanvasGroupUtils.CloseCanvasGroup(CreditsSection_CG);

			CloseButtonGroup();

			//Fade In BG
			MainMenu_BG.DOFade(0f, MainMenu_Speed).SetEase(MainMenu_Ease).OnComplete(() => {
				MainMenu_BG.blocksRaycasts = false;
				MainMenu_BG.interactable = false;

				//Invoke On Close Complete Event
				//OnPrivacyPolicy_Close.Invoke();
				isMainPanelOpen = false;
			});
		}

		/// <summary>
		/// This function opens the BG with Animation
		/// </summary>
		public void OpenMainMenuBG() {
			if (!isMainPanelOpen) {
				isMainPanelOpen = true;

				//Deactivate BG
				MainMenu_BG.alpha = 0f;
				MainMenu_BG.blocksRaycasts = false;
				MainMenu_BG.interactable = false;

				//Initialize Scale
				MainMenu_TRNS.localScale = new Vector3(0.5f, 0.5f, 0.5f);
				MainMenu_TRNS.DOScale(1, MainMenu_Speed);

				//Fade In BG
				MainMenu_BG.DOFade(1f, MainMenu_Speed).SetEase(MainMenu_Ease).OnComplete(() => {
					//Set the Window Interactable
					MainMenu_BG.blocksRaycasts = true;
					MainMenu_BG.interactable = true;

					//Button Pop Up
					//AcceptBTN_TRNS.DOScale(1f, MainMenu_Speed).SetEase(ButtonPopUp_Ease).SetDelay(0.2f);

					//Invoke On Complete Events
					//OnPrivacyPolicy_Show.Invoke();

					MainMenuFlare.PlaySequence();

					//Menu Open Sound
					Core.AudioManager.instance.PlayMenuOpen();
				});
			}
		}
		#endregion

		#region PRIVATE_FUNCTIONS
		/// <summary>
		/// This function cl0se
		/// </summary>
		private void CloseButtonGroup() {
			//Fade In BG
			Btn_Panel_CG.DOFade(0f, Btn_Panel_Speed).SetEase(Btn_Panel_Ease).OnComplete(() => {
				Btn_Panel_CG.blocksRaycasts = false;
				Btn_Panel_CG.interactable = false;
			});
		}
		#endregion
	}
}