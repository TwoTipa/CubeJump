using System;
using UnityEngine;

namespace Player
{
    public class ShieldPresenter : MonoBehaviour
    {
        [SerializeField] private ParticleSystem shield;

        private void OnEnable()
        {
            PlayerHp.ShieldChanged += PlayerHpOnShieldChanged;
        }

        private void OnDisable()
        {
            PlayerHp.ShieldChanged -= PlayerHpOnShieldChanged;
        }

        private void Start()
        {
            shield.Stop();
        }

        private void PlayerHpOnShieldChanged(bool obj)
        {
            if (obj)
            {
                shield.Play();
            }
            else
            {
                shield.Stop();
            }
        }
    }
}