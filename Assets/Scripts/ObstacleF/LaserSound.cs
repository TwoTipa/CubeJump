using System;
using Sounds;
using UnityEngine;

namespace ObstacleF
{
    public class LaserSound : MonoBehaviour
    {
        [SerializeField] private AudioClip laserSound;

        private void Update()
        {
            SoundPlayer.Instance.PlayClip(laserSound);
        }
    }
}