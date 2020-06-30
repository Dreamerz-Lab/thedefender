using UnityEngine;
using UnityEngine.EventSystems;

namespace Defender.Utility {
    public class HapticOnEnter : MonoBehaviour, IPointerEnterHandler {
        [SerializeField] private float frequency = 0.1f;
        [SerializeField] private float amplitude = 1f;
        [SerializeField] private float duration = 0.03f;

        public void OnPointerEnter(PointerEventData eventData) {
#if !UNITY_EDITOR
            HapticUtility.instance.HapticOn(
                HandedInputSelector.instance.ActiveController,
                frequency, amplitude, duration);
#endif
        }
    }
}