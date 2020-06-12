using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Defender.Utility {
	public class ToggleSwitch : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler {
		//The Toggle Value
		public bool isToggle = false;

		//The Button Switch to Animate
		[SerializeField] private RectTransform ButtonSwitch;
		[SerializeField] private UnityEngine.UI.Image ButtonSwitch_Img;
		[SerializeField] private UnityEngine.UI.Image SwitchBg_Img;

		[Space(6)]
		[SerializeField] private Color TurnOnColor;
		private Color TurnOffColor;
		[SerializeField] private Color OnHighlight;
		private Color OnNormal;

		//Toggle Animation Speed
		[Space(6)]
		[SerializeField] private float ToggleSpeed = 0.2f;

		//Position for Switch when Turned On and Off
		[Space(6)]
		[SerializeField] private Vector2 SwitchOnPosition;
		[SerializeField] private Vector2 SwitchOffPosition;

		//UnityEvent Called When Toggled
		[Space(6)]
		public UnityEvent OnToggleChanged;
		public UnityEvent OnToggleOn;
		public UnityEvent OnToggleOff;

		private void Start() {
			OnNormal = ButtonSwitch_Img.color;
			TurnOffColor = SwitchBg_Img.color;

			if (isToggle) {
				ButtonSwitch.anchoredPosition = SwitchOnPosition;
				SwitchBg_Img.color = TurnOnColor;
			} else {
				ButtonSwitch.anchoredPosition = SwitchOffPosition;
				SwitchBg_Img.color = TurnOffColor;
			}
		}

		public void OnPointerClick(PointerEventData eventData) {
			isToggle = !isToggle;

			if (isToggle) {
				ButtonSwitch.DOAnchorPos(SwitchOnPosition, ToggleSpeed);
				SwitchBg_Img.color = TurnOnColor;
				OnToggleOn.Invoke();
			} else {
				ButtonSwitch.DOAnchorPos(SwitchOffPosition, ToggleSpeed);
				SwitchBg_Img.color = TurnOffColor;
				OnToggleOff.Invoke();
			}

			ButtonSwitch_Img.color = OnNormal;

			OnToggleChanged.Invoke();
		}

		public void OnPointerEnter(PointerEventData eventData) {
			ButtonSwitch_Img.color = OnHighlight;
		}

		public void OnPointerExit(PointerEventData eventData) {
			ButtonSwitch_Img.color = OnNormal;
		}

		public void OnPointerUp(PointerEventData eventData) {
			ButtonSwitch_Img.color = OnNormal;
		}

#if UNITY_EDITOR
		private void OnValidate() {
			if (isToggle)
				ButtonSwitch.anchoredPosition = SwitchOnPosition;
			else
				ButtonSwitch.anchoredPosition = SwitchOffPosition;
		}
#endif
	}
}