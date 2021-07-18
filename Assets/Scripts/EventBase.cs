using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventBase<T> : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public abstract void TriggerEvent(T t);
}
