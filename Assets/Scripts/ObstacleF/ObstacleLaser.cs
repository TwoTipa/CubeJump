using DefaultNamespace;
using DG.Tweening;
using Player;
using Sounds;
using UniRx;
using UnityEngine;
using VolumetricLines;

namespace ObstacleF
{
    public class ObstacleLaser : Obstacle
    {
        [SerializeField] private GameObject laser;
        [SerializeField] private GameObject endLaser;
        [SerializeField] private LayerMask blockLayer;
        [SerializeField] private AnimationCurve upperAnim;
        [SerializeField] private AudioClip laserSound;
        [SerializeField] private float fireTime = 2f;
        [SerializeField] private float offTime = 1f;
        private VolumetricLineBehavior _laserScript;
        private CompositeDisposable _disposables = new ();
        private CompositeDisposable _shieldDisposables = new ();
    
        public override void Init(ObstacleSetting setting)
        {
            base.Init(setting);
            transform.position = new Vector3(transform.position.x, transform.position.y-preparationTime-1, transform.position.z);
            //transform.DOMoveY(transform.position.y+preparationTime+Level.Instance.WidthOfSquare*.5f, preparationTime*0.5f);
        
        }
        public override void Destroy()
        {
            _disposables.Clear();
            _shieldDisposables.Clear();
            base.Destroy();
        }

        protected override void Fire()
        {
            base.Fire();
            laser.SetActive(true);
            SoundPlayer.Instance.PlayClip(laserSound);
            Ray ray = new Ray(transform.position, transform.right);
            Observable.EveryUpdate().Subscribe(_ =>
            {
                if (Physics.Raycast(ray, out var hit, Mathf.Infinity, LayerMask.GetMask("Block", "Fall")))
                {
                    laser.transform.localScale = new Vector3((hit.point-transform.position).magnitude*Level.Instance.WidthOfSquare, 1, 1);
                }
                else
                {
                    laser.transform.localScale = new Vector3(Level.Instance.Width+1*Level.Instance.WidthOfSquare, 1, 1);
                }
                fireTime -= Time.deltaTime;
                if (fireTime <= 0)
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
            if (Physics.Raycast(transform.position-Vector3.up*0.5f, transform.right, Mathf.Infinity, LayerMask.GetMask("Block")))
            {
                return false;
            }

            return true;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerHp hp))
            {
                hp.GetDamage((int)dmg);
                if (hp.CheckShield())
                {
                    laser.GetComponent<BoxCollider>().enabled = false;
                    var timer = 0f;
                    Observable.EveryUpdate().Subscribe(_ =>
                    {
                        if (timer >= offTime)
                        {
                            laser.GetComponent<BoxCollider>().enabled = true;
                            _shieldDisposables.Clear();
                        }
                        timer += Time.deltaTime;
                    }).AddTo(_shieldDisposables);
                }
            }
        }

        private void OnDisable()
        {
            _disposables.Clear();
            _shieldDisposables.Clear();
        }
    }
}
