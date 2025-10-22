using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AExampleForObserver : MonoBehaviour, IObserver

{
    [SerializeField] private Subject _subject;

    public void OnNotify()
    {
        Debug.Log("AExampleForObserver has been notified.");
    }

    private void Awake()
    {
        _subject?.AddObserver(this);
    }

    private void OnDestroy()
    {
        _subject?.RemoveObserver(this);
    }






    

}




