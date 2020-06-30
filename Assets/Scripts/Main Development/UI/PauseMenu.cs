using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Defender.Utility;

namespace Defender.UI {
	public class PauseMenu : MonoBehaviour {
		public OVRInput.Button PauseMenuButton;

		public bool isMenuOpen = false;

		[Header("Pause Menu BG")]
		[SerializeField] private CanvasGroup PauseMenu_BG;
		[SerializeField] private Transform PauseMenu_TRNS;
		[Space(6)]
		[SerializeField] private float PauseMenu_Speed;
		[SerializeField] private AnimationCurve PauseMenu_Ease;
		[Space(6)]
		[SerializeField] private FlareGlowSequence PauseMenuFlare;

		[Header("Pause Menu Button Group")]
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

			CanvasGroupUtils.CloseCanvasGroup(PauseMenu_BG);
			CanvasGroupUtils.CloseCanvasGroup(Btn_Panel_CG);
		}

		public void TogglePauseMenu() {
			if (!isMenuOpen) {
				OpenPauseMenu();
			} else {
				ClosePauseMenu();
			}

			//isMenuOpen = !isMenuOpen;
		}

		public void OpenPauseMenu() {
			isMenuOpen = true;
			CanvasGroupUtils.CloseCanvasGroup(PauseMenu_BG);

			PauseMenu_TRNS.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			PauseMenu_TRNS.DOScale(1, PauseMenu_Speed);

			//Fade In BG
			PauseMenu_BG.DOFade(1f, PauseMenu_Speed).SetEase(PauseMenu_Ease).OnComplete(() => {
				OpenButtonGroup();

				//Set the Window Interactable
				PauseMenu_BG.blocksRaycasts = true;
				PauseMenu_BG.interactable = true;

				//Invoke On Complete Events
				OnOpenMenu.Invoke();

				PauseMenuFlare.PlaySequence();

				//Menu Open Sound
				Core.AudioManager.instance.PlayMenuOpen();
			});
		}

		public void ClosePauseMenu() {
			//Menu Close Sound
			Core.AudioManager.instance.PlayMenuClose();

			//Pause Flare Sequence
			PauseMenuFlare.StopSequence();

			CloseButtonGroup();

			//Fade In BG
			PauseMenu_BG.DOFade(0f, PauseMenu_Speed).SetEase(PauseMenu_Ease).OnComplete(() => {
				PauseMenu_BG.blocksRaycasts = false;
				PauseMenu_BG.interactable = false;

				//Invoke On Close Complete Event
				OnCloseMenu.Invoke();
				isMenuOpen = false;
			});
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