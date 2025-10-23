using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITimer : MonoBehaviour, IPlayerObserver
{
    [SerializeField] private Player _player;
    [SerializeField] private TextMeshProUGUI _timerText;

    private float _timeAlive = 0;
    private bool _isPlayerAlive = false;
    public void OnPlayerHpChanged(float curHp, float maxHp)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPlayerStateChange()
    {
        _isPlayerAlive = !_isPlayerAlive;
        if (!_isPlayerAlive)
        {
            Debug.Log($"Player survived for {_timeAlive} seconds.");
            _timeAlive = 0;
        }


    }



    // Start is called before the first frame update
    void Start()
    {
        _player.AddObserver(this);
    }

    void OnDestroy()
    {
        _player.RemoveObserver(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayerAlive)
        {
            _timeAlive += Time.deltaTime;
        }
        _timerText.text = $"Time: {_timeAlive:F2}s"; //F2 소수점 둘째자리까지

    }

    public void OnPlayerPositionChanged()
    {
        //throw new System.NotImplementedException();
    }
}
