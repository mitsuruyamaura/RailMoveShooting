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
    public void GenerateEvents((EventType[] eventTypes, int[] eventNos) events, Transform[] eventTrans) {

        // イベントデータを取得するための配列を準備
        EventDataSO.EventData[] eventDatas = new EventDataSO.EventData[events.eventTypes.Length];

        // イベントの種類と番号に応じてスクリプタブル・オブジェクトからイベントデータを検索
        for (int i = 0; i < events.eventTypes.Length; i++) {
            eventDatas[i] = DataBaseManager.instance.GetEventDataFromEventType(events.eventTypes[i], events.eventNos[i]);
        }

        // TODO ベースクラスを利用して一元化する
        // イベントデータに指定されている種類のイベント生成
        for (int i = 0; i < eventDatas.Length; i++) {
            switch (events.eventTypes[i]) {
                case EventType.Enemy:
                    GenerateEnemy(eventDatas[i], eventTrans[i]);
                    continue;

                case EventType.Gimmick:
                    GenerateGimmick(eventDatas[i], eventTrans[i]);
                    continue;

                case EventType.Item:
                    GenerateItem(eventDatas[i], eventTrans[i]);
                    continue;

                case EventType.Boss:
                    GenerateEnemy(eventDatas[i], eventTrans[i]);
                    continue;
            }
        }
    }

    /// <summary>
    /// 敵の生成
    /// </summary>
    /// <param name="eventData"></param>
    /// <param name="eventTran"></param>
    public void GenerateEnemy(EventDataSO.EventData eventData, Transform eventTran) {
        EnemyBase enemy = Instantiate(eventData.eventPrefab, eventTran).GetComponent<EnemyBase>();
        enemy.SetUpEnemy(playerController.gameObject, gameManager);
        //gameManager.AddEnemyList(enemy);
    }

    /// <summary>
    /// ギミックの生成
    /// </summary>
    /// <param name="eventData"></param>
    /// <param name="eventTran"></param>
    public void GenerateGimmick(EventDataSO.EventData eventData, Transform eventTran) {
        GameObject gimmick = Instantiate(eventData.eventPrefab, eventTran);
        gameManager.AddGimmickList(gimmick);
    }

    /// <summary>
    /// アイテムの生成
    /// </summary>
    /// <param name="eventData"></param>
    /// <param name="eventTran"></param>
    public void GenerateItem(EventDataSO.EventData eventData, Transform eventTran) {
        GameObject item = Instantiate(eventData.eventPrefab, eventTran);
        item.GetComponent<ItemController>().SetUpItem(playerController);

        // TODO List を作ったらここで処理を追加する

    }
}
