using System;
using DefaultNamespace;
using Player;
using UniRx;
using UnityEngine;

namespace ObstacleF
{
    public class ObstacleRocket : Obstacle
    {
        [SerializeField] private GameObject rocketStand;
        [SerializeField] private GameObject rocket;
        [SerializeField] private GameObject boom;
        [SerializeField] private ParticleSystem effect;

        private CompositeDisposable _disposables = new CompositeDisposable();
        private Vector3 _startPosition;
        private Rigidbody _rigidbody;

        public override void Init(ObstacleSetting setting)
        {
            base.Init(setting);
            transform.position -= new Vector3(0, preparationTime+1, 0);
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public override void Destroy()
        {
            _disposables.Clear();
            base.Destroy();
            //boom.SetActive(true);
            // rocket.SetActive(false);
            // var timer = 1f;
            // Observable.EveryFixedUpdate().Subscribe(x =>
            // {
            //     timer -= Time.deltaTime;
            //     if (timer <= 0)
            //     {
            //         base.Destroy();
            //     }
            // }).AddTo(this);
        }

        protected override void Fire()
        {
            base.Fire();
            _startPosition = transform.position;
            Observable.EveryFixedUpdate().Subscribe(x => {
                _rigidbody.MovePosition(Vector3.MoveTowards(transform.position, transform.position+transform.right*100, 0.02f * speed * TimeManager.Instance.GeneralSpeed));
                rocket.transform.Rotate(Vector3.up, 1f);
                if ((transform.position - _startPosition).magnitude > 10f)
                {
                    Destroy();
                }
            }).AddTo(_disposables);
        }

        protected override bool TryLaunch()
        {
            var position = transform.position;
            transform.position = position + Vector3.up * (Time.deltaTime * TimeManager.Instance.GeneralSpeed);
            if (!base.TryLaunch())
            {
                return false;
            }
            if (Physics.Raycast(transform.position-Vector3.up*0.2f, transform.right, Mathf.Infinity, LayerMask.GetMask("Block")))
            {
                return false;
            }

            rocketStand.SetActive(false);
            effect.Play();
            return true;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            Instantiate(boom, transform.position, new Quaternion());
            Destroy();
        }
    }
}
