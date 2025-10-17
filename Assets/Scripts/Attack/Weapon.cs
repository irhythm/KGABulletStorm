using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _fireRate = 0.5f;
    private float _fireCooldownTimer = 0f;


    private Vector3 _weaponFirePosition;

    // Start is called before the first frame update
    void Start()
    {
        
        _fireCooldownTimer = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        _fireCooldownTimer += Time.deltaTime;
        

    }



    public void FireBullet()
    {

        Bounds weaponBounds = GetComponent<MeshRenderer>().bounds;
        _weaponFirePosition = weaponBounds.center + new Vector3(0, 0, weaponBounds.extents.z);
        if (Input.GetKey(KeyCode.Space) && _fireCooldownTimer >= _fireRate)
        {
            _fireCooldownTimer = 0f;
            Instantiate(_bulletPrefab, _weaponFirePosition, _bulletPrefab.transform.rotation);

        }

        


    }


}
