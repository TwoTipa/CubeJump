using System;
using Levels;
using UnityEngine;

namespace DefaultNamespace
{
    public class BonusTimerCleaner : MonoBehaviour
    {
        private void OnEnable()
        {
            LevelChanger.LevelStart += LevelChangerOnLevelStart;
        }

        private void OnDisable()
        {
            LevelChanger.LevelStart -= LevelChangerOnLevelStart;
        }

        private void LevelChangerOnLevelStart(Level obj)
        {
            var count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}