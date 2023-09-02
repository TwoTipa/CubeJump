using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ProjectorController : MonoBehaviour
    {
        [SerializeField] private AnimationCurve shadowCurve;
        private Projector _projector;
        private float _initialPosition;
        private float _endPosition;

        private void Start()
        {
            _projector = GetComponentInChildren<Projector>();
            _projector.orthographicSize = 0.1f;
            _initialPosition = transform.position.y;
            _endPosition = _initialPosition - 10f;
        }

        private void Update()
        {
            var test = 1f - ((transform.position.y - _endPosition) / (_initialPosition - _endPosition));
            _projector.orthographicSize = shadowCurve.Evaluate(test);
            if (test >= 1f)
            {
                _projector.gameObject.SetActive(false);
            }
        }
    }
}