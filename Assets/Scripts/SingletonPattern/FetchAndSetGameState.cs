using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FetchAndSetGameState : MonoBehaviour
{

        void _onButtonClickAction()  { gameObject.SetActive(false); }
        void _onGameOverAction() { gameObject.SetActive(true); }
        void _onGameStartAction() { gameObject.SetActive(false); }

    
    //[SerializeField] Button _buttonSelf;
    private Button _buttonSelf;
    // Start is called before the first frame update
    void Start()
    {


        _buttonSelf = gameObject.GetComponent<Button>();

        if (gameObject.name != "Menu")
        {
        _buttonSelf.onClick.AddListener(GameManager.Instance.ChangeGameState);
            
        }
        
        GameManager.Instance.OnGameOverEvent.AddListener( _onGameOverAction );
        
        _buttonSelf.onClick.AddListener( _onButtonClickAction  );

        //GameManager.Instance.

        if (gameObject.name == "Menu")
        {
            gameObject.SetActive(false);
            GameManager.Instance.OnGameStartAction+=( _onGameStartAction );

        }

        
    }


    private void OnDestroy()
    {
        // Safely check for the Singleton before accessing it
        if (GameManager.Instance != null)
        {
            // 3. Use the defined actions for unsubscribing (THIS IS KEY)
            GameManager.Instance.OnGameOverEvent.RemoveListener(_onGameOverAction);

            if (gameObject.name == "Menu")
            {
                GameManager.Instance.OnGameStartAction -= _onGameStartAction;
            }
        }

        // Unsubscribe from the Button's local event
        //if (_buttonSelf != null)
        //{
        //    _buttonSelf.onClick.RemoveListener(GameManager.Instance.ChangeGameState);
        //    _buttonSelf.onClick.RemoveListener(_onButtonClickAction);
        //}
    }
}


