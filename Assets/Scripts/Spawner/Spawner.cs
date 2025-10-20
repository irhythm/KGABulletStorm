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
    
    public List<GameObject> ObjectToSpawn { get { return _objectToSpawn; } }

    private float _minX;
    private float _maxX;
    private float _maxZ;
    private List<GameObject> enemiesOrTerrain = new List<GameObject>();
    

    private Coroutine spawnCoroutine;
    void Start()
    {
        if (_objectToSpawn.Count >0)
        {
            if (_objectToSpawn[0].tag == "Enemy")
            {
                enemiesOrTerrain = ObjectManager.Instance.EnemyPrefabs;
            }

            else if (_objectToSpawn[0].tag == "Terrain")
            {
                enemiesOrTerrain = ObjectManager.Instance.TerrainPrefabs;
            }

        }

        

        if (_spawnRangePlane != null && _objectToSpawn != null)
        {

            
            Bounds _xBounds = _spawnRangePlane.GetComponent<MeshRenderer>().bounds;
            _minX = _xBounds.center.x - _xBounds.extents.x;
            _maxX = _xBounds.center.x + _xBounds.extents.x;
            _maxZ = _xBounds.center.z + _xBounds.extents.z;
            //Debug.Log("Is it fine until here?");
            //StartCoroutine(SpawnObjectRoutine());
        }
        else
        {
            //Debug.LogError("Spawner: Spawn Range Plane or Object to Spawn is not assigned.");
        }



    }


    private IEnumerator SpawnObjectRoutine()
    {
        while (true)
        {

            


            int totalObjectsIndex2 = UnityEngine.Random.Range(0, enemiesOrTerrain.Count);


            if (enemiesOrTerrain[totalObjectsIndex2].activeSelf == false && enemiesOrTerrain[totalObjectsIndex2].tag == "Enemy" && GameManager.Instance.IsPlaying)
            {




                float randomX = UnityEngine.Random.Range(_minX, _maxX);
                enemiesOrTerrain[totalObjectsIndex2].transform.position = new Vector3(randomX, 0, _maxZ);
                enemiesOrTerrain[totalObjectsIndex2].SetActive(true);
                yield return new WaitForSeconds(_spawnInterval);





            }

            else if (enemiesOrTerrain[totalObjectsIndex2].activeSelf == false && enemiesOrTerrain[totalObjectsIndex2].tag == "Terrain" && GameManager.Instance.IsPlaying)
            {

                float[] xPositionsTerrain = { _minX, 0, _maxX };
                int index = UnityEngine.Random.Range(0, xPositionsTerrain.Length);
                enemiesOrTerrain[totalObjectsIndex2].transform.position = new Vector3(xPositionsTerrain[index], 0, _maxZ);
                enemiesOrTerrain[totalObjectsIndex2].SetActive(true);
                yield return new WaitForSeconds(_spawnInterval);




            }
            else
                {
                yield return null;
            }

        }




    }



    



    //Update is called once per frame
    void Update()
    {
        //Debug.Log("Spawner Update Check");
        if (GameManager.Instance.IsPlaying)
        {
            // 1. If game IS playing, but the coroutine is NOT running, start it.
            if (spawnCoroutine == null)
            {
                spawnCoroutine = StartCoroutine(SpawnObjectRoutine());
            }
            // If it's running, do nothing and let it continue.
        }
        else // Game is NOT playing
        {
            // 2. If game is NOT playing, AND the coroutine IS running, stop it and clear the reference.
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine); //  CRITICAL: Stop the stored reference!
                spawnCoroutine = null;         // Clear the reference so it can't be stopped again or confused.
            }
            // If it's already null, do nothing.
        }

    }


}
