using UnityEngine;

/// <summary>
/// �C�x���g���ʗp�̒��ۃN���X
/// </summary>
public abstract class EventBase : MonoBehaviour
{
    public EventType eventType;

    /// <summary>
    /// �C�x���g���Ηp
    /// </summary>
    public abstract void TriggerEvent(int value, BodyRegionType hitBodyRegionType = BodyRegionType.Not_Available);

    /// <summary>
    /// �C�x���g�̏����ݒ�p
    /// </summary>
    /// <param name="playerController"></param>
    /// <param name="gameManager"></param>
    public abstract void SetUpEvent(PlayerController playerController, GameManager gameManager);
}
