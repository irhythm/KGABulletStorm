using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//외부 호출용 프로퍼티, 해다 탕타입의 싱글톤이 없으면 찾아보고 그래도 없으면 새로 생성 후 설정

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


    //중복 체크 및 연결 기능 구현
    protected virtual void Awake() //상속받는 클래스에서 오버라이드 가능, 프로젝티드로 자식만 접근 가능
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

