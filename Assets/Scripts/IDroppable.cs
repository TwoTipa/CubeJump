using UnityEngine;

namespace DefaultNamespace
{
    public interface IDroppable : ISpawned
    {
        public void Fall(float speed);
        public void LifeTimer(float lifeTime);
    }
}