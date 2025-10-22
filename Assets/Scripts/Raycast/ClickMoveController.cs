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

    //�ð��� �׽�Ʈ�� ����� �Լ�, �̵� ��ǥ ��ġ�� ���� �׷��� ����
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

        //�̵��ϰ��� �ϴ� ����
        Vector3 direction = _targetPosition - transform.position;

        direction.y = 0f;
        //distance ��� 
        float distance = direction.sqrMagnitude;
        //sqrMagnitude�� 

        //���� �Ÿ��� �������ָ�
        if (distance > 0.0025f )
        {

            //ȸ��
            Quaternion targetRot = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _rotateSpeed * Time.deltaTime);
            //transform rotation�� �ڵ� ���� �Ǵϱ� ������

            //�̵�
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


