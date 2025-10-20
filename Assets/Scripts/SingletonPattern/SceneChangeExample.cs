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
        if (Input.GetKeyDown(KeyCode.Alpha1)) //Ű���� 1�� ������
        {
            SceneManager.LoadScene(0); //���� 0�� ������ �̵�
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) //Ű���� 2�� ������
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) //Ű���� 3�� ������
        {
            SceneManager.LoadScene(2);
        }


    }
}
