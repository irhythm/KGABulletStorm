using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMoveController : MonoBehaviour
{
    [SerializeField] private LayerMask _clickableLayer;
    [SerializeField] private Camera _playerCamera;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotateSpeed = 10f;

    private Vector3 _targetPosition;
    private bool _hasTarget = false;

    Ray ray; 
    private void Awake()
    {
        if(_playerCamera == null)
        {
            _playerCamera = Camera.main;
        }

        _targetPosition = transform.position;
    }


    private void HandleMouseInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hitInfo, 100f, _clickableLayer))
            {
                _targetPosition = hitInfo.point;
                _hasTarget = true;
            }
        }
    }

    //시각적 테스트용 기즈모 함수, 이동 목표 위치에 구를 그려줄 예정
    private void OnDrawGizmos()
    {
        if(_hasTarget)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, (_targetPosition - transform.position));
            Gizmos.DrawSphere(_targetPosition+ Vector3.up *0.3f, 0.2f);
        }
    }


    void MoveToTarget()
    {
        if (!_hasTarget) //==false)
        {
            return;
        }

        //이동하고자 하는 방향
        Vector3 direction = _targetPosition - transform.position;

        direction.y = 0f;
        //distance 계산 
        float distance = direction.sqrMagnitude;
        //sqrMagnitude은 

        //아직 거리가 층분히멀면
        if (distance > 0.0025f )
        {

            //회전
            Quaternion targetRot = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _rotateSpeed * Time.deltaTime);
            //transform rotation은 자동 갱신 되니까 괜찮다

            //이동
            Vector3 move = direction.normalized *_moveSpeed * Time.deltaTime;
            if (move.magnitude > distance)
            {
                move = direction.normalized * distance;
                }
            transform.position += move;
        }

        else
        {
            _hasTarget = false;
        }

    }


    void Update()
    {
        HandleMouseInput();
        MoveToTarget();
    }


}


