using System;
using DefaultNamespace;
using Player;
using UnityEditor;
using UnityEngine;

namespace ObstacleF
{
    public abstract class Obstacle : MonoBehaviour , ISpawned
    {
        protected float speed;
        protected float timeToSpawn;
        protected float preparationTime;
        protected float dmg;
        
        protected bool starting = false;

        [SerializeField] protected ObstacleSpawner spawnerType;

        public ObstacleSpawner GetSpawnerType()
        {
            return spawnerType;
        }

        public virtual void Init(ObstacleSetting setting)
        {
            speed = setting.Speed;
            timeToSpawn = setting.TimeToSpawn;
            preparationTime = setting.PreparationTime;
            dmg = setting.Dmg;
        }

        public virtual void Destroy()
        {
            ObstacleController.Instance.RemoveObstacle(this);
            Destroy(gameObject);
        }
        
        protected virtual void Update()
        {
            if (starting) { return; }
            if (TryLaunch())
            {
                Fire();
            }
        }

        private void OnEnable()
        {
            //PlayerHp.GameOver += PlayerHpOnGameOver;
        }

        private void OnDestroy()
        {
            ObstacleController.Instance.RemoveObstacle(this);
            //PlayerHp.GameOver -= PlayerHpOnGameOver;
        }

        private void PlayerHpOnGameOver()
        {
            Destroy(gameObject);
        }


        protected virtual void Fire()
        {
            starting = true;
        }

        protected virtual bool TryLaunch()
        {
            preparationTime -= Time.deltaTime * TimeManager.Instance.GeneralSpeed;
            if (preparationTime <= 0)
            {
                return true;
            }
            return false;
        }
        
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerHp hp))
            {
                hp.GetDamage((int)dmg);
            }
        }
    }
}
