using System.Collections;
using System.Collections.Generic;
using BlocksFolder;
using Player;
using UnityEngine;

public class FakeBlock : Block
{
    protected override void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            //BlockSpawner.Instance.RefundBlock(new Vector2(transform.position.x, transform.position.z));
            Destroy(gameObject);
        }
        base.OnCollisionEnter(other);
    }
}
