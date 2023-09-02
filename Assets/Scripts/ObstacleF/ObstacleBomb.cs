using System;
using DefaultNamespace;
using Sounds;
using UnityEngine;

namespace ObstacleF
{
    [RequireComponent(typeof(Rigidbody))]
    public class ObstacleBomb : ObstacleCapcan
    {
        [SerializeField] private GameObject boom;
        protected override void Fire()
        {
            Destroy();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (other.CompareTag("Player"))
            {
                Instantiate(boom, transform.position, new Quaternion());
                Destroy();
            }
        }
    }
}