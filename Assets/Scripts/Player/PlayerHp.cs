using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerHp : MonoBehaviour
    {
        public static event Action<int> HpChanged;
        public static event Action GameOver;
        public static event Action<bool> ShieldChanged;
        private bool _isShield = false;
        private bool _isDead = false;
        private GameObject currentRagdoll;


        [SerializeField] private int hp = 1;
        [SerializeField] private GameObject ragdollCharPrefab;
        [SerializeField] private GameObject noRagdollChar;

        public bool CheckShield()
        {
            return _isShield;
        }

        public void AddShield(float time)
        {
            _isShield = true;
            ShieldChanged?.Invoke(_isShield);
            StartCoroutine("ShieldCoroutine", time);
        }

        public void RemoveShield()
        {
            _isShield = false;
            ShieldChanged?.Invoke(_isShield);
        }
        
        public void GetDamage(int dmgCount)
        {
            if (_isDead)return;
            if (_isShield)
            {
                RemoveShield();
                return;
            }
            hp -= dmgCount;
            HpChanged?.Invoke(hp);
            if (hp > 0) return;
            StartCoroutine("DeathCoroutine", 0f);
        }

        public void SetHp(int hp)
        {
            if (hp < 1)
            {
                hp = 1;
            }
            _isDead = false;
            this.hp = hp;
            ChangeChar(true);
        }

        private void ChangeChar(bool state)
        {
            ResetRagdoll();
            noRagdollChar.SetActive(state);
            if (!state)
            {
                currentRagdoll = Instantiate(ragdollCharPrefab, noRagdollChar.transform.position, noRagdollChar.transform.rotation);
                currentRagdoll.SetActive(true);
            }
        }
        
        private void ResetRagdoll()
        {
            if(currentRagdoll == null) return;
            Destroy(currentRagdoll.gameObject);
        }
        
        private IEnumerator ShieldCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            RemoveShield();
        }
        
        private IEnumerator DeathCoroutine(float time)
        {
            ChangeChar(false);
            yield return new WaitForSeconds(time);
            _isDead = true;
            GameOver?.Invoke();
        }
        
        private void Start()
        {
            HpChanged?.Invoke(hp);
        }
    }
}
