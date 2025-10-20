using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private Transform _trfStartPos;
    [SerializeField] private int _playerHealth = 10;
    [SerializeField] private List<Weapon> _weapons;

    public int PlayerHealth { get { return _playerHealth; } }
    private int _currentHealth;
    //private InputComponent _inputComponent;


    private void RegistPlayer()
    {
        GameManager.Instance.OnGameStartAction += InitPlayer;
    }


    public void OnTakeDamage(int damage)
    {
        Debug.Log("Player OnTakeDamage called with damage: " + damage);
        _currentHealth -= damage;
        
        if (_currentHealth <= 0)
        {
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
        RegistPlayer();

    }

    //private void SetComponent()
    //{
    //    _inputComponent = GetComponent<InputComponent>();
    //    _inputComponent.OnClickFireAction += Fire;
    //}

    private void OnDestroy()
    {
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

    }
}
