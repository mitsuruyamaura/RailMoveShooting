using UnityEngine;

/// <summary>
/// イベント共通用の抽象クラス
/// </summary>
public abstract class EventBase : MonoBehaviour
{
    public EventType eventType;

    /// <summary>
    /// イベント発火用
    /// </summary>
    public abstract void TriggerEvent(int value, BodyRegionType hitBodyRegionType = BodyRegionType.Not_Available);

    /// <summary>
    /// イベントの初期設定用
    /// </summary>
    /// <param name="playerController"></param>
    /// <param name="gameManager"></param>
    public abstract void SetUpEvent(PlayerController playerController, GameManager gameManager);
}
