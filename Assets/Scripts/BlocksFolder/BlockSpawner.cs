using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlocksFolder
{
    [DefaultExecutionOrder(-10)]
    public class BlockSpawner : MonoBehaviour
    {
        public static BlockSpawner Instance { get; private set; }
        
        public static event Action<int> FloorStart;
        
        private BlockChance[] _blocks;
        private ColorPattern _colorPattern = ColorPattern.Random;
        private Color[] _colors = new []{Color.white};
        private bool isEven = false;
        private float _secondForSpawn;
        private float _currentTime = 0f;
        private float _speed;
        private int floor = 0;
        private readonly List<Vector2> _spawnPositionList = new();
        private readonly List<Vector2> _fallPositionList = new();

        private void OnEnable()
        {
            Block.BlockFall += OnBlockFall;
        }

        private void OnDisable()
        {
            Block.BlockFall -= OnBlockFall;
        }

        private void OnBlockFall(Vector2 obj)
        {
            _fallPositionList.Add(obj);
            if (_fallPositionList.Count >= Level.Instance.Width * Level.Instance.Lenght)
            {
                _fallPositionList.Clear();
                FloorStart?.Invoke(floor);
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            AddBlockFloor();
            FloorStart?.Invoke(floor);
        }

        private void Update()
        {
            if (_blocks == null || _blocks.Length == 0 || floor-1 >= Level.Instance.HeightToWin) return;
            
            if (_currentTime >= _secondForSpawn)
            {
                var blockChance = _blocks.OrderBy(b => b.Chance).FirstOrDefault(b => b.Chance > Random.value);
                SpawnBlock(blockChance, SelectPositionForSpawn());
                _currentTime = 0f;
            }
            else
            {
                _currentTime += Time.deltaTime*TimeManager.Instance.GeneralSpeed;
            }
        }

        public void RefundBlock(Vector2 pos)
        {
            _spawnPositionList.Add(pos);
        }

        public void UpdateBlocksChances(BlockChance[] blocks)
        {
            _blocks = blocks;
            if (blocks.Length == 0)
            {
                
                Ui.Instance.StartBossTimer();
            }
        }
        
        public void UpdateColorPattern(ColorPattern pattern, Color[] colors)
        {
            _colorPattern = pattern;
            _colors = colors;
        }

        public void UpdateSpawnRate(float rate)
        {
            _secondForSpawn = rate;
        }

        private void AddBlockFloor()
        {
            for (int i = 0; i < Level.Instance.Width; i++)
            {
                for (int j = 0; j < Level.Instance.Lenght; j++)
                {
                    _spawnPositionList.Add(new Vector2(i,j));
                }
            }
            floor++;
        }

        private void RemovePositionForSpawn(Vector2 pos)
        {
            _spawnPositionList.Remove(pos);
            if (_spawnPositionList.Count == 0)
            {
                AddBlockFloor();
            }
        }
        
        private void SpawnBlock(BlockChance newBlock, Vector3 pos)
        {
            MaterialPropertyBlock blockColor = new MaterialPropertyBlock();
            var block = Instantiate(newBlock.Block, pos+Vector3.up*10, new Quaternion(), transform);
            block.Init(newBlock);
            var renderer = block.GetComponentInChildren<Renderer>();
            renderer.GetPropertyBlock(blockColor);
            switch (_colorPattern)
            {
                case ColorPattern.Random:
                    blockColor.SetColor("_Color", _colors[Random.Range(0, _colors.Length)]);
                    break;
                case ColorPattern.Even:
                    if (isEven)
                    {
                        blockColor.SetColor("_Color", _colors[0]);

                    }
                    else
                    {
                        blockColor.SetColor("_Color", _colors[1]);
                    }

                    isEven = !isEven;
                    break;
                case ColorPattern.Solid:
                    blockColor.SetColor("_Color", _colors[0]);
                    break;
            }
            renderer.SetPropertyBlock(blockColor);
        }

        private Vector3 SelectPositionForSpawn()
        {
            Level lvl = Level.Instance;

            Vector2 pos = _spawnPositionList[Random.Range(0, _spawnPositionList.Count)];
            RemovePositionForSpawn(pos);
            
            return new Vector3(pos.x * lvl.WidthOfSquare+lvl.WidthOfSquare*0.5f, lvl.MaxHeight, pos.y * lvl.WidthOfSquare + lvl.WidthOfSquare*0.5f);
        }
    }
}
