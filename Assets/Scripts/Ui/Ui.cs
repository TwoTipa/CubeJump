using System;
using System.Collections;
using System.Collections.Generic;
using BlocksFolder;
using DefaultNamespace;
using Levels;
using Player;
using Sounds;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Ui : MonoBehaviour
{
    public static Ui Instance { get; private set; }
    public static event Action PlayerRespawn;
    
    [SerializeField] private string format = "{0} / {1}";
    [SerializeField] private string levelFormat = "Level: {0}";
    [SerializeField] private TMP_Text currentHeight;
    [SerializeField] private TMP_Text currentLevel;
    [SerializeField] private TMP_Text currentLevelInWinWindow;
    [SerializeField] private TMP_Text currentLevelInLoseWindow;
    [SerializeField] private TMP_Text bossTimer;
    [SerializeField] private Slider heightSlider;
    [SerializeField] private Transform bonusTimerContainer;
    [SerializeField] private GameObject bonusTimerPrefab;
    
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;
    
    [SerializeField] private Canvas loseWindow;
    [SerializeField] private Canvas winWindow;
    [SerializeField] private Canvas gameWindow;
    [SerializeField] private Canvas settingsWindow;

    private Canvas[] _allWindows;
    private CompositeDisposable _disposable = new();
    private List<BonusTimer> _bonusTimers = new(); 

    private int _floorCount;
    private int _blocksFall = 0;
    private int _levelId;
    private float _bossTimer = 10f;
    private float _currentBossTimer;
    private List<GameObject> _bonusTimersUi = new List<GameObject>();

    public void StartBossTimer()
    {
        TimeSpan timeSpan;
        bossTimer.gameObject.SetActive(true);
        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (_bossTimer <= 0)
            {
                bossTimer.gameObject.SetActive(false);
                _disposable.Clear();
            }
            _bossTimer -= Time.deltaTime*TimeManager.Instance.GeneralSpeed;
            timeSpan = TimeSpan.FromSeconds(_bossTimer);
            bossTimer.text = timeSpan.Seconds.ToString() + "s";
        }).AddTo(_disposable);
    }

    public void OpenSettings()
    {
        TimeManager.Instance.Pause();
        ShowWindow(settingsWindow);
    }

    public void CloseSettings()
    {
        TimeManager.Instance.Play();
        settingsWindow.gameObject.SetActive(false);
    }
    
    public void SetBossTimer(float delay)
    {
        _bossTimer = delay;
    }

    public void SetBonusTimer(float delay, Image image)
    {
        var count = _bonusTimersUi.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(_bonusTimersUi[i]);
        }
        var newTimer = Instantiate(bonusTimerPrefab, bonusTimerContainer).GetComponent<BonusTimerPrefab>().Initialize(image);
        _bonusTimersUi.Add(newTimer);
        _bonusTimers.Add(new BonusTimer(delay, image, newTimer));
    }

    private void OnEnable()
    {
        BlockSpawner.FloorStart += UpdateText;
        Block.BlockFall += BlockOnBlockFall;
        PlayerHp.GameOver += ShowLooseWindow;
        Level.WinLevel += OnWinLevel;
        LevelChanger.LevelStart += ShowGameWindow;
    }

    private void OnDisable()
    {
        BlockSpawner.FloorStart -= UpdateText;
        Block.BlockFall -= BlockOnBlockFall;
        PlayerHp.GameOver -= ShowLooseWindow;
        Level.WinLevel -= OnWinLevel;
        LevelChanger.LevelStart -= ShowGameWindow;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CloseAllWin();
            loseWindow.gameObject.SetActive(false);
            TimeManager.Instance.SetTimeScale(1f, 0f);
            PlayerRespawn?.Invoke();
        }
        heightSlider.value = Mathf.Lerp(heightSlider.value,_blocksFall, 0.1f);
        foreach (var item in _bonusTimers)
        {
            if (item.Tick())
            {
                Destroy(item.GetVisualPresenter());
                _bonusTimers.Remove(item);
                return;
            }
        }
    }

    private void BlockOnBlockFall(Vector2 obj)
    {
        _blocksFall++;
    }

    private void ShowGameWindow(Level lvl)
    {
        _floorCount = (int)lvl.HeightToWin;
        _blocksFall = 0;
        _levelId = lvl.LevelNum;
        heightSlider.maxValue = _floorCount*Level.Instance.Width*Level.Instance.Lenght;
        heightSlider.value = 0;
        currentLevel.text = string.Format(levelFormat, _levelId);
        TimeManager.Instance.Play();
        ShowWindow(gameWindow);
    }

    private void UpdateText(int obj)
    {
        obj -= 1;
        currentHeight.text = string.Format(format, obj, _floorCount);
    }

    private void ShowLooseWindow()
    {
        SoundPlayer.Instance.PlayClip(loseSound);
        currentLevelInLoseWindow.text = string.Format(levelFormat, _levelId);
        StartCoroutine("DeathCoroutine", 2f);
    }
    
    private IEnumerator DeathCoroutine(float time)
    {
        TimeManager.Instance.SetTimeScale(0.00001f, 100000);
        yield return new WaitForSeconds(time);        
        ShowWindow(loseWindow);
    }
    
    private void OnWinLevel()
    {
        SoundPlayer.Instance.PlayClip(winSound);
        currentLevelInWinWindow.text = string.Format(levelFormat, _levelId);
        ShowWindow(winWindow);    
    }
    
    private void ShowWindow(Canvas win)
    {
        //TimeManager.Instance.Pause();
        CloseAllWin();
        win.gameObject.SetActive(true);
    }

    private void CloseAllWin()
    {
        if (_allWindows != null)
        {
            foreach (var window in _allWindows)
            {
                window.gameObject.SetActive(false);
            }
        }
    }
}
