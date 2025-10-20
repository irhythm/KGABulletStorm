using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    //유니티 이벤트 노출
    [field: SerializeField] public UnityEvent OnPetCalled { get; private set; } = new UnityEvent();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            CallPet();
    }

    private void CallPet()
    {
        OnPetCalled?.Invoke();
    }
}