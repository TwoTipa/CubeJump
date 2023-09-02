using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Bonuses
{
    public class ShieldBonus : Bonus
    {
        [SerializeField] private Image image;
        [SerializeField] private GameObject sheeldPrefab;
        [SerializeField] private float time = 5f;
        
        
        protected override void OnTriggerEnter(Collider other)
        {
            if (!other.transform.TryGetComponent(out Player.Player player))
            {
                return;
            }

            other.transform.GetComponent<PlayerHp>().AddShield(time);
            Destroy(gameObject);
        }
    }
}