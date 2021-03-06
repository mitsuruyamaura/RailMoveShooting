using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class MissionEventDetail : MonoBehaviour {

    [Header("ミッションのクリア条件")]
    public ClearConditionsType clearConditionsType;

    [Header("ミッションクリアのための敵の残数/残り時間")]
    public int missionDuration;

    [Header("イベントのプレファブ"), HideInInspector]
    public EventBase[] eventPrefabs;

    [Header("敵のプレファブ")]
    public EnemyController[] enemyPrefabs;

    [Header("発生するイベントの種類")]
    public EventType[] eventTypes;

    [Header("発生するイベントの番号")]
    public int[] eventNos;


    //mi

    [Tooltip("イベントの生成地点")]
    public Transform[] eventTrans;

    [Header("ムービーのデータ")]
    public VideoClip videoClip;

    [Header("再生するムービーの番号"), HideInInspector]
    public int videoNo;



    //private BoxCollider boxCollider;
    //private GameManager gameManager;

    ///// <summary>
    ///// EventTriggerPoint の準備
    ///// </summary>
    ///// <param name="gameManager"></param>
    //public void SetUpMissionTriggerPoint(GameManager gameManager) {
    //    this.gameManager = gameManager;

    //    TryGetComponent(out boxCollider);
    //}

    //private void OnTriggerEnter(Collider other) {
    //    if (other.tag == "Player") {
    //        Debug.Log("通過");

    //        // ミッション発生の重複判定防止
    //        boxCollider.enabled = false;

    //        // ミッション開始の準備
    //        gameManager.PreparateMission(missionDuration, clearConditionsType, (eventTypes, eventNos), eventTrans);
    //    }
    //}
}