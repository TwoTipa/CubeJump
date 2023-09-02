using UnityEngine;

namespace ObstacleF
{
    public class ObstacleBehaviorChanger : MonoBehaviour
    {
        [field: SerializeField] private ObstacleSetting[] obstacles;

        
        
        public void UpdateObstacle()
        {
            ObstacleController.Instance.ChangeObstacle(obstacles);
        }
    }
}