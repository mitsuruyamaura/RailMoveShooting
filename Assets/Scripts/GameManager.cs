using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField, HideInInspector]
    private MissionTriggerPoint[] eventTriggerPoint;

    [SerializeField, HideInInspector]
    private List<EnemyController_Normal> enemiesList = new List<EnemyController_Normal>();

    [SerializeField, HideInInspector]
    private List<GameObject> gimmicksList = new List<GameObject>();

    [SerializeField, HideInInspector]
    private PathDataSO pathDataSO;

    [SerializeField, HideInInspector]
    private FieldAutoScroller fieldAutoScroller;

    [SerializeField, HideInInspector]
    private UIManager uiManager;

    [System.Serializable]
    public class RootEventData {
        public int[] rootEventNos;
        public BranchDirectionType[] branchDirectionTypes;  // ����̕���
        public RootType rootType;
    }

    [SerializeField, HideInInspector]
    private List<RootEventData> rootDatasList = new List<RootEventData>();

    private int currentRailCount;       // ���݂̐i�s��

    [SerializeField, HideInInspector]
    private PlayerController playerController;

    [SerializeField, HideInInspector]
    private int currentMissionDuration;

    [SerializeField, HideInInspector]
    private EventGenerator eventGenerator;




    [SerializeField]
    private RailMoveController railMoveController;

    [SerializeField, Header("�o�H�p�̃p�X�Q�̌��f�[�^")]
    private RailPathData originRailPathData;

    [SerializeField, Header("�p�X�ɂ�����~�b�V�����̔����L��")]  // Debug �p
    private bool[] isMissionTriggers;

    [Header("���݂̃Q�[���̐i�s���")]
    public GameState currentGameState;


    private IEnumerator Start() {

        originRailPathData = DataBaseManager.instance.GetRailPathDatasFromBranchNo(0, BranchDirectionType.NoBranch);

        // RailMoveController �̏����ݒ�
        railMoveController.SetUpRailMoveController(this);

        // �p�X�f�[�^���~�b�V�����̔����L�����擾
        SetMissionTriggers();

        // ���ɍĐ����郌�[���ړ��̖ړI�n�ƌo�H�̃p�X��ݒ�
        railMoveController.SetNextRailPathData(originRailPathData);

        // ���[���ړ��̌o�H�ƈړ��o�^����������܂őҋ@
        yield return new WaitUntil(() => railMoveController.GetMoveSetting());

        // �Q�[���̐i�s��Ԃ��ړ����ɕύX����
        currentGameState = GameState.Play_Move;
    }

    /// <summary>
    /// �p�X�f�[�^���~�b�V�����̔����L�����擾
    /// </summary>
    private void SetMissionTriggers() {

        // ���򂪂���ꍇ�ɂ͕ύX����
        isMissionTriggers = new bool[originRailPathData.GetIsMissionTriggers().Length];

        // �~�b�V���������L���̏���o�^
        isMissionTriggers = originRailPathData.GetIsMissionTriggers();
    }

    /// <summary>
    /// �~�b�V�����̔����L���̔���
    /// </summary>
    /// <param name="index"></param>
    public void CheckMissionTrigger(int index) {

        if (isMissionTriggers[index]) {
            // TODO �~�b�V��������
            Debug.Log("�~�b�V��������");

            // Debug �p�@���܂͂��̂܂ܐi�s
            railMoveController.ResumeMove();

        } else {
            // �~�b�V�����Ȃ��B���̃p�X�ֈړ����ĊJ
            railMoveController.ResumeMove();
        }
    }


    public void PreparateCheckNextBranch(int nextbranchNo) {

        StartCoroutine(CheckNextBranch(nextbranchNo));

    }

    private IEnumerator CheckNextBranch(int nextStagePathDataNo) {
        if (nextStagePathDataNo >= DataBaseManager.instance.GetStagePathDetasListCount()) {
            // �I��
            Debug.Log("�Q�[���I��");

            yield break;
        }

        // TODO ���򂪂��邩�ǂ����̔���
        if (DataBaseManager.instance.GetBranchDatasListCount(nextStagePathDataNo) == 1) {
            // ����Ȃ��̏ꍇ�A���̌o�H��o�^
            originRailPathData = DataBaseManager.instance.GetRailPathDatasFromBranchNo(nextStagePathDataNo, BranchDirectionType.NoBranch);
        } else {
            // ���򂪂���ꍇ�AUI �ɕ����\�����A�I����҂�

        }

        // �����A���̌o�H��o�^
        originRailPathData = DataBaseManager.instance.GetRailPathDatasFromBranchNo(nextStagePathDataNo, BranchDirectionType.NoBranch);

        SetMissionTriggers();

        // �o�H���ړ���ɐݒ�
        railMoveController.SetNextRailPathData(originRailPathData);
    }


    public IEnumerator SetStart() {

        //playerController.SetUpPlayer();

        //eventGenerator.SetUpEventGenerator(this, playerController);

        //uiManager.SetPlayerInfo(playerController.Hp, playerController.maxBullet);

        //uiManager.SwitchActivatePlayerInfoSet(true);

        //StartCoroutine(uiManager.GenerateLife(playerController.Hp));

        //// �Q�[���̏���
        //yield return StartCoroutine(PreparateGame());

        // ���̃��[�g�̊m�F�Ɛݒ�
        yield return StartCoroutine(CheckNextRootBranch());
    }

    /// <summary>
    /// �Q�[���̏���
    /// </summary>
    /// <returns></returns>
    private IEnumerator PreparateGame() {
        for (int i = 0; i < eventTriggerPoint.Length; i++) {
            eventTriggerPoint[i].SetUpMissionTriggerPoint(this);
        }
        yield return null;
    }

    /// <summary>
    /// �G�̏��� List �ɒǉ�
    /// </summary>
    /// <param name="enemyController"></param>
    public void AddEnemyList(EnemyController_Normal enemyController) {
        enemiesList.Add(enemyController);
    }

    /// <summary>
    /// �M�~�b�N�̏��� List �ɒǉ�
    /// </summary>
    /// <param name="gimmick"></param>
    public void AddGimmickList(GameObject gimmick) {
        gimmicksList.Add(gimmick);
    }

    /// <summary>
    /// ���ׂĂ̓G�̈ړ����ꎞ��~
    /// </summary>
    public void StopMoveAllEnemies() {
        if (enemiesList.Count <= 0) {
            return;
        }

        for (int i = 0; i < enemiesList.Count; i++) {
            enemiesList[i].GetComponent<EnemyController_Normal>().PauseMove();
        }
    }

    /// <summary>
    /// ���ׂĂ̓G�̈ړ����ĊJ
    /// </summary>
    public void ResumeMoveAllEnemies() {
        if (enemiesList.Count <= 0) {
            return;
        }

        for (int i = 0; i < enemiesList.Count; i++) {
            enemiesList[i].GetComponent<EnemyController_Normal>().ResumeMove();
        }
    }

    /// <summary>
    /// PathData �� List ���擾
    /// </summary>
    /// <param name="checkRootNo"></param>
    /// <returns></returns>
    private List<PathData> GetPathDatasList(int checkRootNo) {
        return pathDataSO.rootDatasList.Find(x => x.rootNo == checkRootNo).pathDatasList;
    }

    /// <summary>
    /// ���[�g�̊m�F
    /// ���򂪂���ꍇ�ɂ͕���̖��{�^���𐶐�
    /// </summary>
    /// <returns></returns>
    public IEnumerator CheckNextRootBranch() {

        if (currentRailCount >= rootDatasList.Count) {
            // TODO �N���A����
            Debug.Log("�N���A");

            yield break;
        }

        // ���݂̃��[���J�E���g�� RootType ���m�F���āA���ɔ������郋�[�g�����߂�
        switch (rootDatasList[currentRailCount].rootType) {
            case RootType.Normal_Battle:
                // ���̃��[�g���P�Ȃ�
                if (rootDatasList[currentRailCount].rootEventNos.Length == 1) {
                    // �����I�Ƀ��[���ړ����J�n
                    fieldAutoScroller.SetNextField(GetPathDatasList(rootDatasList[currentRailCount].rootEventNos[0]));
                    Debug.Log("����Ȃ��̈ړ��J�n");
                } else {
                    // ���򂪂���ꍇ�A����C�x���g�𔭐������āA��ʏ�ɖ��̃{�^����\��
                    yield return StartCoroutine(uiManager.GenerateBranchButtons(rootDatasList[currentRailCount].rootEventNos, rootDatasList[currentRailCount].branchDirectionTypes));

                    // ��󂪉������܂őҋ@(while �ł�OK)
                    yield return new WaitUntil(() => uiManager.GetSubmitBranch().Item1 == true);

                    // �I����������̃��[�g��ݒ�
                    fieldAutoScroller.SetNextField(GetPathDatasList(uiManager.GetSubmitBranch().Item2));
                }
                
                break;

            case RootType.Boss_Battle:

                break;

            case RootType.Event:

                break;
        }
       
        // ���̂��߂ɃA�b�v
        currentRailCount++;
    }

    /// <summary>
    /// �G�̏��� List ����폜���A�~�b�V�������̓G�̎c�������炷
    /// </summary>
    /// <param name="enemy"></param>
    public void RemoveEnemyList(EnemyController_Normal enemy) {
        enemiesList.Remove(enemy);

        currentMissionDuration--;
    }

    /// <summary>
    /// �~�b�V�����̏���
    /// </summary>
    /// <param name="missionDuration"></param>
    /// <param name="clearConditionsType"></param>
    /// <param name="events"></param>
    /// <param name="eventTrans"></param>
    public void PreparateMission(int missionDuration, ClearConditionsType clearConditionsType, (EventType[] eventTypes, int[] eventNos) events, Transform[] eventTrans) {

        // �J�����̈ړ���~
        fieldAutoScroller.StopAndPlayMotion();

        // �~�b�V�����̎��Ԑݒ�
        currentMissionDuration = missionDuration;

        // �~�b�V�������̊e�C�x���g�̐���(�G�A�M�~�b�N�A�g���b�v�A�A�C�e���Ȃǂ𐶐�)
        eventGenerator.GenerateEvents(events, eventTrans);

        // �~�b�V�����J�n
        StartCoroutine(StartMission(clearConditionsType));
    }

    /// <summary>
    /// �~�b�V�����J�n
    /// </summary>
    /// <param name="clearConditionsType"></param>
    /// <returns></returns>
    private IEnumerator StartMission(ClearConditionsType clearConditionsType) {

        // �~�b�V�����̊Ď�
        yield return StartCoroutine(ObservateMission(clearConditionsType));

        // �~�b�V�����I��
        EndMission();
    }

    /// <summary>
    /// �~�b�V�����̊Ď�
    /// �e�C�x���g�̏�Ԃ��Ď�
    /// </summary>
    /// <param name="clearConditionsType"></param>
    /// <returns></returns>
    private IEnumerator ObservateMission(ClearConditionsType clearConditionsType) {

        // �N���A�����𖞂����܂ŊĎ�
        while (currentMissionDuration > 0) {

            // �c�莞�Ԃ��Ď�����ꍇ
            if (clearConditionsType == ClearConditionsType.TimeUp) {

                // �J�E���g�_�E��
                currentMissionDuration--;
            }

            yield return null;
        }

        Debug.Log("�~�b�V�����I��");
    }

    /// <summary>
    /// �~�b�V�����I��
    /// </summary>
    public void EndMission() {

        ClearEnemiesList();

        ClearGimmicksList();

        // �J�����̈ړ��ĊJ
        fieldAutoScroller.StopAndPlayMotion();
    }

    /// <summary>
    /// �G�� List ���N���A
    /// </summary>
    private void ClearEnemiesList() {

        if (enemiesList.Count > 0) {
            for (int i = 0; i < enemiesList.Count; i++) {
                Destroy(enemiesList[i]);
            }
        }

        enemiesList.Clear();
    }

    /// <summary>
    /// �M�~�b�N�� List ���N���A
    /// </summary>
    private void ClearGimmicksList() {

        if (gimmicksList.Count > 0) {
            for (int i = 0; i < gimmicksList.Count; i++) {
                Destroy(gimmicksList[i]);
            }
        }

        gimmicksList.Clear();
    }
}