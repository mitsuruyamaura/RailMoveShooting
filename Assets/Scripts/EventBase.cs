using UnityEngine;

/// <summary>
/// イベント共通用の抽象クラス
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class EventBase : MonoBehaviour
{
    public EventType eventType;

    /// <summary>
    /// イベント発火用
    /// </summary>
    public abstract void TriggerEvent(int value, BodyRegionType hitBodyRegionType);
}
