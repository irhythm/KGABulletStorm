using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
public class Player : MonoBehaviour
{

    [SerializeField] private Transform _trfStartPos;
    [SerializeField] private int _playerHealth = 10;
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private GameObject _petPrefab;
    [SerializeField] private float _petSpawnYOffset = 10f;


    [SerializeField] private bool _isPetSpawned = false;

    private List<IPlayerObserver> _playerObservers = new List<IPlayerObserver>();

    public bool IsPetSpawned { get { return _isPetSpawned; } }

    private Vector3 _currentPosition ;
    public void TogglePetSpawn()
    {
        _isPetSpawned = !_isPetSpawned;
    }

    public void AddObserver(IPlayerObserver observer)
    {
        _playerObservers.Add(observer);
    }
    public void RemoveObserver(IPlayerObserver observer)
    {
        _playerObservers.Remove(observer);
    }

    public int PlayerHealth { get { return _playerHealth; } }
    private int _currentHealth;
    //private InputComponent _inputComponent;

    private void NotifyHpChange()
    {
        foreach (IPlayerObserver observer in _playerObservers)
        {
            observer.OnPlayerHpChanged(_currentHealth, _playerHealth);
        }
    }

    private void NotifyStateChange()
    {
        foreach (IPlayerObserver observer in _playerObservers)
        {
            observer.OnPlayerStateChange();
        }
    }

    

    private void NotifyPositionChange()
    {
        foreach (IPlayerObserver observer in _playerObservers)
        {
            observer.OnPlayerPositionChanged();
        }
    }

    private void RegistPlayer()
    {
        GameManager.Instance.OnGameStartAction += InitPlayer;
        //GameManager.Instance.OnGameOverEvent.AddListener(() => { gameObject.GetComponent<SphereCollider>().enabled = true; });
    }



    public void OnTakeDamage(int damage)
    {
        //Debug.Log("Player OnTakeDamage called with damage: " + damage);
        _currentHealth -= damage;
        NotifyHpChange();

        if (_currentHealth <= 0)
        {
            NotifyStateChange();
            _currentHealth = 0;
            //Debug.Log("Player is Dead!");
            // Implement player death logic here
            GameManager.Instance.ChangeGameState();
            //gameObject.SetActive(false);
        }

        // Implement damage logic here
        //Debug.Log("Player took " + damage + " damage.");
    }


    // Start is called before the first frame update

    private void InitPlayer()
    {
        _currentHealth = _playerHealth;
        transform.position = _trfStartPos.position;
        gameObject.SetActive(true);
        NotifyHpChange();
        NotifyStateChange();

    }


    private void Fire()
    {
        if (_weapons == null)
        {
            return;
        }
        //if (_fireCoolTime >0)
        //{
        //    _fireCoolTime -= Time.deltaTime;
        //}

        foreach (Weapon weapon in _weapons)
        {
            weapon.FireBullet();
        }


    }

    void Start()
    {
        //GameManager gm = GameManager.Instance;


        InitPlayer();
        StartCoroutine(PetSpawnCondition());
        NotifyStateChange(); //처음에는 죽어있으니까
        RegistPlayer();
        _currentPosition = transform.position;

        if (GameManager.Instance != null)
        {
            //GameManager.Instance.OnGameOverEvent.AddListener(() => { gameObject.GetComponent<SphereCollider>().enabled = false; });
            //GameManager.Instance.OnGameStartAction += () => gameObject.GetComponent<SphereCollider>().enabled = true;

        }
        //Debug.Log(GameManager.Instance.IsPlaying);

    }

    //private void SetComponent()
    //{
    //    _inputComponent = GetComponent<InputComponent>();
    //    _inputComponent.OnClickFireAction += Fire;
    //}

    private void OnDestroy()
    {


        //GameManager.Instance.OnGameStartAction -= InitPlayer;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStartAction -= InitPlayer;

        }

        //_inputComponent.OnClickFireAction -= Fire;
    }

    //Awake is called before Start
    //Awake is different from onenable
    //void OnEnable()
    //{
    //    InitPlayer();
    //}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Fire();
        }



        if (_currentPosition !=transform.position)
        {
            NotifyPositionChange();
        }
        _currentPosition = transform.position;
    }


    IEnumerator PetSpawnCondition()
    {

        while (true)
        {

            switch (_isPetSpawned)
            {
                case true:
                    if (FindObjectOfType<PetController>() == null)
                    {
                        Instantiate(_petPrefab, transform.position + Vector3.up * _petSpawnYOffset, _petPrefab.transform.rotation);
                        
                    }
                    yield return null;
                    break;
                case false:
                    if (FindObjectOfType<PetController>() != null)
                    {
                        Destroy(FindObjectOfType<PetController>());
                    }
                    yield return null;
                    break;
            }

        }
    }
}



