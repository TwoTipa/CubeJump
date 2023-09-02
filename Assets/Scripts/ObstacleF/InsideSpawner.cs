using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ObstacleF
{
    public class InsideSpawner : ObstacleSpawner
    {
        protected override void SelectPositionToSpawn(out Vector3 pos, out Quaternion rot)
        {
            var lvl = Level.Instance;
            float width = Random.Range(0, lvl.Width);
            float lenght = Random.Range(0, lvl.Lenght);
            rot = new Quaternion();
            pos = new Vector3(width+lvl.WidthOfSquare*0.5f, lvl.MaxHeight+5, lenght+lvl.WidthOfSquare*0.5f);
        }
    }
}
