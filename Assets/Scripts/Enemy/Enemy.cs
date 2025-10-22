using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 200f;
    [SerializeField] private float _deadMoveSpeed = 30f;
    [SerializeField] private float _enemyHealth = 2f;
    //[SerializeField] private GameObject _rangePlane;
    [SerializeField] private float _despawnTimerInput = 3f;
    [SerializeField] private float _explodeUponDeathForce = 10f;
    [SerializeField] private float _totalLifetime = 20f;
    //[SerializeField] GameObject _enemyType;
    [SerializeField] private float _knockbackForce = 5f;
    [SerializeField] private float _knockbackDuration = 0.5f;


    private float _currentHealth;
    //private float _spawnTimer = 0;
    private Vector3 _minWorldBounds;
    private Vector3 _maxWorldBounds;
    //private float _randomX = 0;
    private float _despawnTimer = 0f;
    private int _deathAction = 0;
    private bool _hasCollidedWithPlayer = false;
    private float _lifetimeTimer = 0f;
    private GameObject placeHolder;
    private bool _isKnockbacked = false;
    public int DeathAction { get { return _deathAction; } }
    public bool IsKnockbacked { get { return _isKnockbacked; } }

    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; }  }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().OnTakeDamage(1);

            
            _hasCollidedWithPlayer = true;
            //Debug.Log("Player Hit!");
            
        }


        else if(other.tag == "Despawner")
        {
            ResetState();
            gameObject.SetActive(false);
            //Debug.Log("Enemy Destroyed by Despawner!");
        }
    }

    //void SpawnEnemy()
    //{
    //    _spawnTimer += Time.deltaTime;
    //    if (_spawnTimer % 10 == 0)
    //    {
    //        _spawnTimer = 0;
    //        Instantiate(gameObject, new Vector3(_randomX, 0, _maxWorldBounds.z), transform.rotation);
    //    }
    //}

    //void Start()
    //{
        
    //    if (_rangePlane != null)
    //    {
    //        Bounds _planeBounds = _rangePlane.GetComponent<MeshRenderer>().bounds;
    //        _minWorldBounds = _planeBounds.center - _planeBounds.extents;
    //        _maxWorldBounds = _planeBounds.center + _planeBounds.extents;
    //        //_randomX = Random.Range(_minWorldBounds.x, _maxWorldBounds.x);
            
    //    }
    //}

    void Start()
    {
        //scene에서 활성화 되면 한번 호출
        //Debug.Log("Enemy Start Test");
        //placeHolder = gameObject;
    }

    void Awake()
    {
        //scene에서 활성화 되면 한번 호출
        //Debug.Log("Enemy Awake Test");
    }

    void OnEnable()
    {
        ResetState();
        _currentHealth = _enemyHealth;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.back * _moveSpeed;
    }



    // Update is called once per frame
    void Update()
    {
        CheckHealth();
        
        //SpawnEnemy();
    }
    public void ResetState()
    {
        //_currentHealth = _enemyHealth;
        _despawnTimer = 0f;
        _deathAction = 0;
        _lifetimeTimer = 0f;
        _hasCollidedWithPlayer = false;
        _isKnockbacked = false;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    void CheckHealth()
    {
        _lifetimeTimer += Time.deltaTime;
        if (_currentHealth <= 0 || _hasCollidedWithPlayer || _lifetimeTimer >=_totalLifetime)
        {
            if (_deathAction == 0)
            {
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.back * _deadMoveSpeed;
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-10, 10), Random.Range(5, 15), Random.Range(-10, 10)) * _explodeUponDeathForce, ForceMode.Impulse);
                gameObject.GetComponent<BoxCollider>().enabled = false;
                _deathAction++;

            }

            if (_despawnTimer >= _despawnTimerInput)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<BoxCollider>().enabled = true;
                gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                _deathAction = 0;
                _despawnTimer = 0f;
                _lifetimeTimer = 0f;
                _hasCollidedWithPlayer = false;

                gameObject.SetActive(false);
                //Debug.Log("Enemy Destroyed!");
            }
            _despawnTimer += Time.deltaTime;



        }
        if (_deathAction == 0 && _isKnockbacked == false && CameraMovement._isBoosting == false)
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.back * _moveSpeed;
        }
    }

    public void TakeDamage(int damage)
    {

        _currentHealth -= damage;
        // Implement damage logic here
        //Debug.Log("Enemy took " + damage + " damage.");
        StartCoroutine(Knockback());
        

    }


    private IEnumerator Knockback()
    {
        _isKnockbacked = true;
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * _knockbackForce, ForceMode.Impulse);
        yield return new WaitForSeconds(_knockbackDuration);
        _isKnockbacked = false;

    }


}
