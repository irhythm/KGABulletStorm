using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIPlayerHpTopBar : MonoBehaviour , IPlayerObserver
{
    [SerializeField] private Image _hpBarFillImage;
    [SerializeField] private Player player;

    private void Awake()
    {
        if (player != null)
        {
            player.AddObserver(this);
        }
    }
    public void OnDestroy()
    {
        if (player != null)
        {
            player.RemoveObserver(this);
        }
    }

    public void OnPlayerHpChanged(float curHp, float maxHp)
    {
        _hpBarFillImage.fillAmount = curHp / maxHp;

    }

    public void OnPlayerStateChange()
    {
        //throw new System.NotImplementedException();
    }
}
