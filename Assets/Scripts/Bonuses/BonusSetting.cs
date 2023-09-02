using System;
using UnityEngine;

namespace Bonuses
{
    [Serializable]
    public struct BonusSetting
    {
        [field: SerializeField] public Bonus bonus;
        [field: SerializeField] public float speed;
        [field: SerializeField] public float lifeTimer;

    }
}