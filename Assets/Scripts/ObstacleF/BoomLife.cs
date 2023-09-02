using System;
using System.Collections;
using Sounds;
using UnityEngine;

namespace ObstacleF
{
    public class BoomLife : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 1f;
        [SerializeField] private AudioClip boomSound;
        private IEnumerator Start()
        {
            SoundPlayer.Instance.PlayClip(boomSound);
            yield return new WaitForSeconds(lifeTime);
            Destroy(gameObject);
        }
    }
}