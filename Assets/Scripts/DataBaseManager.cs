using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 各種データベース用のスクリプタブル・オブジェクトの管理クラス
/// </summary>
public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;

    [SerializeField, HideInInspector]
    private EventDataSO enemyEventDataSO;

    [SerializeField, HideInInspector]
    private EventDataSO gimmickDataSO;

    [SerializeField, HideInInspector]
    private EventDataSO trapDataSO;

    [SerializeField, HideInInspector]
    private EventDataSO itemDataSO;

    [SerializeField, HideInInspector]
    private EventDataSO treasureDataSO;

    [SerializeField, HideInInspector]
    private EnemyDataSO enemyDataSO;

    [SerializeField]
    private BranchDataSO branchDataSO;


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 分岐番号からパス情報を取得
    /// </summary>
    /// <param name="searchStageNo"></param>
    /// <returns></returns>
    public RailPathData[] GetRailPathDatasFromBranchNo(int searchBranchNo) {
        return branchDataSO.branchDatasList.Find(x => x.branchNo == searchBranchNo).railPathDatas;
    }

    //public RailPathData[] GetRailPathDatasFromBranchDirectionType(BranchDirectionType branchDirectionType, List<RailPathDataSO.StagePathData.RootData> rootDatas) {
    //    return rootDatas.Find(x => x.branchDirectionType == branchDirectionType).railPathDatas;
    //}

    /// <summary>
    /// イベントの情報を取得
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="eventNo"></param>
    /// <returns></returns>
    public EventDataSO.EventData GetEventDataFromEventType(EventType eventType, int eventNo) {
        switch (eventType) {
            case EventType.Enemy:
                return enemyEventDataSO.eventDatasList.Find(x => x.eventNo == eventNo);

            case EventType.Gimmick:
                return gimmickDataSO.eventDatasList.Find(x => x.eventNo == eventNo);

            case EventType.Trap:
                return trapDataSO.eventDatasList.Find(x => x.eventNo == eventNo);

            case EventType.Item:
                return itemDataSO.eventDatasList.Find(x => x.eventNo == eventNo);

            case EventType.Treasure:
                return treasureDataSO.eventDatasList.Find(x => x.eventNo == eventNo);

            default:
                return null;
        }
    }

    /// <summary>
    /// 敵の情報を取得
    /// </summary>
    /// <param name="searchEnemyNo"></param>
    /// <returns></returns>
    public EnemyData GetEnemyData(int searchEnemyNo) {
        return enemyDataSO.enemyDatasList.Find(x => x.enemyNo == searchEnemyNo);
    }
}
