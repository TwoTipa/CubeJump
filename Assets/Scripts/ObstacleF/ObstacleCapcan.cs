using DefaultNamespace;
using Sounds;
using UnityEngine;

namespace ObstacleF
{
    public class ObstacleCapcan : Obstacle
    {
        [SerializeField] private AudioClip sound;
        private Rigidbody _rigidbody;

        public override void Init(ObstacleSetting setting)
        {
            base.Init(setting);
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (other.CompareTag("Player"))
            {
                Destroy();
                SoundPlayer.Instance.PlayClip(sound);
                //Destroy(gameObject);
            }
        }

        protected override void Update()
        {
            base.Update();
            Fall();
        }

        protected virtual void Fall()
        {
            _rigidbody.velocity = -Vector3.up * (speed * TimeManager.Instance.GeneralSpeed);
        }

        protected override void Fire()
        {
            base.Fire();
        }
    }
}