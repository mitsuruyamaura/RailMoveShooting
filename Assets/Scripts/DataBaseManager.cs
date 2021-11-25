using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// �e��f�[�^�x�[�X�p�̃X�N���v�^�u���E�I�u�W�F�N�g�̊Ǘ��N���X
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
    private StagePathDataSO stagePathDataSO;

    [SerializeField]
    private VideoDataSO videoDataSO;

    [SerializeField]
    private WeaponDataSO weaponDataSO;

    /// <summary>
    /// stagePathDatasList �ϐ��� Count �p�̃v���p�e�B
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
    /// �X�e�[�W�p�X�ԍ����番���� RailPathData �����擾
    /// </summary>
    /// <param name="searchStageNo"></param>
    /// <returns></returns>
    public RailPathData GetRailPathDatasFromBranchNo(int nextStagePathDataNo, BranchDirectionType searchBranchDirectionType) {
        return stagePathDataSO.stagePathDatasList[nextStagePathDataNo].branchDatasList.Find(x => x.branchDirectionType == searchBranchDirectionType).railPathData;
    }

    /// <summary>
    /// �X�e�[�W���̃��[�g�̐��̎擾
    /// </summary>
    /// <returns></returns>
    public int GetStagePathDetasListCount() {
        return stagePathDataSO.stagePathDatasList.Count;
    }

    /// <summary>
    /// �u�����`�̊Ǘ����Ă��镪�򐔂̎擾
    /// </summary>
    /// <param name="branchNo"></param>
    /// <returns></returns>
    public int GetBranchDatasListCount(int branchNo) {
        return stagePathDataSO.stagePathDatasList[branchNo].branchDatasList.Count;
    }

    //public RailPathData[] GetRailPathDatasFromBranchDirectionType(BranchDirectionType branchDirectionType, List<RailPathDataSO.StagePathData.RootData> rootDatas) {
    //    return rootDatas.Find(x => x.branchDirectionType == branchDirectionType).railPathDatas;
    //}

    /// <summary>
    /// �C�x���g�̏����擾
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
    /// �G�̏����擾
    /// </summary>
    /// <param name="searchEnemyNo"></param>
    /// <returns></returns>
    public EnemyData GetEnemyData(int searchEnemyNo) {
        return enemyDataSO.enemyDatasList.Find(x => x.enemyNo == searchEnemyNo);
    }

    /// <summary>
    /// VideoClip �̏����擾
    /// </summary>
    /// <param name="searchVideoNo"></param>
    /// <returns></returns>
    public VideoData GetVideoData(int searchVideoNo) {
        return videoDataSO.videoDatasList.Find(x => x.videoNo == searchVideoNo);
    }

    /// <summary>
    /// WeaponData �̏����擾
    /// </summary>
    /// <param name="searchWeaponNo"></param>
    /// <returns></returns>
    public WeaponData GetWeaponData(int searchWeaponNo) {
        return weaponDataSO.weaponDatasList.Find(x => x.weaponNo == searchWeaponNo);
    }
}
