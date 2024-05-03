using UnityEngine;
using UnityEngine.Events;

public class SpawnObject : MonoBehaviour
{
    public UnityEvent onHoverIn;
    public UnityEvent onHoverOut;

    public bool isMine=false;

    public void OnHoverIn() => onHoverIn?.Invoke();
    public void OnHoverOut() => onHoverOut?.Invoke();
}
