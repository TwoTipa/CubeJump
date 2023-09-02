using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlocksFolder
{
    public class BlockBehaviourChanger : MonoBehaviour
    {
        [SerializeField] private BlockChance[] blocks;
        [field: SerializeField] public ColorPattern ColorPattern { get; private set; }
        [field: SerializeField] public List<Color> Colors { get; private set; }

        public void UpdateBlocks()
        {
            BlockSpawner.Instance.UpdateBlocksChances(blocks);
        }

        public void UpdateSpawnRate(float rate)
        {
            BlockSpawner.Instance.UpdateSpawnRate(rate);
        }
        public void UpdateColorPattern()
        {
            BlockSpawner.Instance.UpdateColorPattern(ColorPattern, Colors.ToArray());
        }

        private void OnValidate()
        {
            AdjustListSize();   
        }

        private void AdjustListSize()
        {
            switch (ColorPattern)
            {
                case ColorPattern.Random:
                    ChangeColorsCount(4);
                    break;
                case ColorPattern.Even:
                    ChangeColorsCount(2);
                    break;
                case ColorPattern.Solid:
                    ChangeColorsCount(1);
                    break;
            }
        }

        private void ChangeColorsCount(int count)
        {
            if (Colors.Count > count)
            {
                Colors = Colors.GetRange(0, Mathf.Min(Colors.Count, count));
            }
            else
            {
                for (int i = Colors.Count; i < count; i++)
                {
                    Colors.Add(new Color());
                }
            }

        }
    }
}