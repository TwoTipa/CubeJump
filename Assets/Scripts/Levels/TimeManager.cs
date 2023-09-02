using System;
using Levels;
using UniRx;
using UnityEngine;

namespace DefaultNamespace
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance;
        public float GeneralSpeed { get; private set; } = 1f;
        private float _baseTimeScale = 1f;
        private CompositeDisposable _disposable = new();

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            LevelChanger.RestartGame += Play;
        }

        private void OnDisable()
        {
            LevelChanger.RestartGame -= Play;
        }

        private void Update()
        {
            //ButtonControl();
        }

        private void ButtonControl()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Pause();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                Play();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                SetTimeScale(2f, 10f);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                SetTimeScale(0.5f, 10f);
            }
        }

        public void SetTimeScale(float timeScale, float timeToOver)
        {
            _disposable.Clear();
            GeneralSpeed = timeScale;
            Observable.EveryUpdate().Subscribe(_ =>
            {
                timeToOver -= Time.deltaTime;
                if (timeToOver <= 0)
                {
                    GeneralSpeed = _baseTimeScale;
                    _disposable.Clear();
                }
            }).AddTo(_disposable);

        }

        public void Pause()
        {
            Time.timeScale = 0f;
        }

        public void Play()
        {
            Time.timeScale = _baseTimeScale;
        }
        
    }
}