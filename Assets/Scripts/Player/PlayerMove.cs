using System;
using BlocksFolder;
using DefaultNamespace;
using UniRx;
using UnityEngine;

namespace Player
{
    [DefaultExecutionOrder(300)]
    public class PlayerMove : MonoBehaviour
    {
        public float Height { get; private set; }

        public static event Action<Vector3> OnPlayerMove;
        public Vector2 Pos => _pos;
        
        
        [SerializeField] private int jumpForce = 1;
    
        private Vector2 _pos;
        private Vector2 _mousePastPos;
        private bool _isSwipeOver = false;
        private bool _isMove = false;
        private bool _isMoveBlock = false;
        private float _maxHeight = -1;
        private Rigidbody _rigidbody;

        private CompositeDisposable _disposable = new CompositeDisposable();

        public void UpdatePlayerPosition()
        {
            Move(new Vector2Int());
        }

        public void blockMove()
        {
            _isMoveBlock = true;
        }
        
        public void SetPosition(Vector2 newPos)
        {
            _disposable.Clear();
            CompositeDisposable timer = new CompositeDisposable();
            float time = 0.001f;
            Observable.EveryUpdate().Subscribe(_ =>
            {
                time -= Time.deltaTime;
                if (time <= 0)
                {
                    TryCheckHeight(newPos, out var height);
                    transform.position = new Vector3(newPos.x, height, newPos.y);
                    Height = height;
                    _pos = newPos;
                    _isMove = false;
                    _isMoveBlock = false;
                    timer.Clear();
                }
            }).AddTo(timer);
        }

        private void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            transform.localScale = Vector3.one*Level.Instance.WidthOfSquare;
            _pos = new Vector2(transform.position.x, transform.position.z)*Level.Instance.WidthOfSquare;
            _isMove = false;
        }
        
        private void Update()
        {
            if(_isMoveBlock)return;
            if (TimeManager.Instance.GeneralSpeed <= 0) return;
            SwipeControl();
            ButtonControl();
        }

        private void ButtonControl()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Move(new Vector2(0, -1));
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                Move(new Vector2(0, 1));
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Move(new Vector2(1, 0));
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                Move(new Vector2(-1, 0));
            }
        }
        
        private void SwipeControl()
        {
            if (Input.GetMouseButtonUp(0))
            {
                _isSwipeOver = false;
                _mousePastPos = Vector2.zero;
            }
            if (!Input.GetMouseButton(0) || _isSwipeOver)
            {
                return;
            }

            if (!CheckMagnitude())
            {
                Vector2 dir = DirCalculate();
                
                Move(dir);
            }
        }
        
        private void Move(Vector2 dir)
        {
            if (_isMove) return;
            if (Time.timeScale <= 0) return;
            _isSwipeOver = true;
            var lvl = Level.Instance;
            Vector2 newPos = _pos + dir;

            if (newPos.x < 0 || newPos.x >= lvl.Width*lvl.WidthOfSquare || newPos.y < 0 || newPos.y >= lvl.Lenght*lvl.WidthOfSquare)
            {
                BreakMove(Vector2.zero);
                return;
            }
            if (!TryCheckHeight(newPos, out var newHeight)) return;
            if (newHeight > (Math.Round(transform.position.y)+jumpForce)*lvl.WidthOfSquare)
            {
                BreakMove(Vector2.zero);
                return;
            }

            if (newHeight/lvl.WidthOfSquare > _maxHeight)
            {
                _maxHeight = newHeight/lvl.WidthOfSquare;
            }
            
            _pos = newPos;
            Height = newHeight;

            Vector3 endPosition = new Vector3(newPos.x, Height, newPos.y);
            OnPlayerMove?.Invoke((endPosition - transform.position));

            _rigidbody.rotation = Quaternion.AngleAxis(-Vector3.SignedAngle(new Vector3(endPosition.x,0,endPosition.z) - new Vector3(transform.position.x,0, transform.position.z), Vector3.forward, Vector3.up), Vector3.up);
            
            Observable.EveryFixedUpdate().Subscribe(_ =>
            {
                _isMove = true;
                TryCheckHeight(newPos, out var newHeight);
                Vector3 endPosition = new Vector3(newPos.x, newHeight, newPos.y);
                _rigidbody.MovePosition(Vector3.MoveTowards(transform.position,endPosition, 10*Time.fixedDeltaTime));
                if (Vector3.Distance(transform.position, endPosition) < 0.05f)
                {
                    _isMove = false;
                    _disposable.Clear();
                }
            }).AddTo(_disposable);
        }

        private bool TryCheckHeight(Vector2 pos, out float height)
        {
            var ray = new Ray(new Vector3(pos.x, Height+10, pos.y), -Vector3.up);
            if (Physics.Raycast(ray, out var hit ,Mathf.Infinity, LayerMask.GetMask("Block")))
            {
                if (!hit.collider.CompareTag("Block"))
                {
                    height = 0;
                    return false;
                }

                height = Mathf.Round(hit.point.y);
                return true;
            }
        
            height = 0;
            return false;
        }
        private void BreakMove(Vector2 oldPos)
        {
            //Debug.Log("Что-то не так");
            //Move(oldPos);
            //_animator.Play("FailMove");
        }
        
        private bool CheckBlockPresence(Vector3 pos, Vector3 dir)
        {
            var ray = new Ray(pos, dir);
            Debug.DrawRay(pos, dir, Color.red);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, LayerMask.GetMask("Block")))
            {
                return hit.collider.CompareTag("Block");
            }

            return false;
        }

        private bool CheckMagnitude()
        {
            Vector2 mPos = new(Input.mousePosition.x, Input.mousePosition.y); 
            if (_mousePastPos == Vector2.zero)
            {
                _mousePastPos = mPos;
            }

            return (Input.mousePosition - new Vector3(_mousePastPos.x, _mousePastPos.y, 0)).magnitude < 50;
        }
    
        private Vector2 DirCalculate()
        {
            Vector2 mPos = new(Input.mousePosition.x, Input.mousePosition.y);

            Vector2 dir = mPos - _mousePastPos;

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                dir.y = 0;
            }
            else
            {
                dir.x = 0;
            }

            float angle = -CameraController.Instance.transform.rotation.eulerAngles.y;
            angle = RoundToNearestMultiple(angle, 90);
            _mousePastPos = mPos;
            Vector2 ret = (Quaternion.Euler(0,0, angle) * new Vector2((int)dir.normalized.x, (int)dir.normalized.y));
            return  ret;
        }

        private static float RoundToNearestMultiple(float num, float multiple)
        {
            double quotient = Math.Round(num / multiple);
            float roundedNum = (float)quotient * multiple;
            return roundedNum;
        }
    }
}
