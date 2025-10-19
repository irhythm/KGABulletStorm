using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _playerHealth = 10;
    [SerializeField] private List<Weapon> _weapons;

    public int PlayerHealth { get { return _playerHealth; } }
    private int _currentHealth;
    public void OnTakeDamage(int damage)
    {
        _currentHealth -= damage;
        
        if (_currentHealth <= 0)
        {
            Debug.Log("Player is Dead!");
            // Implement player death logic here
            gameObject.SetActive(false);
        }

        // Implement damage logic here
        //Debug.Log("Player took " + damage + " damage.");
    }


    // Start is called before the first frame update

    private void InitPlayer()
    {
        _currentHealth = _playerHealth;
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


    //Awake is called before Start
    void Awake()
    {
        InitPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Fire();
        }

    }
}
