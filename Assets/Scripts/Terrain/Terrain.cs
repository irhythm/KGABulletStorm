using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : Enemy
{
    

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Despawner")
        {
            gameObject.SetActive(false);
            Debug.Log("Terrain Destroyed by Despawner!");
        }
    }


    

}
