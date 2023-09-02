using System;
using BlocksFolder;
using UnityEngine;

namespace DefaultNamespace
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float timer = 5f;
        [SerializeField] private Vector3 startPosition;
        private float _myTime = 0f;
        public void Launch()
        {
            _myTime = timer;
            //transform.position = startPosition;//UnityEngine.Random.Range(0, Level.Instance.Width)
            GetComponent<HingeJoint>().connectedAnchor = startPosition;
            //CameraController.Instance.LookToEndLevel(timer, transform);
            var hits = Physics.SphereCastAll(transform.position, 10f, Vector3.forward, Mathf.Infinity);
            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent<Block>(out var block))
                {
                    var _block = block.GetComponent<Rigidbody>();
                    _block.constraints = RigidbodyConstraints.None;
                    _block.isKinematic = false;
                    _block.freezeRotation = false;
                }
            }
        }

        private void Start()
        {
            //Launch();
        }

        private void Update()
        {
            _myTime -= Time.deltaTime;
            if (_myTime <= 0)
            {
                Level.Instance.Win();
                gameObject.SetActive(false);
            }
        }
    }
}