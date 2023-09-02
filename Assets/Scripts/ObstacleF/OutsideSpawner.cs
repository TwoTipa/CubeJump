using UnityEngine;

namespace ObstacleF
{
    public class OutsideSpawner : ObstacleSpawner
    {
        protected override void SelectPositionToSpawn(out Vector3 pos, out Quaternion rot)
        {
            float orientation = Random.Range(0, 2);
            float side = Random.Range(0, 2);
            float lenght = 0f;
            float width = 0f;
            var lvl = Level.Instance;
            if (orientation == 0)
            {
                lenght = Random.Range(0, lvl.Lenght)+ 0.5f;
                width = (lvl.Width+1 + lvl.WidthOfSquare) * side-1;
                rot = new Quaternion(0, 180*side,0,0);
            }
            else
            {
                width = Random.Range(0, lvl.Width)+ 0.5f;
                lenght = (lvl.Lenght+1 + lvl.WidthOfSquare) * side-1;
                rot = Quaternion.Euler(0,  -90+180*side,0);
            }
            pos = new Vector3(width, lvl.MaxHeight, lenght);
        }
    }
}
