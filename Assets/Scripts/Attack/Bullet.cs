using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 20f;
    [SerializeField] private float _lifeTime = 3f;


    private float _lifeTimer = 0f;



    

    // Update is called once per frame
    void Update()
    {
        _lifeTimer += Time.deltaTime;
        CheckSelfDestroy();
        Move();
    }


    void Move()
    {
        Vector3 deltaMovement = _moveSpeed * Time.deltaTime * Vector3.forward;
        transform.Translate(deltaMovement);
    }

    void CheckSelfDestroy()
    {
        if (_lifeTimer >= _lifeTime)
        {
            Destroy(gameObject);
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Enemy")
        {
            other.GetComponent<Enemy>().TakeDamage(1);


        }


        else if (other.tag == "Terrain")
        {
            other.GetComponent<Terrain>().TakeDamage(1);


        }
    }

    





}
