using System;
using DefaultNamespace;
using Player;
using Sounds;
using UnityEngine;

namespace BlocksFolder
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    public class Block : MonoBehaviour
    {
        public static event Action<Vector2> BlockFall;
        public Renderer renderer;
        [SerializeField] private AudioClip dropSound;
        [SerializeField] private GameObject destroyParticle;
        private Rigidbody _rigidbody;
        protected bool isFall = false;
        protected float speed;

        public void Init(BlockChance stats)
        {
            speed = stats.Speed;
        }

        private void Awake()
        {
            renderer = GetComponentInChildren<Renderer>();
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            transform.localScale = new Vector3(Level.Instance.WidthOfSquare, Level.Instance.WidthOfSquare, Level.Instance.WidthOfSquare);
        }

        private void Update()
        {
            if (isFall) return;
            Fall();
        }

        protected virtual void Fall()
        {
            _rigidbody.velocity = -Vector3.up * (speed * TimeManager.Instance.GeneralSpeed);
            
            // if (transform.position.y <= Level.Instance.MaxHeight-0.1f)
            // {
            //     gameObject.layer = LayerMask.NameToLayer("Block");
            //     transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Block");
            //     transform.position = new Vector3(transform.position.x, ((int)transform.position.y)+0.5f, transform.position.z);
            //     GetComponent<Rigidbody>().isKinematic = true;
            //     BlockFall?.Invoke(new Vector2(transform.position.x, transform.position.z));
            //     _isFall = true;
            //     return;
            // }
        }
        
        protected virtual void OnCollisionEnter(Collision other)
        {
            if(gameObject.layer == LayerMask.NameToLayer("Block")) return;
            if (other.transform.CompareTag("Block"))
            {
                SoundPlayer.Instance.PlayClip(dropSound);
                gameObject.layer = LayerMask.NameToLayer("Block");
                transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Block");
                transform.position = new Vector3(transform.position.x, ((int)transform.position.y)+0.5f, transform.position.z);
                GetComponent<Rigidbody>().isKinematic = true;
                BlockFall?.Invoke(new Vector2(transform.position.x, transform.position.z));
                isFall = true;
                return;
            }
       
            if (other.transform.TryGetComponent(out PlayerHp hp))
            {
                Instantiate(destroyParticle, transform.position, Quaternion.identity);
                Destroy(gameObject);
                hp.RemoveShield();
                hp.GetDamage(999);
            }
        }
    }
}
