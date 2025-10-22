using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject : MonoBehaviour
{
    private List<IObserver> observers = new List<IObserver>();
    // Start is called before the first frame update



    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }


    private void Notify()
    {
        foreach(IObserver observer in observers)
        {
            observer.OnNotify();
        }
    }

    void Start()
    {
        Notify();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
