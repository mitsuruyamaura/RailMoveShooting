using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MissionBase<T> : MonoBehaviour
{
    /// <summary>
    /// �C�x���g���Ηp
    /// </summary>
    public abstract void TriggerMission(T t);
}
