using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputComponent : MonoBehaviour
{
    private float _horInput;
    private float _verInput;

    public float HorInput => _horInput; //return _horInput;�� ����
    public float VerInput => _verInput;

    // Update is called once per frame
    void Update()
    {
        _horInput = Input.GetAxisRaw("Horizontal"); //���� //GetAxis �� �ն��� 0���� õõ�� ���ư��⶧���� GetAxisRaw����� �ٷ� ��ǲ�� 0�̵�

        _verInput = Input.GetAxisRaw("Vertical"); //����

        Debug.Log($"HorInput : {_horInput}, VerInput : {_verInput}");



    }
}
