using UnityEngine;

[System.Serializable]
public class MissionEventDetail : MonoBehaviour {

    [Header("�~�b�V�����̃N���A����")]
    public ClearConditionsType clearConditionsType;

    [Header("�~�b�V�����N���A�̂��߂̓G�̎c��/�c�莞��")]
    public int missionDuration;

    [Header("��������C�x���g�̎��")]
    public EventType[] eventTypes;

    [Header("��������C�x���g�̔ԍ�")]
    public int[] eventNos;

    [Tooltip("�C�x���g�̐����n�_")]
    public Transform[] eventTrans;

    [Header("�C�x���g�̃v���t�@�u")]
    public EventBase[] eventPrefabs;

    //private BoxCollider boxCollider;
    //private GameManager gameManager;

    ///// <summary>
    ///// EventTriggerPoint �̏���
    ///// </summary>
    ///// <param name="gameManager"></param>
    //public void SetUpMissionTriggerPoint(GameManager gameManager) {
    //    this.gameManager = gameManager;

    //    TryGetComponent(out boxCollider);
    //}

    //private void OnTriggerEnter(Collider other) {
    //    if (other.tag == "Player") {
    //        Debug.Log("�ʉ�");

    //        // �~�b�V���������̏d������h�~
    //        boxCollider.enabled = false;

    //        // �~�b�V�����J�n�̏���
    //        gameManager.PreparateMission(missionDuration, clearConditionsType, (eventTypes, eventNos), eventTrans);
    //    }
    //}
}