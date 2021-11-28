using UnityEngine;

public class EventGenerator : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController playerController;

    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="gameManager"></param>
    /// <param name="playerController"></param>
    public void SetUpEventGenerator(GameManager gameManager, PlayerController playerController) {
        this.gameManager = gameManager;
        this.playerController = playerController;
    }

    /// <summary>
    /// ミッション内の各イベントの生成
    /// </summary>
    /// <param name="events"></param>
    /// <param name="eventTrans"></param>
    /// <returns></returns>
    public void PrepareGenerateEvents((EventType[] eventTypes, int[] eventNos) events, Transform[] eventTrans) {

        Debug.Log("イベント生成　開始");

        // イベントデータを取得するための配列を準備
        EventDataSO.EventData[] eventDatas = new EventDataSO.EventData[events.eventTypes.Length];

        Debug.Log(events.eventTypes.Length);
        Debug.Log(events.eventTypes[0]);


        // イベントの種類と番号に応じてスクリプタブル・オブジェクトからイベントデータを検索
        for (int i = 0; i < events.eventTypes.Length; i++) {
            Debug.Log(events.eventNos[0]);
            eventDatas[i] = DataBaseManager.instance.GetEventDataFromEventType(events.eventTypes[i], events.eventNos[i]);
        }

        Debug.Log(eventDatas[0].eventPrefab.name);

        // ベースクラスを利用して一元化する
        // イベントデータに指定されている種類のイベント生成
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

        Debug.Log("イベント生成　完了");
    }

    /// <summary>
    /// イベント生成。各イベントごとに振る舞いを変える
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


    //　教材はこっち

    /// <summary>
    /// ミッション内の各イベントの生成処理のオーバーロード
    /// </summary>
    /// <param name="eventDatas"></param>
    /// <param name="eventTrans"></param>
    public void PrepareGenerateEvents(EnemyController[] eventDatas, Transform[] eventTrans) {

        for (int i = 0; i < eventDatas.Length; i++) {

            GenerateEvent(eventDatas[i], eventTrans[i]);
        }
     }

    /// <summary>
    /// イベント生成。各イベントごとに振る舞いを変える処理のオーバーロード
    /// スクリプタブル・オブジェクトを利用せず、MissionEventTrigger に直接 EventBase のプレファブをアサインして利用する場合
    /// </summary>
    /// <param name="eventPrefab"></param>
    /// <param name="eventTran"></param>
    private void GenerateEvent(EnemyController eventPrefab, Transform eventTran) {
        EnemyController enemy = Instantiate(eventPrefab, eventTran.position, eventPrefab.transform.rotation);
        enemy.SetUpEnemy(playerController, gameManager);

        gameManager.AddEnemyList(enemy);
    }


/**** 未使用 ****/

    ///// <summary>
    ///// 敵の生成
    ///// </summary>
    ///// <param name="eventData"></param>
    ///// <param name="eventTran"></param>
    //public void GenerateEnemy(EventDataSO.EventData eventData, Transform eventTran) {
    //    EnemyBase enemy = Instantiate(eventData.eventPrefab, eventTran).GetComponent<EnemyBase>();
    //    enemy.SetUpEnemy(playerController.gameObject, gameManager);
    //    //gameManager.AddEnemyList(enemy);
    //}

    ///// <summary>
    ///// ギミックの生成
    ///// </summary>
    ///// <param name="eventData"></param>
    ///// <param name="eventTran"></param>
    //public void GenerateGimmick(EventDataSO.EventData eventData, Transform eventTran) {
    //    GameObject gimmick = Instantiate(eventData.eventPrefab, eventTran);
    //    gameManager.AddGimmickList(gimmick);
    //}

    ///// <summary>
    ///// アイテムの生成
    ///// </summary>
    ///// <param name="eventData"></param>
    ///// <param name="eventTran"></param>
    //public void GenerateItem(EventDataSO.EventData eventData, Transform eventTran) {
    //    GameObject item = Instantiate(eventData.eventPrefab, eventTran);
    //    item.GetComponent<ItemController>().SetUpItem(playerController);

    //    // TODO List を作ったらここで処理を追加する

    //}
}
