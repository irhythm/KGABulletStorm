using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objectToSpawn;
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private GameObject _spawnRangePlane;
    // Start is called before the first frame update
    [SerializeField] private GameObject _player;
    [SerializeField] int _maxSpawnCount = 10;


    private float _minX;
    private float _maxX;
    private float _maxZ;
    private List<GameObject> enemiesOrTerrain = new List<GameObject>();
    private int totalObjectsIndex;

    void Start()
    {


        if (_spawnRangePlane != null && _objectToSpawn != null)
        {

            for (int i = 0; i < _maxSpawnCount; i++)
            {
                totalObjectsIndex = UnityEngine.Random.Range(0, _objectToSpawn.Count);
                GameObject temp = Instantiate(_objectToSpawn[totalObjectsIndex]); //transform.position, Quaternion.identity are removed to spawn at (0,0,0) with no rotation
                temp.SetActive(false);
                enemiesOrTerrain.Add(temp);
            }
            Bounds _xBounds = _spawnRangePlane.GetComponent<MeshRenderer>().bounds;
            _minX = _xBounds.center.x - _xBounds.extents.x;
            _maxX = _xBounds.center.x + _xBounds.extents.x;
            _maxZ = _xBounds.center.z + _xBounds.extents.z;
            StartCoroutine(SpawnObjectRoutine());
        }
        else
        {
            Debug.LogError("Spawner: Spawn Range Plane or Object to Spawn is not assigned.");
        }



    }


    private IEnumerator SpawnObjectRoutine()
    {
        while (true)
        {
            int totalObjectsIndex2 = UnityEngine.Random.Range(0, enemiesOrTerrain.Count);
            if (enemiesOrTerrain[totalObjectsIndex2].activeSelf == false && enemiesOrTerrain[totalObjectsIndex2].tag == "Enemy")
            {


                float randomX = UnityEngine.Random.Range(_minX, _maxX);
                enemiesOrTerrain[totalObjectsIndex2].transform.position = new Vector3(randomX, 0, _maxZ);
                enemiesOrTerrain[totalObjectsIndex2].SetActive(true);
                yield return new WaitForSeconds(_spawnInterval);


                


            }

            else if (enemiesOrTerrain[totalObjectsIndex2].activeSelf == false && enemiesOrTerrain[totalObjectsIndex2].tag == "Terrain")
            {

                float[] xPositionsTerrain = { _minX , 0, _maxX  };
                int index = UnityEngine.Random.Range(0, xPositionsTerrain.Length);
                enemiesOrTerrain[totalObjectsIndex2].transform.position = new Vector3(xPositionsTerrain[index], 0, _maxZ);
                enemiesOrTerrain[totalObjectsIndex2].SetActive(true);
                yield return new WaitForSeconds(_spawnInterval);


                

            }



        }
    }


    // Update is called once per frame
    void Update()
    {
        if (_player.GetComponent<Player>().PlayerHealth <= 0)
        {
            StopCoroutine(SpawnObjectRoutine());
        }

    }


}
