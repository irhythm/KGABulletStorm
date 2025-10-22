using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform _initialPosition;
    [SerializeField] Transform _boostCameraPosition;
    [SerializeField] private float _speedUpEffect = 400f;
    [SerializeField] private float _cameraSpeed = 10f;
    // Start is called before the first frame update
    [SerializeField] GameObject _playerObject;
    [SerializeField] private float _dashDurationBoostMultiplier = 0.5f;
    [SerializeField] private float _dashCooldownBoostMultiplier = 0.5f;


    private float _initialSpeedOfEnemiesAndTerrain = 0;
    private float _initialDashCooldown = 0;
    private float _initialDashDuration = 0;
    public static bool _isBoosting = false;

    void Start()
    {
        
        
        _initialSpeedOfEnemiesAndTerrain = ObjectManager.Instance.EnemyPrefabs[0].GetComponent<Enemy>().MoveSpeed;
        _initialDashCooldown = _playerObject.GetComponent<MoveComponent>().DashCooldown;
        _initialDashDuration = _playerObject.GetComponent<MoveComponent>().DashDuration;
    }

    private IEnumerator CameraMovementCoroutine(Transform end)
    {
        transform.position = Vector3.MoveTowards(transform.position, end.position, _cameraSpeed);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, end.rotation, _cameraSpeed);
        yield return null;

        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _isBoosting = !_isBoosting;

            StartCoroutine(CameraMovementCoroutine(_boostCameraPosition));
            
            foreach (var enemy in ObjectManager.Instance.EnemyPrefabs)
            {
                if (enemy.activeSelf && enemy.GetComponent<Enemy>().DeathAction ==0 && !enemy.GetComponent<Enemy>().IsKnockbacked)
                {
                    //enemy.GetComponent<Enemy>().MoveSpeed = _speedUpEffect;
                    enemy.GetComponent<Rigidbody>().velocity = Vector3.back * _speedUpEffect;
                }
            }
            foreach (var terrain in ObjectManager.Instance.TerrainPrefabs )
            {
                if (terrain.activeSelf && terrain.GetComponent<Terrain>().DeathAction ==0 && !terrain.GetComponent<Terrain>().IsKnockbacked)
                {
                    //terrain.GetComponent<Terrain>().MoveSpeed = _speedUpEffect;
                    terrain.GetComponent<Rigidbody>().velocity = Vector3.back * _speedUpEffect;
                }
            }
            _playerObject.GetComponent<MoveComponent>().DashCooldown = _initialDashCooldown * _dashCooldownBoostMultiplier;
            _playerObject.GetComponent<MoveComponent>().DashDuration = _initialDashDuration * _dashDurationBoostMultiplier;

            if (transform.position == _boostCameraPosition.position && transform.rotation == _boostCameraPosition.rotation)
            {
                StopCoroutine(CameraMovementCoroutine(_boostCameraPosition));
            }

        }

        else
        {
            _isBoosting = false;
            StartCoroutine(CameraMovementCoroutine(_initialPosition));

            //foreach (var enemy in ObjectManager.Instance.EnemyPrefabs)
            //{
            //    if (enemy.activeSelf)
            //    {
            //        enemy.GetComponent<Enemy>().MoveSpeed = _initialSpeedOfEnemiesAndTerrain;
            //        enemy.GetComponent<Rigidbody>().velocity = Vector3.back * _initialSpeedOfEnemiesAndTerrain;
            //    }
            //}
            //foreach (var terrain in ObjectManager.Instance.TerrainPrefabs)
            //{
            //    if (terrain.activeSelf)
            //    {
            //        terrain.GetComponent<Terrain>().MoveSpeed = _initialSpeedOfEnemiesAndTerrain;
            //        terrain.GetComponent<Rigidbody>().velocity = Vector3.back * _initialSpeedOfEnemiesAndTerrain;
            //    }
            //}

            _playerObject.GetComponent<MoveComponent>().DashCooldown = _initialDashCooldown;
            _playerObject.GetComponent<MoveComponent>().DashDuration = _initialDashDuration;


            if (transform.position == _initialPosition.position && transform.rotation == _initialPosition.rotation)
            {
                StopCoroutine(CameraMovementCoroutine(_initialPosition));
            }
        }



    }
}
