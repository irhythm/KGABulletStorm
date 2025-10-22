using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private bool _isPlaying = false;

    public bool IsPlaying { get { return _isPlaying; } }
    public event Action OnGameStartAction;
    [field: SerializeField] public UnityEvent OnGameOverEvent { get; private set; } = new UnityEvent();

    //void Add(int a)
    //{

    //}
    //현재 게임의 진행중인 상태를 설정
    public void ChangeGameState()
    {
        //OnGameStartAction += Add(int a);
        //Button btn = new Button();

        _isPlaying = !_isPlaying;

        Debug.Log("Game State Changed. IsPlaying: " + _isPlaying);
        if (_isPlaying)
            OnGameStartAction?.Invoke();

        else
        {
            OnGameOverEvent?.Invoke();
        }
    }



}
