using System;
using DefaultNamespace;
using ObstacleF;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Bonuses
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bonus : MonoBehaviour
    {
        private CompositeDisposable _disposable = new();
        private CompositeDisposable _lifeDisposable = new();
        private Rigidbody _rigidbody;
        private Transform _camera;

        private float _speed = 9f;
        
        public virtual void Init(BonusSetting setting)
        {
            _speed = setting.speed;
            LifeTimer(setting.lifeTimer);
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _camera = Camera.main!.transform;
        }

        private void Update()
        {
            transform.LookAt(_camera);
            Fall(_speed);
        }

        protected virtual void Fall(float speed)
        {
            _rigidbody.velocity = -Vector3.up * (speed * TimeManager.Instance.GeneralSpeed);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!other.transform.TryGetComponent(out Player.Player player))
            {
                return;
            }
        }

        private void OnDestroy()
        {
            _disposable.Clear();
            _lifeDisposable.Clear();
        }

        protected virtual void LifeTimer(float lifeTime)
        {
            Observable.EveryUpdate().Subscribe(_ =>
            {
                lifeTime -= Time.deltaTime;
                if (lifeTime <= 0)
                {
                    Destroy(gameObject);
                }
            }).AddTo(_lifeDisposable);
        }
    }
}