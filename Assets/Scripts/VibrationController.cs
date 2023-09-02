using System;
using BlocksFolder;
using Lofelt.NiceVibrations;
using UnityEngine;

namespace DefaultNamespace
{
    public class VibrationController : MonoBehaviour
    {
        private void OnEnable()
        {
            Block.BlockFall += BlockOnBlockFall;
        }

        private void OnDisable()
        {
            Block.BlockFall -= BlockOnBlockFall;
        }

        private void BlockOnBlockFall(Vector2 obj)
        {
            Handheld.Vibrate();
        }
    }
}