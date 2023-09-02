using UnityEngine;

namespace BlocksFolder
{
    [System.Serializable]
    public struct BlockChance
    {
        [field: SerializeField] public Block Block { get; private set; }
        [field: SerializeField] public float Chance { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
    }
}