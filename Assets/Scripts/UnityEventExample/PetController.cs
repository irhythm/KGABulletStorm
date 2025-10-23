using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PetController : MonoBehaviour, IPlayerObserver
{
    [SerializeField] private Player _player;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _moveStopDistance;

    //private Coroutine _moveCoroutine;
    //private Transform _fixedYTransform;
    private Vector3 _fixedYPosition;

    //public struct sType
    //{
    //    public int a;
    //    public float b;
    //}

    Ray ray;


    private void Awake()
    {
        //ray = Physics.Ray(Vector3.forward,
        _player = FindObjectOfType<Player>();
        Init();
    }

    private void OnDestroy()
    {
        _player.RemoveObserver(this);
    }

    private void Init()
    {
        //이거는 코드로   유니티 이벤트에 리스너 등록하는 방법 즉 이벤트에 메서드 추가임
        _player.AddObserver(this);
    }

    void Update()
    {
        if (_player == null || _player.IsPetSpawned == false)
        {
            Destroy(gameObject);
        }

        //if (Physics.Raycast(transform.position))


        transform.rotation = _player.transform.rotation;


        if (Vector3.Distance(transform.position, _player.transform.position) > _moveStopDistance)
        {
            MoveToPlayer();
        }

    }

    //플레이어의 유니티 이벤트에 연결해줄 코루틴 실행 함수, 코루틴이 중복 실행되지 않도록 조건 추가함
    public void MoveToPlayer()
    {
        if (true)
        {
            //_fixedYTransform = _player.transform;
            _fixedYPosition = _player.transform.position;
            _fixedYPosition.y = transform.position.y;
            transform.position = Vector3.MoveTowards(
                transform.position,
                _fixedYPosition,
                _moveSpeed * Time.deltaTime
                );
            //_moveCoroutine = StartCoroutine(MoveToTarget(_fixedYPosition));
        }
    }

    //플레이어를 따라서 이동하는데 정해진 간격까지 이동하면 멈추도록 하는 코루틴 함수
    //private IEnumerator MoveToTarget(Vector3 target)
    //{
    //    while (true)
    //    {
    //        float distance = Vector3.Distance(target, transform.position); //Distance is positive value

    //        if (distance <= _moveStopDistance)
    //        {
    //            //_moveCoroutine = null;
    //            yield break;
    //        }

    //        transform.position = Vector3.MoveTowards(
    //            transform.position,
    //            target,
    //            _moveSpeed * Time.deltaTime
    //            );

    //        yield return null;
    //    }
    //}

    public void OnPlayerHpChanged(float curHp, float maxHp)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPlayerStateChange()
    {
        //throw new System.NotImplementedException();
    }

    public void OnPlayerPositionChanged()
    {
        MoveToPlayer();
    }
}