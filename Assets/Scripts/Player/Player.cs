using System;
using DefaultNamespace;
using Levels;
using UnityEngine;

namespace Player
{
    [DefaultExecutionOrder(150)]
    public class Player : MonoBehaviour
    {
        private PlayerAnimation _animation;
        private PlayerHp _hp;
        private PlayerMove _move;
        private PlayerSounds _sound;

        private void Awake()
        {
            _animation = GetComponent<PlayerAnimation>();
            _hp = GetComponent<PlayerHp>();
            _move = GetComponent<PlayerMove>();
            _sound = GetComponent<PlayerSounds>();
        }

        private void OnEnable()
        {
            PlayerHp.GameOver += OnGameOver;
            PlayerHp.GameOver += _animation.PlayDeathAnimation;
            PlayerMove.OnPlayerMove += _animation.PlayAnimationMove;
            PlayerMove.OnPlayerMove += _sound.PlaySoundMove;
            LevelChanger.LevelStart += PlayerRestart;
            Ui.PlayerRespawn += OnPlayerRespawn;
        }

        private void OnDisable()
        {
            PlayerHp.GameOver -= OnGameOver;
            PlayerMove.OnPlayerMove -= _animation.PlayAnimationMove;
            PlayerHp.GameOver -= _animation.PlayDeathAnimation;
            PlayerMove.OnPlayerMove -= _sound.PlaySoundMove;
            LevelChanger.LevelStart -= PlayerRestart;
            Ui.PlayerRespawn -= OnPlayerRespawn;
        }

        private void OnGameOver()
        {
            _move.blockMove();
            //TimeManager.Instance.Pause();
        }
        private void OnPlayerRespawn()
        {
            _move.SetPosition(_move.Pos);
            _hp.SetHp(1);
            _animation.PlayerRestart();
        }

        private void PlayerRestart(Level lvl)
        {
            _move.SetPosition(new Vector2(lvl.transform.position.x+lvl.WidthOfSquare +0.5f, lvl.transform.position.z + lvl.WidthOfSquare + 0.5f));
            _hp.SetHp(1);
            _animation.PlayerRestart();
        }
    }
}