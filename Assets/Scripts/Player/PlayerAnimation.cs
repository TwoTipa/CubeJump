using System;
using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void PlayAnimationMove(Vector3 dir)
        {
            switch (Mathf.RoundToInt(dir.normalized.y))
            {
                case -1:
                    _animator.Play("Jumping Down");
                    break;
                case 1:
                    _animator.Play("JumpUp");
                    break;
                case 0: 
                    _animator.Play("Run");
                    break;
                default:
                    _animator.Play("Idle");
                    break;
            }
        }

        public void PlayDeathAnimation()
        {
            _animator.SetBool("isDead", true);
            //_animator.Play("Death");
        }

        public void PlayerRestart()
        {
            _animator.SetBool("isDead", false);
            _animator.Play("Idle");
        }
    }
}