using System;
using DefaultNamespace;
using DG.Tweening;
using Libs.GradientSkybox;
using Player;
using UnityEngine;
using UnityEngine.UIElements;
using BlockSpawner = BlocksFolder.BlockSpawner;

public class Level : MonoBehaviour
{
    public static event Action WinLevel;
    public static Level Instance { get; private set; }
    public int LevelNum => _levelNum;
    public float MaxHeight { get; private set; } = 0;
    public int MinHeight { get; private set; } = 0;
    public float HeightToWin => heightToWin;
    public int Lenght => lenght;
    public int MaxBacklog => maxBacklog;
    public int Width => width;
    public int WidthOfSquare => widthOfSquare;
    public Transform background;

    [SerializeField] private int width = 3;
    [SerializeField] private int lenght = 3;
    [SerializeField] private int widthOfSquare = 1;
    [SerializeField] private int maxBacklog = 3;
    [SerializeField] private float heightToWin = 10;

    [SerializeField] private GameObject startPlatform;

    private int _levelNum;

    public void Win()
    {
        TimeManager.Instance.Pause();
        WinLevel?.Invoke();
    }

    public void Init(int num)
    {
        _levelNum = num;
    }
    
    private void MinHeightChange()
    {
        MinHeight++;
    }

    private void OnFloorStart(int obj)
    {
        MaxHeight = obj;
        if (obj-1 >= heightToWin)
        {
            SpawnWiningObject();
        }
    }

    private void SpawnWiningObject()
    {
        var throne = Throne.Instance;
        throne.transform.position = new Vector3(WidthOfSquare + 0.5f, MaxHeight + 5, WidthOfSquare + 0.5f);
        throne.gameObject.SetActive(true);
    }
    
    private void OnEnable()
    {
        BlockSpawner.FloorStart += OnFloorStart; 
    }

    private void OnDisable()
    {
        BlockSpawner.FloorStart -= OnFloorStart;
    }

    private void OnValidate()
    {
        startPlatform.transform.localScale = new Vector3(width * widthOfSquare, widthOfSquare, lenght * widthOfSquare);
    }

    private void Start()
    {
        CameraController.Instance.ChangeBackground(background);
    }

    private void Awake()
    {
        Instance = this;
        background = GetComponentInChildren<SkyboxSettings>().transform;
    }
}

