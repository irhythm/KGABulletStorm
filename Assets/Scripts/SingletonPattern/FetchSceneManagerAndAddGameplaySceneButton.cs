using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   
public class FetchSceneManagerAndAddGamepplaySceneButton : MonoBehaviour
{
    [SerializeField] private Button _buttonSelf;
    //[SerializeField] private string sceneName;
    [SerializeField] private int _sceneIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
        

        _buttonSelf.onClick.AddListener(() => { SceneManagerDude.Instance.LoadScene(_sceneIndex); });



    }

    private void OnDestroy()
    {
        //_buttonSelf.onClick.RemoveListener(() =>{SceneManagerDude.Instance.LoadScene(_sceneIndex); });
    }
}


