using System;
using System.Collections.Generic;
using BlocksFolder;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bonuses
{
    public class BonusSpawner : MonoBehaviour
    {
        [SerializeField] private float timeToSpawn;
        [SerializeField] private List<BonusSetting> bonuses = new();

        private float _timer = 0f;
        private bool endLevel = false;
        
        private void Update()
        {
            _timer += Time.deltaTime*TimeManager.Instance.GeneralSpeed;
            if (_timer < timeToSpawn) return;
            SpawnBonus();
            _timer = 0f;
        }

        private void OnEnable()
        {
            BlockSpawner.FloorStart += BlockSpawnerOnFloorStart;
        }

        private void OnDisable()
        {
            BlockSpawner.FloorStart -= BlockSpawnerOnFloorStart;
        }

        private void BlockSpawnerOnFloorStart(int obj)
        {
            if (obj >= Level.Instance.HeightToWin)
            {
                endLevel = true;
            }
        }

        private void SpawnBonus()
        {
            if(bonuses.Count == 0) return;
            if (endLevel) return;
            var i = Random.Range(0, bonuses.Count);
            Bonus bonus = bonuses[i].bonus;
            Instantiate(bonus, SelectPositionForSpawn(), Quaternion.identity, transform).Init(bonuses[i]);
        }

        private Vector3 SelectPositionForSpawn()
        {
            Vector3 ret = new();
            var lvl = Level.Instance;
         
            ret = new Vector3(Random.Range(0, lvl.Width)+0.5f,lvl.MaxHeight+10,Random.Range(0, lvl.Lenght)+0.5f);
            return ret;
        }
    }
}