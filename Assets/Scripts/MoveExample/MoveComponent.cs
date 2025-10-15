using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(InputComponent))] //InputComponent�� ������ �ڵ����� �߰�����
public class MoveComponent : MonoBehaviour
{

    [SerializeField] private GameObject _movingPlane;
    //[SerializeField] private InputComponent _inputCompontent;
    [SerializeField] private float _moveSpeed = 20;
    [SerializeField] private float _rotationSpeed = 700f;
    [SerializeField] private float _rotationLimitPlus = 45f;
    [SerializeField] private float _rotationLimitMinus = -45f;

    [SerializeField] private float _xBoundValue = 2f;
    [SerializeField] private float _zBoundValue = 2f;
    

    private InputComponent _inputCompontent;
    private Vector3 _minWorldBounds;
    private Vector3 _maxWorldBounds;
    private Vector3 _playerExtents;
    private float _currentRotationYZ = 0f;

    void Start()
    {
        _inputCompontent = GetComponent<InputComponent>();
        SphereCollider playerCollider = GetComponent<SphereCollider>();

        if(_movingPlane != null)
        {
            Bounds planeBounds = _movingPlane.GetComponent<MeshRenderer>().bounds;

            _minWorldBounds = planeBounds.center - planeBounds.extents;
            _maxWorldBounds = planeBounds.center + planeBounds.extents;
        }

        if (playerCollider != null)
        {
            _playerExtents = playerCollider.bounds.extents;
        }

    }

    private void Move()
    {
        Vector3 inputVec = new Vector3(_inputCompontent.HorInput, 0, _inputCompontent.VerInput).normalized;
        Vector3 deltaMovement = inputVec * _moveSpeed * Time.deltaTime; //���͸� �ǵڿ� �Űܳ����� ������ ���� ������
        //������ ������ �� ������� ������ �ϴ°� ����
        Vector3 nextPosition = transform.position + deltaMovement;

        float xGap = _xBoundValue *_playerExtents.x;
        float zGap = _zBoundValue *_playerExtents.z;

        //Mathf.Clamp(��, �ּҰ�, �ִ밪) : ���� �ּҰ��� �ִ밪 ���̷� ����
        nextPosition.x= Mathf.Clamp(nextPosition.x, _minWorldBounds.x + xGap, _maxWorldBounds.x - xGap);
        nextPosition.z= Mathf.Clamp(nextPosition.z, _minWorldBounds.z + zGap-1.6f, _maxWorldBounds.z - zGap-1.6f);

        



        if (_inputCompontent.HorInput != 0)
        {
            float rotationDelta = _inputCompontent.HorInput * _rotationSpeed * Time.deltaTime;
            
            _currentRotationYZ += rotationDelta;
            _currentRotationYZ = Mathf.Clamp(_currentRotationYZ, _rotationLimitMinus, _rotationLimitPlus);
            transform.rotation = Quaternion.Euler(0,_currentRotationYZ, _currentRotationYZ*(-0.1f));

        }

        if (_inputCompontent.HorInput == 0) //�Է��� ������ ������� ���ƿ���
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), _rotationSpeed * Time.deltaTime);
        }



        transform.position = nextPosition;


    }
    

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
