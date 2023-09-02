using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ObstacleF
{
    public abstract class ObstacleSpawner : MonoBehaviour
    {
        private float _timeToSpawn;
        private ObstacleSetting _obstacle = new();
        private float _timer = 0f;

        public virtual void Init(ObstacleSetting obstacle)
        {
            _obstacle = obstacle;
            _timeToSpawn = obstacle.TimeToSpawn;
        }

        public virtual void Destroy()
        {
            transform.DetachChildren();
            // foreach (var child in transform.GetComponentsInChildren<Transform>())
            // {
            //     child.SetParent(transform.parent);
            // }
            Destroy(gameObject);
        }
        
        private void Update()
        {
            _timer += Time.deltaTime*TimeManager.Instance.GeneralSpeed;
            if (!(_timer >= _timeToSpawn)) return;
            SpawnObstacle();
            _timer = 0f;
        }

        protected virtual void SelectPositionToSpawn(out Vector3 pos, out Quaternion rot)
        {
            pos = new Vector3();
            rot = new Quaternion();
        }
        
        private void SpawnObstacle()
        {
            SelectPositionToSpawn(out var pos, out var rot);
            var newObst = Instantiate(_obstacle.Obstacle, pos, rot, transform);
            newObst.Init(_obstacle);
            ObstacleController.Instance.AddObstacle(newObst);
        }
    }
}
