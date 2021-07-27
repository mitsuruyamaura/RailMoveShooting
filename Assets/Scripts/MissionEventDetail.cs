using UnityEngine;

public class MissionEventDetail : MonoBehaviour {

    [Header("�~�b�V�����̃N���A����")]
    public ClearConditionsType clearConditionsType;

    [Header("�~�b�V�����N���A�̂��߂̓G�̎c��/�c�莞��")]
    public int missionDuration;

    [SerializeField, Header("��������C�x���g�̎��")]
    private EventType[] eventTypes;

    [SerializeField, Header("��������C�x���g�̔ԍ�")]
    private int[] eventNos;

    [SerializeField, Tooltip("�C�x���g�̐����n�_")]
    private Transform[] eventTrans;

    private BoxCollider boxCollider;
    private GameManager gameManager;

    /// <summary>
    /// EventTriggerPoint �̏���
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpMissionTriggerPoint(GameManager gameManager) {
        this.gameManager = gameManager;

        TryGetComponent(out boxCollider);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Debug.Log("�ʉ�");

            // �~�b�V���������̏d������h�~
            boxCollider.enabled = false;
            
            // �~�b�V�����J�n�̏���
            gameManager.PreparateMission(missionDuration, clearConditionsType, (eventTypes, eventNos), eventTrans);
        }
    }
}