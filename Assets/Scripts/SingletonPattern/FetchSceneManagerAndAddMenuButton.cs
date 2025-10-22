using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   
public class FetchSceneManagerAndAddMenuButton : MonoBehaviour
{
    [SerializeField] private Button _buttonSelf;
    //[SerializeField] private string sceneName;
    [SerializeField] private int sceneIndex = 0;

    //private enum SceneType
    //{
    //    TitleScreen,
    //    GameplayScene,
    //    SceneManagerDude
    //}


    // Start is called before the first frame update
    void Start()
    {
        

        _buttonSelf.onClick.AddListener(() => { SceneManagerDude.Instance.LoadScene(sceneIndex); });
        


    }


    private void OnDestroy()
    {
        //_buttonSelf.onClick.RemoveListener(() => { SceneManagerDude.Instance.LoadScene(sceneIndex); });
    }
}



