using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(InputComponent))] //InputComponent가 없으면 자동으로 추가해줌
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

    [SerializeField] private float _dashDistance = 2f;
    [SerializeField] private float _dashCooldown = 2f;
    //[SerializeField] private float _dashSpeed = 0.5f;
    [SerializeField] private float _dashDuration = 0.2f;
    //[SerializeField] private float _dashInterpolation = 0.1f;
    [SerializeField] private float _dashBarrelRollSpeed = 5f;
    [SerializeField] private float _dashColliderShrinkFactor = 2f;
    [SerializeField] private float _dashAfterDashShrinkRevertDelay = 0.1f;


    private InputComponent _inputComponent;
    private Vector3 _minWorldBounds;
    private Vector3 _maxWorldBounds;
    private Vector3 _playerExtents;
    private Vector3 _dashStartPosition;
    private Vector3 _dashEndPosition;
    private float _currentRotationYZ = 0f;
    private float _currentDashTime = 0f;
    private float _currentDashCooldown = 0f;
    private int _dashDirection = 0; //-1 left, 1 right




    //private float _dashCooldownTimer = 0f;
    private IEnumerator PlayerColliderTempSmaller()
    {

        float _extraTime = 0;
        SphereCollider playerCollider = GetComponent<SphereCollider>();
        float originalRadius = gameObject.GetComponent<SphereCollider>().radius;
        gameObject.GetComponent<SphereCollider>().radius = originalRadius /_dashColliderShrinkFactor ; //절반 크기로 줄임
        yield return new WaitForSeconds(_dashDuration);
        while (_extraTime < _dashAfterDashShrinkRevertDelay)
        {
            _extraTime += Time.deltaTime;
            yield return null;
        }
        gameObject.GetComponent<SphereCollider>().radius = originalRadius;
    }

    void Dash()
    {
        if (GameManager.Instance.IsPlaying == false)
        {
            return;
        }


        if (_inputComponent.LeftClickInput  && _currentDashTime <= 0 && _currentDashCooldown <=0)
        {
            _currentDashTime = _dashDuration;
            _currentDashCooldown = _dashCooldown;
            _dashDirection = -1;
            _dashStartPosition = transform.position;
            _dashEndPosition = transform.position + (-1f) * _dashDistance * Vector3.right;
            StartCoroutine(PlayerColliderTempSmaller());
        }
        if (_inputComponent.RightClickInput  && _currentDashTime <= 0 && _currentDashCooldown <=0)
        {
            _currentDashTime = _dashDuration;
            _currentDashCooldown = _dashCooldown;
            _dashDirection = 1;
            _dashStartPosition = transform.position;
            _dashEndPosition = transform.position + (1f) * _dashDistance * Vector3.right;
            StartCoroutine(PlayerColliderTempSmaller());
        }
        if (_currentDashTime > 0)
        {

            float _timeline = 1 - (_currentDashTime / _dashDuration);

            float _rotationDelta = 360f* (-1f)*_dashDirection * _dashBarrelRollSpeed * Time.deltaTime / _dashDuration;

            if (_dashDirection == -1)            
                {
                //transform.Rotate(Vector3.forward, 90f);
                
                
                transform.Rotate(Vector3.forward, _rotationDelta);
                
                
                
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 180f), _dashBarrelRollSpeed);
                //Vector3 dashMovement = (-1f) * _dashDistance * Time.deltaTime * Vector3.right / _dashDuration;
                //Vector3 afterDash = transform.position + dashMovement;
                _dashEndPosition.x = Mathf.Clamp(_dashEndPosition.x, _minWorldBounds.x + _xBoundValue * _playerExtents.x, _maxWorldBounds.x - _xBoundValue * _playerExtents.x);
                transform.position = Vector3.Lerp(_dashStartPosition, _dashEndPosition, _timeline );
            }
            else if (_dashDirection == 1)
            {
                //transform.Rotate(Vector3.forward, -90f);
                //transform.Rotate(Vector3.forward, -90f);
                
                transform.Rotate(Vector3.forward, _rotationDelta);
              
                
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, -180f), _dashBarrelRollSpeed);
                //Vector3 dashMovement = (1f) * _dashDistance * Time.deltaTime * Vector3.right / _dashDuration;
                //Vector3 afterDash = transform.position + dashMovement;
                _dashEndPosition.x = Mathf.Clamp(_dashEndPosition.x, _minWorldBounds.x + _xBoundValue * _playerExtents.x, _maxWorldBounds.x - _xBoundValue * _playerExtents.x);
                transform.position = Vector3.Lerp(_dashStartPosition, _dashEndPosition, _timeline );
                
            }
            _currentDashTime -= Time.deltaTime;
            if (_currentDashTime == 0f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), _dashBarrelRollSpeed);
                _dashDirection = 0;
            }
        }
        
        _currentDashCooldown -= Time.deltaTime;
    }



    void Start()
    {
        _inputComponent = GetComponent<InputComponent>();
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
        //현재 게임이 진행중이지 않으면 리턴
        if (GameManager.Instance.IsPlaying == false)
        {
            return;
        }


        if (_currentDashTime >0)
        {
            return;
        }
        Vector3 inputVec = new Vector3(_inputComponent.HorInput, 0, _inputComponent.VerInput).normalized;
        Vector3 deltaMovement = inputVec * _moveSpeed * Time.deltaTime; //벡터를 맨뒤에 옮겨놓으면 성능이 조금 좋아짐
        //데이터 가격이 싼 순서대로 곱셈을 하는게 좋음
        Vector3 nextPosition = transform.position + deltaMovement;
        //Vector3 dashNextPosition;

        float xGap = _xBoundValue *_playerExtents.x;
        float zGap = _zBoundValue *_playerExtents.z;
        //if (_inputComponent.LeftClickInput > 0 && _dashCooldownTimer <= 0)
        //{
        //    dashNextPosition = transform.position;
        //    dashNextPosition.x = transform.position.x + (-1f) * _dashDistance;

        //    dashNextPosition.x = Mathf.Clamp(dashNextPosition.x, _minWorldBounds.x + xGap, _maxWorldBounds.x - xGap);
        //    for (int i = 0; i < _dashDuration; i++) //대쉬할때 부드럽게 이동하게
        //    {
        //    transform.position = Vector3.Lerp(transform.position, dashNextPosition, _dashSpeed);
        //    }

        //    _dashCooldownTimer = _dashCooldown;
        //}

        //else if (_inputComponent.RightClickInput > 0 && _dashCooldownTimer <= 0)
        //{
        //    dashNextPosition = transform.position;
        //    dashNextPosition.x = transform.position.x + (1f) * _dashDistance;

        //    dashNextPosition.x = Mathf.Clamp(dashNextPosition.x, _minWorldBounds.x + xGap, _maxWorldBounds.x - xGap);

        //    for (int i = 0; i < _dashDuration; i++) //대쉬할때 부드럽게 이동하게
        //    {
        //        transform.position = Vector3.Lerp(transform.position, dashNextPosition, _dashSpeed);
        //    }
        //    _dashCooldownTimer = _dashCooldown;
        //}

        //_dashCooldownTimer -= Time.deltaTime;

        //Mathf.Clamp(값, 최소값, 최대값) : 값을 최소값과 최대값 사이로 제한
        nextPosition.x = Mathf.Clamp(nextPosition.x, _minWorldBounds.x + xGap, _maxWorldBounds.x - xGap);
        nextPosition.z= Mathf.Clamp(nextPosition.z, _minWorldBounds.z + zGap-1.6f, _maxWorldBounds.z - zGap-1.6f);

        
        



        if (_inputComponent.HorInput != 0)
        {
            
            float rotationDelta = _inputComponent.HorInput * _rotationSpeed * Time.deltaTime;
            
            _currentRotationYZ += rotationDelta;
            _currentRotationYZ = Mathf.Clamp(_currentRotationYZ, _rotationLimitMinus, _rotationLimitPlus);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x,_currentRotationYZ, _currentRotationYZ*(-0.1f));
            //transform.Rotate(Vector3.up, _currentRotationYZ);

        }

        if (_inputComponent.HorInput == 0 && _currentDashTime <=0) //입력이 없을때 원래대로 돌아오게
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), _rotationSpeed * Time.deltaTime);
        }


        //if (_inputComponent.LeftClickInput == 0 && _inputComponent.RightClickInput == 0)

        //{
        //    transform.position = nextPosition;
        //}

        transform.position = nextPosition;

    }
    

    // Update is called once per frame
    void Update()
    {
        Move();
        Dash();
    }
}
