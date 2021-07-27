using UnityEngine;

public abstract class EventBase<T> : MonoBehaviour
{
    public EventType eventType;

    /// <summary>
    /// イベント発火用
    /// </summary>
    public abstract void TriggerEvent(T t);
}
