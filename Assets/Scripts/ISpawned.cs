using ObstacleF;

namespace DefaultNamespace
{
    public interface ISpawned
    {
        public ObstacleSpawner GetSpawnerType();
        public void Init(ObstacleSetting setting);
    }
}