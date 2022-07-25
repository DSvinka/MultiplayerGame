using DG.Tweening;
using UnityEngine;

namespace Code.Views.UI
{
    public class LoadingIndicatorView: MonoBehaviour
    {
        [SerializeField] private Transform _rotatingIconTransform;

        private void OnEnable()
        {
            var rotation = _rotatingIconTransform.eulerAngles + new Vector3(0, 0, 360);
            _rotatingIconTransform
                .DOLocalRotate(rotation, 5f, RotateMode.FastBeyond360)
                .SetLoops(-1)
                .SetEase(Ease.Linear);
        }

        private void OnDisable()
        {
            _rotatingIconTransform.DOKill();
        }
    }
}