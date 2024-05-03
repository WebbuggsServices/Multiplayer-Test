using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector3> OnMove;
    public event Action OnStart;
    public event Action OnInteract;

    private void Update()
    {
        UpdateMove();
        UpdateInteractions();
    }

    private void UpdateMove()
    {
        var MoveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        OnMove?.Invoke(MoveVector);
    }

    private void UpdateInteractions()
    {
        if (Input.GetKeyDown(KeyCode.E)) OnInteract?.Invoke();

        if(Input.GetKeyDown(KeyCode.Return)) OnStart?.Invoke();
    }
}
