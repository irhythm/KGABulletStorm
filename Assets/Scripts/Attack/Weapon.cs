using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _fireRate = 0.5f;
    private float _fireCooldownTimer = 0f;


    private Vector3 _weaponFirePosition;
    private List<GameObject> _bulletsPool = new List<GameObject>();


    public GameObject BulletPrefab { get { return _bulletPrefab; } }

    // Start is called before the first frame update
    void Start()
    {
        _bulletsPool = ObjectManager.Instance.BulletPrefabs;
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
            BulletFromPool().SetActive(true);

            //Instantiate(_bulletPrefab, _weaponFirePosition, _bulletPrefab.transform.rotation);

        }

        


    }

    private GameObject BulletFromPool()
    {
        foreach (var bullet in _bulletsPool)
        {
            if (bullet.activeSelf == false)
            {
                bullet.transform.position = _weaponFirePosition;
                bullet.transform.rotation = _bulletPrefab.transform.rotation;
                return bullet;
                
            }
        }
        return null;
    }


}
