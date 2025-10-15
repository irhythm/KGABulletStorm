using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Vector3 _initialPosition;
    Vector3 _BoostCameraPosition;
    [SerializeField] private float _interpolation = 10f;
    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;
        _BoostCameraPosition = new Vector3(_initialPosition.x, _initialPosition.y + 2f, _initialPosition.z - 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position = Vector3.Lerp(_initialPosition, _BoostCameraPosition, _interpolation);
        }
        
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            transform.position = Vector3.Lerp(_BoostCameraPosition, _initialPosition, _interpolation);
        }



    }
}
