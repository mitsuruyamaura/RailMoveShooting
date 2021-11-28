using UnityEngine;

public class EventGenerator : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController playerController;

    /// <summary>
    /// �ݒ�
    /// </summary>
    /// <param name="gameManager"></param>
    /// <param name="playerController"></param>
    public void SetUpEventGenerator(GameManager gameManager, PlayerController playerController) {
        this.gameManager = gameManager;
        this.playerController = playerController;
    }

    /// <summary>
    /// �~�b�V�������̊e�C�x���g�̐���
    /// </summary>
    /// <param name="events"></param>
    /// <param name="eventTrans"></param>
    /// <returns></returns>
    public void PrepareGenerateEvents((EventType[] eventTypes, int[] eventNos) events, Transform[] eventTrans) {

        Debug.Log("�C�x���g�����@�J�n");

        // �C�x���g�f�[�^���擾���邽�߂̔z�������
        EventDataSO.EventData[] eventDatas = new EventDataSO.EventData[events.eventTypes.Length];

        Debug.Log(events.eventTypes.Length);
        Debug.Log(events.eventTypes[0]);


        // �C�x���g�̎�ނƔԍ��ɉ����ăX�N���v�^�u���E�I�u�W�F�N�g����C�x���g�f�[�^������
        for (int i = 0; i < events.eventTypes.Length; i++) {
            Debug.Log(events.eventNos[0]);
            eventDatas[i] = DataBaseManager.instance.GetEventDataFromEventType(events.eventTypes[i], events.eventNos[i]);
        }

        Debug.Log(eventDatas[0].eventPrefab.name);

        // �x�[�X�N���X�𗘗p���Ĉꌳ������
        // �C�x���g�f�[�^�Ɏw�肳��Ă����ނ̃C�x���g����
        for (int i = 0; i < eventDatas.Length; i++) {

            GenerateEvent(eventDatas[i], eventTrans[i]);

            //switch (events.eventTypes[i]) {
            //    case EventType.Enemy:
            //        GenerateEnemy(eventDatas[i], eventTrans[i]);
            //        continue;

            //    case EventType.Gimmick:
            //        GenerateGimmick(eventDatas[i], eventTrans[i]);
            //        continue;

            //    case EventType.Item:
            //        GenerateItem(eventDatas[i], eventTrans[i]);
            //        continue;

            //    case EventType.Boss:
            //        GenerateEnemy(eventDatas[i], eventTrans[i]);
            //        continue;
            //}
        }

        Debug.Log("�C�x���g�����@����");
    }

    /// <summary>
    /// �C�x���g�����B�e�C�x���g���ƂɐU�镑����ς���
    /// </summary>
    /// <param name="eventData"></param>
    /// <param name="eventTran"></param>
    private void GenerateEvent(EventDataSO.EventData eventData, Transform eventTran) {

        Debug.Log(eventData.eventPrefab);
        Debug.Log(eventTran);

        EventBase eventBase = Instantiate(eventData.eventPrefab, eventTran.position, eventData.eventPrefab.transform.rotation).GetComponent<EventBase>();
        eventBase.SetUpEvent(playerController, gameManager);

        gameManager.AddEventList(eventBase);
    }


    //�@���ނ͂�����

    /// <summary>
    /// �~�b�V�������̊e�C�x���g�̐��������̃I�[�o�[���[�h
    /// </summary>
    /// <param name="eventDatas"></param>
    /// <param name="eventTrans"></param>
    public void PrepareGenerateEvents(EnemyController[] eventDatas, Transform[] eventTrans) {

        for (int i = 0; i < eventDatas.Length; i++) {

            GenerateEvent(eventDatas[i], eventTrans[i]);
        }
     }

    /// <summary>
    /// �C�x���g�����B�e�C�x���g���ƂɐU�镑����ς��鏈���̃I�[�o�[���[�h
    /// �X�N���v�^�u���E�I�u�W�F�N�g�𗘗p�����AMissionEventTrigger �ɒ��� EventBase �̃v���t�@�u���A�T�C�����ė��p����ꍇ
    /// </summary>
    /// <param name="eventPrefab"></param>
    /// <param name="eventTran"></param>
    private void GenerateEvent(EnemyController eventPrefab, Transform eventTran) {
        EnemyController enemy = Instantiate(eventPrefab, eventTran.position, eventPrefab.transform.rotation);
        enemy.SetUpEnemy(playerController, gameManager);

        gameManager.AddEnemyList(enemy);
    }


/**** ���g�p ****/

    ///// <summary>
    ///// �G�̐���
    ///// </summary>
    ///// <param name="eventData"></param>
    ///// <param name="eventTran"></param>
    //public void GenerateEnemy(EventDataSO.EventData eventData, Transform eventTran) {
    //    EnemyBase enemy = Instantiate(eventData.eventPrefab, eventTran).GetComponent<EnemyBase>();
    //    enemy.SetUpEnemy(playerController.gameObject, gameManager);
    //    //gameManager.AddEnemyList(enemy);
    //}

    ///// <summary>
    ///// �M�~�b�N�̐���
    ///// </summary>
    ///// <param name="eventData"></param>
    ///// <param name="eventTran"></param>
    //public void GenerateGimmick(EventDataSO.EventData eventData, Transform eventTran) {
    //    GameObject gimmick = Instantiate(eventData.eventPrefab, eventTran);
    //    gameManager.AddGimmickList(gimmick);
    //}

    ///// <summary>
    ///// �A�C�e���̐���
    ///// </summary>
    ///// <param name="eventData"></param>
    ///// <param name="eventTran"></param>
    //public void GenerateItem(EventDataSO.EventData eventData, Transform eventTran) {
    //    GameObject item = Instantiate(eventData.eventPrefab, eventTran);
    //    item.GetComponent<ItemController>().SetUpItem(playerController);

    //    // TODO List ��������炱���ŏ�����ǉ�����

    //}
}
