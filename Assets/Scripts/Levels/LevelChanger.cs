using System;
using System.Collections;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

namespace Levels
{
    public class LevelChanger : MonoBehaviour
    {
        public static event Action<Level> LevelStart;
        public static event Action RestartGame;
        public static event Action NextLevel;
        public static LevelChanger Instance { get; private set; }

        [SerializeField] private LevelsList levels;
        [SerializeField] private Level currentLevel;
        
        private int _currentLevelNum = 0;

        public void Restart()
        {
            RepeatLevel();
            RestartGame?.Invoke();
        }
    
        public void Next()
        {
            ChangeLevel();
            PlayerPrefs.SetInt("LevelNum", _currentLevelNum);
            PlayerPrefs.Save();
            NextLevel?.Invoke();
        }

        private void ChangeLevel()
        {
            _currentLevelNum++;
            OpenLevel(levels.levels[_currentLevelNum]);
        }

        private void RepeatLevel()
        {
            OpenLevel(levels.levels[_currentLevelNum]);
        }

        private void OpenLevel(Level lvl)
        {
            if (currentLevel != null)
            {
                DeleteLevel();
            }
            currentLevel = Instantiate(lvl, transform);
            currentLevel.Init(_currentLevelNum+1);
            LevelStart?.Invoke(currentLevel);
            TimeManager.Instance.Play();
        }

        private void DeleteLevel()
        {
            DOTween.KillAll();
            Destroy(currentLevel.gameObject);
        }
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            int savedLevelNum = PlayerPrefs.GetInt("LevelNum");
            if (savedLevelNum != 0)
            {
                _currentLevelNum = savedLevelNum;
            }
            else
            {
                _currentLevelNum = 0;
            }
            OpenLevel(levels.levels[_currentLevelNum]);
        }
    }
}