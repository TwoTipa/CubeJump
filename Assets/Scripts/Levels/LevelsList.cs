using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "LelelsList", order = 0)]
    public class LevelsList : ScriptableObject
    {
        [SerializeField] public Level[] levels;
    }
}