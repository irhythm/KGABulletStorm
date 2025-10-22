using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchSingleton : MonoBehaviour
{


    //[SerializeField] privat

    [SerializeField] private SingletonType singletonType;
    private void Awake()
    {
        switch (singletonType)
        {
            case SingletonType.GameManager:
                GameManager gm = GameManager.Instance;
                Debug.Log("Fetched GameManager singleton: " + gm);
                break;
            case SingletonType.ObjectManager:
                ObjectManager om = ObjectManager.Instance;
                Debug.Log("Fetched ObjectManager singleton: " + om);
                break;
            case SingletonType.SceneManagerDude:
                SceneManagerDude smd = SceneManagerDude.Instance; 
                Debug.Log("Fetched SceneManagerDude singleton: " + smd);
                break;
            default:
                Debug.LogWarning("Unknown singleton type selected.");
                break;
        }
    }

    private enum SingletonType
    {
        GameManager,
        ObjectManager,
        SceneManagerDude
    }





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
