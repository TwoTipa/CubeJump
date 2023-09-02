using System;
using UnityEngine;

namespace BlocksFolder
{
    public class DestroyBlockParticle : MonoBehaviour
    {
        [SerializeField] private float time;
        private float timer = 0f;
        
        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= time)
            {
                Destroy(gameObject);
            }
        }
    }
}