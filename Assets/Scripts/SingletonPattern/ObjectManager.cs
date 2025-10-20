using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    [SerializeField] private int _maxEnemySpawnCount = 10;
    [SerializeField] private int _maxBulletSpawnCount = 20;
    [SerializeField] private int _maxTerrainSpawnCount = 5;
    [SerializeField] private List<GameObject> _spawners;


    private List<GameObject> _objectPrefabs = new List<GameObject>();

    private List<GameObject> _enemyPrefabs;
    private List<GameObject> _terrainPrefabs;
    private List<GameObject> _bulletPrefabs;
    public List<GameObject> EnemyPrefabs { get { return _enemyPrefabs; } }
    public List<GameObject> TerrainPrefabs { get { return _terrainPrefabs; } }
    public List<GameObject> BulletPrefabs { get { return _bulletPrefabs; } }
    private int _totalObjectsCount;
    private int _currentEnemyTypeCount = 0;
    private int _currentBulletTypeCount = 0;
    private int _currentTerrainTypeCount = 0;
    //private int _tempEnemyTypeCount = 0;
    //private int _tempBulletTypeCount = 0;
    //private int _tempTerrainTypeCount = 0;




    void Init()
    {
        foreach (var spawner in _spawners)
        {
            
            Spawner spawnerComponent = spawner.GetComponent<Spawner>();

            
            if (spawnerComponent != null && spawnerComponent.ObjectToSpawn != null)
            {
                foreach (GameObject obj in spawnerComponent.ObjectToSpawn)
                {
                    _objectPrefabs.Add(obj);
                }
            }

            
            Weapon weaponComponent = spawner.GetComponent<Weapon>();

            
            if (weaponComponent != null && weaponComponent.BulletPrefab != null)
            {
                
                _objectPrefabs.Add(weaponComponent.BulletPrefab);
            }
        }




        _totalObjectsCount = _objectPrefabs.Count;

        foreach (var prefab in _objectPrefabs)
        {

            if (prefab.CompareTag("Enemy"))
            {
                _currentEnemyTypeCount++;
                if (_enemyPrefabs == null)
                {
                    _enemyPrefabs = new List<GameObject>();
                }
                for (int i = 0; i < _maxEnemySpawnCount; i++)
                {

                    GameObject temp = Instantiate(prefab); //기본 위치 및 회전값
                    temp.SetActive(false);
                    _enemyPrefabs.Add(temp);

                }
            }
            else if (prefab.CompareTag("Terrain"))
            {
                _currentTerrainTypeCount++;
                if (_terrainPrefabs == null)
                {
                    _terrainPrefabs = new List<GameObject>();
                }
                for (int i = 0; i < _maxTerrainSpawnCount; i++)
                {

                    GameObject temp = Instantiate(prefab); 
                    temp.SetActive(false);

                    _terrainPrefabs.Add(temp);

                }
            }
            else if (prefab.CompareTag("Bullet"))
            {
                _currentBulletTypeCount++;
                if (_bulletPrefabs == null)
                {
                    _bulletPrefabs = new List<GameObject>();
                }
                for (int i = 0; i < _maxBulletSpawnCount; i++)
                {

                    GameObject temp = Instantiate(prefab); 
                    temp.SetActive(false);

                    _bulletPrefabs.Add(temp);

                }
            }
        }
    }
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        Init();




    }

   
}
