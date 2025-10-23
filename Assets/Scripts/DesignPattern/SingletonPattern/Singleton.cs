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
                _instance = FindObjectOfType<T>(); //������ �ش� Ÿ���� �̱����� ã��
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


    // Singleton<T> Ŭ���� ���ο� �߰�

    //private static bool applicationIsQuitting = false;

    // ���ø����̼��� ����� �� �÷��׸� true�� �����մϴ�.
    //protected virtual void OnApplicationQuit()
    //{
    //    applicationIsQuitting = true;
    //}

    //protected virtual void OnDestroy()
    //{
        // ������Ʈ�� �ı��� ��, ���ø����̼� ���� ������ �ƴ϶��
        // _instance�� ����ְų� �ʿ��� ó���� �մϴ�.
        // DontDestroyOnLoad ������ �� ���� �� OnDestroy�� ȣ����� ���� �� ������,
        // �� �÷��װ� ������ ����Ƽ�� '�������� ���� ������Ʈ'�� �Ǵ��ϴ� ���� ���ϴ� �� ������ �˴ϴ�.
    //}

}

