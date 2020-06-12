using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;


namespace Defender.Utility {
    public class ButtonScaleTransitionOnEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        private Transform TargetTransform;

        [SerializeField] private float ScaleFactor = 0.15f;
        [SerializeField] private float Speed = 0.3f;

        [SerializeField] private AnimationCurve ScaleCurve;

        [SerializeField] private Vector3 ExitScale = Vector3.one;

        private void Start() {
            TargetTransform = transform;
            ExitScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            TargetTransform.DOScale(ExitScale * ScaleFactor, Speed);
        }

        public void OnPointerExit(PointerEventData eventData) {
            TargetTransform.DOScale(ExitScale, Speed).SetEase(ScaleCurve);
        }
    }
}