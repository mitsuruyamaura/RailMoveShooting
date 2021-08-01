using UnityEngine;

/// <summary>
/// �C�x���g���ʗp�̒��ۃN���X
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class EventBase : MonoBehaviour
{
    public EventType eventType;

    /// <summary>
    /// �C�x���g���Ηp
    /// </summary>
    public abstract void TriggerEvent(int value, BodyRegionType hitBodyRegionType);
}
