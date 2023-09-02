using UnityEngine;

namespace BlocksFolder
{
    public class BaseBlock : Block
    {
        protected override void OnCollisionEnter(Collision other)
        {
            base.OnCollisionEnter(other);
        }
    }
}
