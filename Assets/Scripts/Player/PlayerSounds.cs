using Sounds;
using UnityEngine;

namespace Player
{
    public class PlayerSounds : MonoBehaviour
    {
        [SerializeField] private AudioClip jump, run, jumpDown;
        
        public void PlaySoundMove(Vector3 dir)
        {
            switch (Mathf.RoundToInt(dir.normalized.y))
            {
                case -1:
                    SoundPlayer.Instance.PlayClip(jumpDown);
                    break;
                case 1:
                    SoundPlayer.Instance.PlayClip(jump);
                    break;
                case 0: 
                    SoundPlayer.Instance.PlayClip(run);
                    break;
            }
        }
    }
}