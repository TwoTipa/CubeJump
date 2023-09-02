using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Player;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class TutorialSwipe : MonoBehaviour
{
    private bool _isWathcing = false;
    private void Start()
    {
        if (_isWathcing)
        {
            gameObject.SetActive(false);
            return;
        }
        TimeManager.Instance.SetTimeScale(0.00000001f, 1000f);
        PlayerMove.OnPlayerMove += PlayerMoveOnOnPlayerMove;
    }

    private void PlayerMoveOnOnPlayerMove(Vector3 obj)
    {
        TimeManager.Instance.SetTimeScale(1f, 0f);
        gameObject.SetActive(false);
        _isWathcing = true;
        PlayerMove.OnPlayerMove -= PlayerMoveOnOnPlayerMove;
    }
}
