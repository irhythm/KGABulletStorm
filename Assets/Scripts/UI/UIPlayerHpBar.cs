using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHpBar : MonoBehaviour ,IPlayerObserver
{
    [SerializeField] private float _gap;
    [SerializeField] private Image _hpBarFillImage;
    [SerializeField] GameObject _target;
    [SerializeField] Player player;

    private Camera _camera;
    private Vector3 _gapPos;

    private void Init()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }

        _gapPos = Vector3.up * _gap;

    }

    public void OnPlayerHpChanged(float curHp, float maxHp)
    {
        _hpBarFillImage.fillAmount = curHp / maxHp;

    }
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


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void MoveToTarget()
    {

        Vector3 movePos = _target.transform.position + _gapPos;
        transform.position = _camera.WorldToScreenPoint(movePos);
        //worldtoScreenPoint ¿ùµåÁÂÇ¥¸¦ ½ºÅ©¸° ÁÂÇ¥·Î ¹Ù²ãÁÜ
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
    }

    public void OnPlayerStateChange()
    {
        //throw new System.NotImplementedException();
    }
}
