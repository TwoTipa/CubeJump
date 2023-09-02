using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Levels;
using UnityEngine;

public class Throne : MonoBehaviour
{
    public static Throne Instance { get; private set; }
    [SerializeField] private Ball ball;
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            //ball.gameObject.SetActive(true);
            //ball.Launch();
            Level.Instance.Win();
        }
    }

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        LevelChanger.LevelStart += level => gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        LevelChanger.LevelStart -= level => gameObject.SetActive(false);
    }
}
