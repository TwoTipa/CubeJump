using System;
using System.Collections.Generic;
using System.Timers;
using BlocksFolder;
using DefaultNamespace;
using ObstacleF;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BlockBehaviourChanger), typeof(ObstacleBehaviorChanger)), DefaultExecutionOrder(20)]
public class FloorEvent : MonoBehaviour
{
    [SerializeField] private int floor;
    [SerializeField] private UnityEvent[] floorStart;
    [SerializeField] private List<float> partsDelay = new List<float>();
    private int _part = 0;
    CompositeDisposable disposable = new();

        
    private void Awake()
    {
        BlockSpawner.FloorStart += OnFloorStart; 
    }

    private void OnDestroy()
    {
        BlockSpawner.FloorStart -= OnFloorStart;
    }

    private void OnDisable()
    {
        disposable.Clear();
    }

    private void OnValidate()
    {
        if (floorStart.Length <= 1)
        {
            return;
        }

        ChangeDelayCount(floorStart.Length-1);
    }

    private void ChangeDelayCount(int count)
    {
        if (partsDelay.Count > count)
        {
            partsDelay = partsDelay.GetRange(0, Mathf.Min(partsDelay.Count, count));
        }
        else
        {
            for (int i = partsDelay.Count; i < count; i++)
            {
                partsDelay.Add(new float());
            }
        }

    }
    
    private void OnFloorStart(int obj)
    {
        if (floor != obj) return;
        FloorStarter();
        if(floorStart.Length == 1) return;
        float timer = 0f;
        
        Observable.EveryUpdate().Subscribe(_ =>
        {
            timer += Time.deltaTime * TimeManager.Instance.GeneralSpeed;
            if (!(timer >= partsDelay[_part]))
            {
                return;
            }

            timer = 0f;
            _part++;
            FloorStarter();
            if(_part+1 >= floorStart.Length) disposable.Clear();
        }).AddTo(disposable);
    }

    private void FloorStarter()
    {
        Ui.Instance.SetBossTimer(0);
        floorStart[_part]?.Invoke();
        if (partsDelay.Count > _part)
        {
            Ui.Instance.SetBossTimer(partsDelay[_part]);
        }
    }
}