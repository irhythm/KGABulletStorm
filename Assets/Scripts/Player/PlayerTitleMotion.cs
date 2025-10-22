using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTitleMotion : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _moveRange = 20f;
    [SerializeField] private float _rotateSpeed = 10f;
    //[SerializeField] private Transform _startTransform;

    [SerializeField] private float _increaseRate = 0.1f;
    [SerializeField] private float _howLongUporDown = 20f;
    //private float _timeline;
    private float _passedTime = 0;

    private Vector3 _currentPosition;

    private Vector3 _initialPosition;
    //private Vector3 _initialPosition;
    private Vector3 _targetPosition;
    private string _upOrDown = "up";

    // Start is called before the first frame update
    void Start()
    {
        _currentPosition = transform.position;
        _initialPosition = transform.position;
        _targetPosition = _currentPosition + Vector3.up * _moveRange;

        StartCoroutine(GoUpOrDown());


    }

    private IEnumerator GoUpOrDown()
    {
        while (true)
        {
            string upordown = _upOrDown;
            //_timeline = 1 - _passedTime;

            if (upordown == "up")
            {
                transform.position = Vector3.Lerp(_currentPosition + Vector3.up * _passedTime * _moveSpeed, _targetPosition, _increaseRate);
            }
            else if (upordown == "down")
            {
                transform.position = Vector3.Lerp(_currentPosition - Vector3.up * _passedTime * _moveSpeed, _initialPosition, _increaseRate);
            }

            yield return null;
        }
    }




    void ChangeDirection()
    {
        if (_upOrDown == "up")
        {
            _upOrDown = "down";
        }
        else if (_upOrDown == "down")
        {
            _upOrDown = "up";
        }
    }


    // Update is called once per frame
    void Update()
    {
        
        _passedTime += Time.deltaTime;
        if (_passedTime >= _howLongUporDown)
        {
            
                //시간이 층분할것으로 기대
             _currentPosition = transform.position;
            
            _passedTime = 0;
            ChangeDirection();
            //StartCoroutine(GoUpOrDown(_upOrDown));
            //StartCoroutine(GoDown());
        }

        transform.Rotate(Vector3.up, _rotateSpeed*Time.deltaTime);




    }
}
