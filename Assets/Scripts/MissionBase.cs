using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MissionBase<T> : MonoBehaviour
{
    /// <summary>
    /// イベント発火用
    /// </summary>
    public abstract void TriggerMission(T t);
}
