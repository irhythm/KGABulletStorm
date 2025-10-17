using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputComponent : MonoBehaviour
{
    private float _horInput;
    private float _verInput;
    private bool _leftClickInput;
    private bool _rightClickInput;

    public float HorInput => _horInput; //return _horInput;�� ����
    public float VerInput => _verInput;
    public bool LeftClickInput => _leftClickInput;
    public bool RightClickInput => _rightClickInput;


    // Update is called once per frame
    void Update()
    {
        _horInput = Input.GetAxisRaw("Horizontal"); //���� //GetAxis �� �ն��� 0���� õõ�� ���ư��⶧���� GetAxisRaw����� �ٷ� ��ǲ�� 0�̵�

        _verInput = Input.GetAxisRaw("Vertical"); //����

        _leftClickInput = Input.GetKeyDown(KeyCode.Mouse0);
        //Input.GetAxisRaw("Fire1");
        _rightClickInput =  Input.GetKeyDown(KeyCode.Mouse1);
        //Input.GetAxisRaw("Fire2");

        //Debug.Log($"HorInput : {_horInput}, VerInput : {_verInput} LeftClick : {_leftClickInput}, RightClick : {_rightClickInput}");


    }
}
