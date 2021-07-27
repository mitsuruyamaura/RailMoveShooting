using UnityEngine;

public abstract class EventBase<T> : MonoBehaviour
{
    /// <summary>
    /// イベント発火用
    /// </summary>
    public abstract void TriggerEvent(T t);
}
