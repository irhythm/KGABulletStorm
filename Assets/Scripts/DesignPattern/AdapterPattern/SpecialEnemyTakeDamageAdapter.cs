using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEnemyTakeDamageAdapter : MonoBehaviour, ITakeDamageAdapter
{

    [SerializeField] private int _weakness = 2;

    private SpecialEnemy _specialEnemy;
    // Start is called before the first frame update
    void Start()
    {
        _specialEnemy = GetComponent<SpecialEnemy>();

        if (_specialEnemy == null)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnTakeDamage(int damage)
    {
        _specialEnemy.TakeDamage(damage * _weakness);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
