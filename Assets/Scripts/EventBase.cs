using UnityEngine;

public abstract class EventBase<T> : MonoBehaviour
{
    /// <summary>
    /// �C�x���g���Ηp
    /// </summary>
    public abstract void TriggerEvent(T t);
}
