using System;
using BlocksFolder;
using DefaultNamespace;
using UniRx;
using UnityEngine;
using DG.Tweening;
using Levels;
using Player;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    public Vector3 offset;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private Transform platform;
    private Transform background;

    private Vector3 _rotatePoint;
    //private Quaternion _targetRotation = Quaternion.Euler(50, 0,0);
    private float _targetRotation;
    private float _targetHeight;
    private float currentAngle;
    private CompositeDisposable _disposable = new ();

    public void UpCamera(int floor)
    {
        _targetHeight = floor-1 * Level.Instance.WidthOfSquare;
        
    }

    public void Rotate(float angle)
    {
        //_targetRotation = Quaternion.Euler(50, _targetRotation.eulerAngles.y + angle, 0);
        _targetRotation += angle;

    }

    public void ChangeBackground(Transform back)
    {
        background = back;
    }

    public void LookToEndLevel(float delay, Transform obj)
    {
        var timer = delay;

        var tmpPl = platform;
        var tmpRt = _rotatePoint;        
        transform.position = obj.position + offset;
        //_rotatePoint = platform.position;
        transform.LookAt(obj);
        //_targetRotation = transform.rotation.y;
        
        Observable.EveryUpdate().Subscribe(_ =>
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                transform.position = platform.position + offset;
                _targetRotation = 0;
                _rotatePoint = tmpRt;
                _disposable.Clear();
            }

        }).AddTo(_disposable);
    }
    
    private void Awake()
    {
        Instance = this;
    }

    private void LateUpdate()
    {
        var rotationDir = _targetRotation-transform.rotation.y;
        var angle = (rotationSpeed * Time.deltaTime * 10) * rotationDir;
        _targetRotation -= angle;
        transform.RotateAround(_rotatePoint, Vector3.up, angle);
        if(background != null) background.rotation = Quaternion.Euler(45,transform.rotation.eulerAngles.y,0);
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, _targetHeight+offset.y , transform.position.z),speed * Time.deltaTime);
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Rotate(90);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Rotate(-90);
        }
    }

    private void Start()
    {
        var lvl = Level.Instance;

        offset = new Vector3(platform.position.x + lvl.Width*0.5f, offset.y, offset.z);
        
        transform.position = platform.position + offset;
        _rotatePoint = new Vector3(lvl.Width * lvl.WidthOfSquare * .5f, 0, lvl.Lenght * lvl.WidthOfSquare * .5f);
    }

    private void OnEnable()
    {
        //LevelChanger.RestartGame += OnLevelRestart;
        BlockSpawner.FloorStart += UpCamera;
        LevelChanger.LevelStart += OnLevelStart;
    }

    private void OnDisable()
    {
        //LevelChanger.RestartGame -= OnLevelRestart;
        BlockSpawner.FloorStart -= UpCamera;
        LevelChanger.LevelStart -= OnLevelStart;
    }
    
    private void OnLevelStart(Level obj)
    {
        platform = obj.transform;
        background = obj.background;
        ResetCamera();
    }

    private void OnLevelRestart()
    {
        ResetCamera();
    }

    private void ResetCamera()
    {
        var lvl = Level.Instance;
        offset = new Vector3(platform.position.x + lvl.Width*0.5f, offset.y, offset.z);
        _targetRotation = 0;
        transform.position = platform.position + offset;
        transform.rotation = Quaternion.Euler(50, 0, 0);
    }
}
