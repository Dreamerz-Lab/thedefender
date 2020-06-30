using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Defender.Utility;
using DG.Tweening;

namespace Defender.UI {
	public class GameOverMenu : MonoBehaviour {
		//public OVRInput.Button PauseMenuButton;

		public bool isMenuOpen = false;

		[Header("GameOver Data")]
		[SerializeField] private TMPro.TMP_Text Score_TXT;
		[SerializeField] private TMPro.TMP_Text Wave_TXT;

		[Header("GameOver Menu BG")]
		[SerializeField] private CanvasGroup GameOverMenu_BG;
		[SerializeField] private Transform GameOverMenu_TRNS;
		[Space(6)]
		[SerializeField] private float GameOverMenu_Speed;
		[SerializeField] private AnimationCurve GameOverMenu_Ease;
		[Space(6)]
		[SerializeField] private FlareGlowSequence GameOverMenuFlare;

		[Space(6)]
		[SerializeField] private CanvasGroup ScorePanel_BG;

		[Header("GameOver Button Group")]
		[SerializeField] private float Btn_Panel_Speed = 0.3f;
		[SerializeField] private CanvasGroup Btn_Panel_CG;
		[SerializeField] private RectTransform Btn_Panel_TRNS;
		[SerializeField] private AnimationCurve Btn_Panel_Ease;

		private Vector2 Btn_Panel_Pos;

		[SerializeField] private UnityEvent OnCloseMenu;
		[SerializeField] private UnityEvent OnOpenMenu;

		private void Start() {
			//Cache Position
			Btn_Panel_Pos = Btn_Panel_TRNS.anchoredPosition;

			CanvasGroupUtils.CloseCanvasGroup(GameOverMenu_BG);
			CanvasGroupUtils.CloseCanvasGroup(ScorePanel_BG);
			CanvasGroupUtils.CloseCanvasGroup(Btn_Panel_CG);
		}

		public void ToggleMenu() {
			if (!isMenuOpen) {
				OpenMenu();
			} else {
				CloseMenu();
			}

			//isMenuOpen = !isMenuOpen;
		}

		public void OpenMenu() {
			isMenuOpen = true;
			CanvasGroupUtils.CloseCanvasGroup(GameOverMenu_BG);

			GameOverMenu_TRNS.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			GameOverMenu_TRNS.DOScale(1, GameOverMenu_Speed);

			//Fade In BG
			GameOverMenu_BG.DOFade(1f, GameOverMenu_Speed).SetEase(GameOverMenu_Ease).OnComplete(() => {
				OpenButtonGroup();

				//Fade In BG
				ScorePanel_BG.DOFade(1f, 0.15f).SetEase(GameOverMenu_Ease).OnComplete(() => {
					//Set the Window Interactable
					ScorePanel_BG.blocksRaycasts = true;
					ScorePanel_BG.interactable = true;
				});

				//Set the Window Interactable
				GameOverMenu_BG.blocksRaycasts = true;
				GameOverMenu_BG.interactable = true;

				//Button Pop Up
				//AcceptBTN_TRNS.DOScale(1f, MainMenu_Speed).SetEase(ButtonPopUp_Ease).SetDelay(0.2f);

				//Invoke On Complete Events
				OnOpenMenu.Invoke();

				GameOverMenuFlare.PlaySequence();

				//Menu Open Sound
				Core.AudioManager.instance.PlayMenuOpen();
			});
		}

		public void CloseMenu() {
			//Menu Close Sound
			Core.AudioManager.instance.PlayMenuClose();

			//Pause Flare Sequence
			GameOverMenuFlare.StopSequence();

			CloseButtonGroup();

			//Fade In BG
			ScorePanel_BG.DOFade(0f, GameOverMenu_Speed).SetEase(GameOverMenu_Ease).OnComplete(() => {
				//Set the Window Interactable
				ScorePanel_BG.blocksRaycasts = false;
				ScorePanel_BG.interactable = false;
			});

			//Fade In BG
			GameOverMenu_BG.DOFade(0f, GameOverMenu_Speed).SetEase(GameOverMenu_Ease).OnComplete(() => {
				GameOverMenu_BG.blocksRaycasts = false;
				GameOverMenu_BG.interactable = false;

				//Invoke On Close Complete Event
				OnCloseMenu.Invoke();
				isMenuOpen = false;
			});
		}

		public void UpdateScoreBoard(float score) {
			Score_TXT.text = score.ToString();
		}

		public void UpdateWaveBoard(int wave) {
			Wave_TXT.text = wave.ToString();
		}

		private void OpenButtonGroup() {
			CanvasGroupUtils.CloseCanvasGroup(Btn_Panel_CG);

			//Initialize Scale
			Btn_Panel_TRNS.anchoredPosition = new Vector2(Btn_Panel_Pos.x, Btn_Panel_Pos.y - 0.2f);
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

				//UIManager.instance.PlayMenuOpen();
				//OpenMainMenuBG();
			});
		}

		private void CloseButtonGroup() {
			//Fade In BG
			Btn_Panel_CG.DOFade(0f, Btn_Panel_Speed).SetEase(Btn_Panel_Ease).OnComplete(() => {
				Btn_Panel_CG.blocksRaycasts = false;
				Btn_Panel_CG.interactable = false;
			});
		}
	}
}