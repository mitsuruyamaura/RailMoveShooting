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
    private WeaponDataSO weaponDataSO;

    [SerializeField]
    private StagePathDataSO stagePathDataSO;

    [SerializeField, HideInInspector]
    private VideoDataSO videoDataSO;

    [SerializeField]
    private StageSO stageSO;


    /// <summary>
    /// stagePathDatasList 変数の Count 用のプロパティ
    /// </summary>
    public int StagePathDataCount
    {
        get => stagePathDataSO.stagePathDatasList.Count;
    }


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// WeaponData の情報を取得
    /// </summary>
    /// <param name="searchWeaponNo"></param>
    /// <returns></returns>
    public WeaponData GetWeaponData(int searchWeaponNo) {
        return weaponDataSO.weaponDatasList.Find(x => x.weaponNo == searchWeaponNo);
    }

    /// <summary>
    /// ステージパス番号から分岐先の RailPathData 情報を取得
    /// </summary>
    /// <param name="searchStageNo"></param>
    /// <returns></returns>
    public RailPathData GetRailPathDatasFromBranchNo(int nextStagePathDataNo, BranchDirectionType searchBranchDirectionType) {
        //return stagePathDataSO.stagePathDatasList[nextStagePathDataNo].branchDatasList.Find(x => x.branchDirectionType == searchBranchDirectionType).railPathData;
        
        // 選択したステージの番号を利用する方法に変更する
        return stageSO.stageDataList[GameData.instance.chooseStageNo].stagePathDatasList[nextStagePathDataNo].branchDatasList.Find(x => x.branchDirectionType == searchBranchDirectionType).railPathData;
    }

    /// <summary>
    /// ステージ内のルートの数の取得
    /// </summary>
    /// <returns></returns>
    public int GetStagePathDetasListCount() {
        return stagePathDataSO.stagePathDatasList.Count;
    }

    /// <summary>
    /// 分岐の種類の取得
    /// </summary>
    /// <param name="nextStagePathDataNo"></param>
    /// <param name="no"></param>
    /// <returns></returns>
    public BranchDirectionType GetBranchDirectionTypeFromRailPathData(int nextStagePathDataNo, int no) {
        return stagePathDataSO.stagePathDatasList[nextStagePathDataNo].branchDatasList[no].branchDirectionType;
    }

    /// <summary>
    /// 全分岐情報の取得
    /// </summary>
    /// <param name="nextStagePathDataNo"></param>
    /// <returns></returns>
    public BranchDirectionType[] GetBranchDirectionTypes(int nextStagePathDataNo) {
        return stagePathDataSO.stagePathDatasList[nextStagePathDataNo].branchDatasList.Select(x => x.branchDirectionType).ToArray();
    }


    /// <summary>
    /// ブランチの管理している分岐数の取得
    /// </summary>
    /// <param name="branchNo"></param>
    /// <returns></returns>
    public int GetBranchDatasListCount(int branchNo) {
        return stagePathDataSO.stagePathDatasList[branchNo].branchDatasList.Count;
    }


    public int GetStageListCount() {
        return stageSO.stageDataList.Count;
    }


    public StagePathDataSO GetStagePathDataSO(int stageNo) {
        return stageSO.stageDataList[stageNo];
    }

    // mi

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
                EventDataSO.EventData enemy = enemyEventDataSO.eventDatasList.Find(x => x.eventNo == eventNo);
                Debug.Log(enemy.eventPrefab.name);
                return enemy;

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

    /// <summary>
    /// VideoClip の情報を取得
    /// </summary>
    /// <param name="searchVideoNo"></param>
    /// <returns></returns>
    public VideoData GetVideoData(int searchVideoNo) {
        return videoDataSO.videoDatasList.Find(x => x.videoNo == searchVideoNo);
    }
}
