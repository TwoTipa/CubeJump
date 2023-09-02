using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace Bonuses
{
    public class SlowTimeBonus : Bonus
    {
        [SerializeField] private float timeScale = 0.5f;
        [SerializeField] private float delay = 10f;
        [SerializeField] private Image image;


        protected override void OnTriggerEnter(Collider other)
        {
            
            if (!other.transform.TryGetComponent(out Player.Player player))
            {
                return;
            }
            TimeManager.Instance.SetTimeScale(timeScale, delay);
            Ui.Instance.SetBonusTimer(delay, image);
            Destroy(gameObject);
            
        }
    }
}