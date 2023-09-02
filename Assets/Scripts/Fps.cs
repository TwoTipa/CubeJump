using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Fps : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}