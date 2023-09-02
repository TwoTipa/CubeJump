using System;
using System.Collections.Generic;
using System.Linq;
using BlocksFolder;
using DefaultNamespace;
using Levels;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace ObstacleF
{
    public class ObstacleController : MonoBehaviour
    {
        public static ObstacleController Instance { get; private set; }
        private List<ObstacleSetting> _obstacles;
        private List<Obstacle> obstaclesInScene = new();

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            PlayerHp.GameOver += PlayerHpOnGameOver;
            LevelChanger.NextLevel += PlayerHpOnGameOver;
            LevelChanger.RestartGame += PlayerHpOnGameOver;
            BlockSpawner.FloorStart += PlayerHpOnGameOver;
        }

        private void OnDisable()
        {
            LevelChanger.NextLevel -= PlayerHpOnGameOver;
            LevelChanger.RestartGame -= PlayerHpOnGameOver;
            BlockSpawner.FloorStart -= PlayerHpOnGameOver;
            PlayerHp.GameOver -= PlayerHpOnGameOver;
        }

        private void OnDestroy()
        {
            PlayerHpOnGameOver();
        }

        private void PlayerHpOnGameOver()
        {
            //SpawnersClear();
            var count = obstaclesInScene.Count;
            for (int i = 0; i < count; i++)
            {
                obstaclesInScene[0].Destroy();
            }
        }
        
        private void PlayerHpOnGameOver(int floor)
        {
            if (floor == Level.Instance.HeightToWin+1)
            {
                PlayerHpOnGameOver();
            }
        }

        public void ChangeObstacle(ObstacleSetting[] obst)
        {
            SpawnersClear();
            _obstacles = obst.ToList();
            foreach (var obstacle in _obstacles)
            {
                Instantiate(obstacle.Obstacle.GetSpawnerType(), transform).Init(obstacle);
            }
        }

        public void AddObstacle(Obstacle obst)
        {
            obstaclesInScene.Add(obst);
            
        }
        public void RemoveObstacle(Obstacle obst)
        {
            obstaclesInScene.Remove(obst);
        }
        
        private void SpawnersClear()
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                transform.GetChild(i).GetComponent<ObstacleSpawner>().Destroy();
            }
        }
    }
}
