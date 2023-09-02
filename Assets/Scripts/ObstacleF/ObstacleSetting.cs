using DefaultNamespace;
using UnityEngine;

namespace ObstacleF
{
    [System.Serializable]
    public struct ObstacleSetting
    {
        [field: SerializeField] public Obstacle Obstacle { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float TimeToSpawn { get; private set; }
        [field: SerializeField] public float PreparationTime { get; private set; }
        [field: SerializeField] public float Dmg { get; private set; }
    }
}