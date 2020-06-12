using UnityEngine;
using DG.Tweening;

namespace Defender.Utility {
    public class FlareGlowSequence : MonoBehaviour {
        [SerializeField] private Transform Target;
        private Sequence FlareSequence;

        private void Start() {
            if (Target == null)
                Target = transform;

            Target.localScale = new Vector3(0.5f, 1f, 1f);

            FlareSequence = DOTween.Sequence();
            FlareSequence.Append(Target.DOScaleX(1f, 2f));
            FlareSequence.Append(Target.DOScaleX(0.5f, 2f));
            FlareSequence.SetLoops(-1, LoopType.Restart);
            FlareSequence.Pause();
        }

        public void PlaySequence() {
            FlareSequence.Play();
        }

        public void StopSequence() {
            FlareSequence.Pause();
        }
    }
}