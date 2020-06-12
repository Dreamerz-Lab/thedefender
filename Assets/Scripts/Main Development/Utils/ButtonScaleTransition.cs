using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Defender.Utility {
    public class ButtonScaleTransition : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler {
        [SerializeField] private Transform TargetTransform;

        [SerializeField] private float ScaleFactor = 0.15f;
        [SerializeField] private float Speed = 0.3f;

        [SerializeField] private AnimationCurve ScaleCurve;

        [SerializeField] private Vector3 ExitScale = Vector3.one;

        private void Start() {
            if (TargetTransform == null)
                TargetTransform = transform;

            ExitScale = transform.localScale;
        }

        public void OnPointerDown(PointerEventData eventData) {
            //thisTransform.DOKill(true);
            TargetTransform.DOScale(ExitScale * ScaleFactor, Speed);
        }

        public void OnPointerUp(PointerEventData eventData) {
            //thisTransform.DOKill(true);
            TargetTransform.DOScale(ExitScale, Speed).SetEase(ScaleCurve);
        }

        public void OnBeginDrag(PointerEventData eventData) {
            TargetTransform.DOKill();
            TargetTransform.DOScale(ExitScale, Speed).SetEase(ScaleCurve);
        }
    }
}