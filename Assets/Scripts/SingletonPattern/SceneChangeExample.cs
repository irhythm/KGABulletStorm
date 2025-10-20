using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) //키보드 1번 누르면
        {
            SceneManager.LoadScene(0); //순서 0번 씬으로 이동
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) //키보드 2번 누르면
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) //키보드 3번 누르면
        {
            SceneManager.LoadScene(2);
        }


    }
}
