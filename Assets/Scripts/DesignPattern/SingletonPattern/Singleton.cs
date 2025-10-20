using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�ܺ� ȣ��� ������Ƽ, �ش� ��Ÿ���� �̱����� ������ ã�ƺ��� �׷��� ������ ���� ���� �� ����

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }


    //�ߺ� üũ �� ���� ��� ����
    protected virtual void Awake() //��ӹ޴� Ŭ�������� �������̵� ����, ������Ƽ��� �ڽĸ� ���� ����
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this) // for returning to a previous scene where the singleton already exists

            {
                Destroy(gameObject);
            }

        }
    }
}

