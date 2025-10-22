using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BExampleForObserver : MonoBehaviour, IObserver
{
    [SerializeField] private Subject _subject;

    private void Awake()
    {
        _subject?.AddObserver(this);
    }

    private void OnDestroy()
    {
        _subject?.RemoveObserver(this);
    }



    public void OnNotify()
    {
        Debug.Log("BExampleForObserver has been notified.");
    }

}




