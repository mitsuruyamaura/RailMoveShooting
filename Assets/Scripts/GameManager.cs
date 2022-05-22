using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField, HideInInspector]
    private MissionEventDetail[] eventTriggerPoint;

    [SerializeField, HideInInspector]
    private List<GameObject> gimmicksList = new List<GameObject>();

    [SerializeField, HideInInspector]
    private PathDataSO pathDataSO;

    [SerializeField, HideInInspector]
    private FieldAutoScroller fieldAutoScroller;

    [System.Serializable]
    public class RootEventData {
        public int[] rootEventNos;
        public BranchDirectionType[] branchDirectionTypes;  // ����̕���
        public RootType rootType;
    }

    [SerializeField, HideInInspector]
    private List<RootEventData> rootDatasList = new List<RootEventData>();

    private int currentRailCount;       // ���݂̐i�s��



    //�@��������g���Ă���

    [SerializeField]
    private RailMoveController railMoveController;

    [SerializeField, Header("�o�H�p�̃p�X�Q�̌��f�[�^")]
    private RailPathData originRailPathData;

    [SerializeField, Header("�p�X�ɂ�����~�b�V�����̔����L��")]  // Debug �p
    private bool[] isMissionTriggers;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private EventGenerator eventGenerator;

    [SerializeField, Header("�~�b�V�����Ŕ������Ă���C�x���g�̃��X�g"), HideInInspector]
    private List<EventBase> eventBasesList = new List<EventBase>();

    [SerializeField]
    private List<EnemyController> enemiesList = new List<EnemyController>();

    private int currentMissionDuration;

    [Header("���݂̃Q�[���̐i�s���")]
    public GameState currentGameState;

    [SerializeField]
    private WeaponEventInfo weaponEventInfo;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private WeaponChanger weaponChanger;

    [SerializeField]
    private CameraController cameraController;

    private int currentStageNo;

    //private int clearMissionCount;

    //[SerializeField]
    //private List<MissionEventDetail> missionEventDetailsList = new List<MissionEventDetail>();

    // ���[�h�m�F�p
    //public RailPathData.PathDataDetail pathDataDetail;

    [SerializeField]
    private bool[] isMoviePlays;


    private IEnumerator Start() {

        SoundManager.instance.PlayBGM(SoundManager.BGM_Type.Main);

        currentGameState = GameState.Wait;

        if (PlayerPrefsHelper.ExistsData("rail3")) {
            Debug.Log("�Z�[�u�f�[�^����");
            originRailPathData = PlayerPrefsHelper.LoadGetObjectData<RailPathData>("rail3");
        } else {
            Debug.Log("�Z�[�u�f�[�^�Ȃ�");
            originRailPathData = DataBaseManager.instance.GetRailPathDatasFromBranchNo(0, BranchDirectionType.NoBranch);
            currentStageNo = 0;
        }

        // TODO �擾�����ŃX�e�[�W�ԍ����g���悤�ɕύX
        originRailPathData = DataBaseManager.instance.GetRailPathDatasFromBranchNo(0, BranchDirectionType.NoBranch);
        

        //PlayerPrefsHelper.SaveSetObjectData("rail3", originRailPathData.pathDataDetails[0]);

        eventGenerator.SetUpEventGenerator(this, playerController);

        // ��������o�^
        GameData.instance.AddWeaponData(DataBaseManager.instance.GetWeaponData(0));
        //GameData.instance.AddWeaponData(DataBaseManager.instance.GetWeaponData(1));
        //GameData.instance.AddWeaponData(DataBaseManager.instance.GetWeaponData(2));

        // ����擾�C�x���g�p�̐ݒ�
        weaponEventInfo.InitializeWeaponEventInfo();

        // ��������ݒ�
        playerController.ChangeBulletData(GameData.instance.weaponDatasList[0]);

        // ��������̃��f���\��
        weaponChanger.InitWeaponModel();

        // �ǉ�
        uiManager.GetWeaponChangeButton().onClick.AddListener(weaponChanger.ChangeWeapon);

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

        // �B���A�N�V�����̃f�o�b�O�p
        //currentGameState = GameState.Play_Mission;
    }

    /// <summary>
    /// �p�X�f�[�^���~�b�V�����̔����L�����擾
    /// </summary>
    private void SetMissionTriggers() {

        // �z��̏�����
        isMissionTriggers = new bool[originRailPathData.GetIsMissionTriggers().Length];

        // �~�b�V���������L���̏���o�^
        isMissionTriggers = originRailPathData.GetIsMissionTriggers();

        // GetIsMissionTriggers �Ŏ擾���Ă���̂ŕs�v
        //for (int i = 0; i < isMissionTriggers.Length; i++) {
        //    if (originRailPathData.pathDataDetails[i].missionEventDetail) {
        //        missionEventDetailsList.Add(originRailPathData.pathDataDetails[i].missionEventDetail);
        //    }
        //}

        //clearMissionCount = 0;

        // ���[�r�[�Đ����p�̔z���������
        isMoviePlays = new bool[originRailPathData.GetIsMoviePlays().Length];

        // ���[�r�[�Đ��L���̏���o�^
        isMoviePlays = originRailPathData.GetIsMoviePlays();
    }

    /// <summary>
    /// �~�b�V�����̔����L���̔���
    /// </summary>
    /// <param name="index"></param>
    public void CheckMissionTrigger(int index) {

        if (isMissionTriggers[index]) {
            // TODO �~�b�V��������
            Debug.Log("�~�b�V��������");
            //Debug.Log(index);

            // �~�b�V�����̏���
            PreparateMission(originRailPathData.pathDataDetails[index].missionEventDetail);

            // Debug �p�@���܂͂��̂܂ܐi�s
            //railMoveController.ResumeMove();

        } else {
            // TODO �~�b�V�����Ȃ��B���̃p�X�ֈړ����ĊJ(�܂Ƃ߂ē������ꍇ�̏�����)
            //railMoveController.ResumeMove();

            // �p�X���Ƃɓ������ꍇ
            railMoveController.CountUp();
        }
    }

    /// <summary>
    /// �~�b�V�����̏���
    /// </summary>
    /// <param name="missionEventDetail"></param>
    private void PreparateMission(MissionEventDetail missionEventDetail) {

        // �~�b�V�����̎��Ԑݒ�
        currentMissionDuration = missionEventDetail.missionDuration;

        //Debug.Log(missionEventDetail.eventTypes[0]);
        
        // ����擾�C�x���g������
        if (missionEventDetail.eventTypes[0] == EventType.Weapon) {
            // ����̏����擾���ăZ�b�g
            weaponEventInfo.SetWeaponData(DataBaseManager.instance.GetWeaponData(missionEventDetail.eventNos[0]));
            weaponEventInfo.Show();
            Debug.Log("����擾�C�x���g ����");
        } else {

            // TODO �����������C���B�~�b�V�������̊e�C�x���g�̐���(�G�A�M�~�b�N�A�g���b�v�A�A�C�e���Ȃǂ𐶐�)
            //eventGenerator.PrepareGenerateEvents((missionEventDetail.eventTypes, missionEventDetail.eventNos), missionEventDetail.eventTrans);

            // �v���t�@�u�𒼐� MissionEventTrigger ���ɓo�^����(�X�N���v�^�u���E�I�u�W�F�N�g�𗘗p���Ȃ�)�ꍇ
            eventGenerator.PrepareGenerateEvents(missionEventDetail.enemyPrefabs, missionEventDetail.eventTrans);
        }

        // �~�b�V�����J�n
        StartCoroutine(StartMission(missionEventDetail.clearConditionsType));
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

            // ����擾�C�x���g���A����I���̂����ꂩ�̃{�^������������
            if (weaponEventInfo.gameObject.activeSelf && weaponEventInfo.isChooseWeapon) {
                // �C�x���g�I��
                currentMissionDuration = 0;

                weaponEventInfo.Hide();

                Debug.Log("����擾�C�x���g�I��");
                yield break;
            }

            yield return null;
        }

        Debug.Log("�~�b�V�����I��");
    }

    /// <summary>
    /// �~�b�V�����I��
    /// </summary>
    public void EndMission() {

        // ����̎擾�C�x���g�̏ꍇ�ɂ͕�����擾�����Ƀ|�b�v�A�b�v�����
        if (weaponEventInfo.gameObject.activeSelf) {
            weaponEventInfo.Hide();
        }

        ClearEventList();

        // �ړ��ĊJ(�g��Ȃ�)
        //railMoveController.ResumeMove();

        // �ړ��ĊJ
        railMoveController.CountUp();

        //ClearEnemiesList();

        //ClearGimmicksList();

        // �J�����̈ړ��ĊJ
        //fieldAutoScroller.StopAndPlayMotion();
    }

    /// <summary>
    /// �C�x���g�̏��� List ����폜���A�G�̏ꍇ�ɂ́A�~�b�V�������̓G�̎c�������炷
    /// </summary>
    /// <param name="enemy"></param>
    public void RemoveEventList(EventBase eventBase) {

        if (eventBase.eventType == EventType.Enemy || eventBase.eventType == EventType.Boss) {
            currentMissionDuration--;
        }
        eventBasesList.Remove(eventBase);
    }

    /// <summary>
    /// �C�x���g�����X�g�ɒǉ�
    /// </summary>
    /// <param name="eventBase"></param>
    public void AddEventList(EventBase eventBase) {
        eventBasesList.Add(eventBase);
    }

    /// <summary>
    /// �C�x���g�� List ���N���A
    /// </summary>
    private void ClearEventList() {
        if (eventBasesList.Count > 0) {
            for (int i = 0; i < eventBasesList.Count; i++) {
                Destroy(eventBasesList[i]);
            }
        }

        eventBasesList.Clear();
    }

    /// <summary>
    /// �G�̏��� List �ɒǉ�
    /// </summary>
    /// <param name="enemyController"></param>
    public void AddEnemyList(EnemyController enemyController) {
        enemiesList.Add(enemyController);
    }


    /// <summary>
    /// �G�̏��� List ����폜���A�~�b�V�������̓G�̎c�������炷
    /// </summary>
    /// <param name="enemy"></param>
    public void RemoveEnemyList(EnemyController enemy) {
        enemiesList.Remove(enemy);

        currentMissionDuration--;
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
    /// ���[�g�̕���m�F�̏���
    /// </summary>
    /// <param name="nextbranchNo">moveCount</param>
    public void PreparateCheckNextBranch(int nextbranchNo) {

        StartCoroutine(CheckNextBranch(nextbranchNo));
    }

    /// <summary>
    /// ���[�g�̕��򔻒�
    /// </summary>
    /// <param name="nextStagePathDataNo"></param>
    /// <returns></returns>
    private IEnumerator CheckNextBranch(int nextStagePathDataNo) {
        Debug.Log(nextStagePathDataNo);
        if (nextStagePathDataNo >= DataBaseManager.instance.GetStagePathDetasListCount()) {
            // �I��
            Debug.Log("�Q�[���I��");

            // �v���C���[�̃N���A�̏���
            playerController.PrepareClearSettings();

            // ���ׂĂ̕�����\��
            weaponChanger.InactiveWeapons();

            // �J�����̉��o
            cameraController.ClearCameraRoll(playerController.transform.position + new Vector3(0, 0, 10), new Vector3(0, 180, 0), new float[2] { 1.5f, 2.0f });

            // �N���A�����X�e�[�W�̔ԍ��� List �ɒǉ�
            GameData.instance.AddClearStageNoList(currentStageNo);

            // �Z�[�u
            GameData.instance.SetSaveData();

            // ResultCanvas ����
            uiManager.GenerateResultCanvas(0, 0);


            yield break;
        }

        // ���[�g�ɕ��򂪂��邩�ǂ����̔���
        if (DataBaseManager.instance.GetBranchDatasListCount(nextStagePathDataNo) == 1) {

            Debug.Log("����Ȃ��Ŏ��̃��[�g��");

            // ����Ȃ��̏ꍇ�A���̌o�H��o�^
            originRailPathData = DataBaseManager.instance.GetRailPathDatasFromBranchNo(nextStagePathDataNo, BranchDirectionType.NoBranch);
        } else {
            // ���򂪂���ꍇ�AUI �ɕ����\�����A�I����҂�

            Debug.Log("���[�g�̕��򔭐�");

            // ���򂪂���ꍇ�A����C�x���g�𔭐������āA��ʏ�ɖ��̃{�^����\��
            uiManager.GenerateBranchButtons(DataBaseManager.instance.GetBranchDirectionTypes(nextStagePathDataNo));

            // �����I������܂őҋ@(while �ł�OK)
            yield return new WaitUntil(() => uiManager.GetSubmitBranch().Item1 == true);

            // �I����������̃��[�g��ݒ�
            originRailPathData = DataBaseManager.instance.GetRailPathDatasFromBranchNo(nextStagePathDataNo, uiManager.GetSubmitBranch().Item2);
        }

        // ���[�g���̃~�b�V��������ݒ�
        SetMissionTriggers();

        // �o�H���ړ���ɐݒ�
        railMoveController.SetNextRailPathData(originRailPathData);

        // ���[���ړ��̌o�H�ƈړ��o�^����������܂őҋ@
        yield return new WaitUntil(() => railMoveController.GetMoveSetting());

        // �Q�[���̐i�s��Ԃ��ړ����ɕύX����
        currentGameState = GameState.Play_Move;
    }

    /// <summary>
    /// ���[�r�[���Đ����邩�m�F
    /// </summary>
    /// <param name="index"></param>
    public void CheckMoviePlay(int index) {

        Debug.Log(index);

        if (!isMoviePlays[index]) {

            Debug.Log("���[�r�[�Đ� �Ȃ�");

            // �~�b�V���������L���̊m�F
            CheckMissionTrigger(index);
        } else {

            // ���[�r�[�Đ��̏���
            StartCoroutine(PrepareMoviePlay(index));
        }
    }

    /// <summary>
    /// ���[�r�[�Đ��̏���
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private IEnumerator PrepareMoviePlay(int index) {

        Debug.Log("���[�r�[�����J�n");

        // Canvas ���\��
        uiManager.SwitchActivateCanvas(false);

        // TODO �Q�[���X�e�[�g��؂�ւ��āA��ʂ̃^�b�v���~�߂�
        playerController.IsShootPerimission = false;

        yield return StartCoroutine(PlayMovie());

        /// ���[�r�[�Đ�
        IEnumerator PlayMovie() {

            Debug.Log("���[�r�[�Đ��J�n");

            // ���[�r�[�Đ��̏����ƍĐ�
            VideoClipManager.instance.PrepareVideoClip(originRailPathData.pathDataDetails[index].missionEventDetail.videoNo, originRailPathData.pathDataDetails[index].missionEventDetail.videoClip);

            // ���[�r�[�̏������Ԃ����ҋ@
            yield return new WaitForSeconds(1.5f);

            Debug.Log("���[�r�[�����ҋ@ �I��");

            // ���[�r�[�Đ����I������܂őҋ@
            yield return new WaitUntil(() => !VideoClipManager.instance.IsVideoPlaying);
            //yield return new WaitForSeconds((float)originRailPathData.pathDataDetails[index].missionEventDetail.videoClip.length);

            Debug.Log("���[�r�[�Đ��@�I��");

            // BGM �Đ�
            SoundManager.instance.PlayBGM(SoundManager.BGM_Type.Main);

            // ��ʂ̃t�F�[�h�C�����߂�܂ł̊ԁA�ҋ@���Ă���
            yield return new WaitForSeconds(1.0f);

            // Canvas ��\��
            uiManager.SwitchActivateCanvas(true);

            // TODO �Q�[���X�e�[�g��؂�ւ��āA��ʂ̃^�b�v��L����
            playerController.IsShootPerimission = true;

            // �~�b�V���������L���̊m�F
            CheckMissionTrigger(index);
        }
    }

    
    // mi

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

        //for (int i = 0; i < enemiesList.Count; i++) {
        //    enemiesList[i].GetComponent<EnemyController_Normal>().PauseMove();
        //}
    }

    /// <summary>
    /// ���ׂĂ̓G�̈ړ����ĊJ
    /// </summary>
    public void ResumeMoveAllEnemies() {
        if (enemiesList.Count <= 0) {
            return;
        }

        //for (int i = 0; i < enemiesList.Count; i++) {
        //    enemiesList[i].GetComponent<EnemyController_Normal>().ResumeMove();
        //}
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


    // �g���Ă��Ȃ�

    /// <summary>
    /// AR �p
    /// </summary>
    /// <returns></returns>
    public IEnumerator SetStart() {

        //playerController.SetUpPlayer();

        //eventGenerator.SetUpEventGenerator(this, playerController);

        //uiManager.SetPlayerInfo(playerController.Hp, playerController.maxBullet);

        //uiManager.SwitchActivatePlayerInfoSet(true);

        //StartCoroutine(uiManager.GenerateLife(playerController.Hp));

        //// �Q�[���̏���
        //yield return StartCoroutine(PreparateGame());

        // TODO ���̃��[�g�̊m�F�Ɛݒ�(�Â����Ȃ̂ŁA�g���ꍇ�ɂ͐V�������ɂ���)
        yield return StartCoroutine(CheckNextRootBranch());   // CheckNextBranch �ɂ��邱��
    }

    /// <summary>
    /// ���[�g�̊m�F(�Â���BRootData ���g���Ă���)
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
                    yield return new WaitUntil(() => uiManager.GetSubmitBranchNo().Item1 == true);

                    // �I����������̃��[�g��ݒ�
                    fieldAutoScroller.SetNextField(GetPathDatasList(uiManager.GetSubmitBranchNo().Item2));
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

    ///// <summary>
    ///// �Q�[���̏���
    ///// </summary>
    ///// <returns></returns>
    //private IEnumerator PreparateGame() {
    //    for (int i = 0; i < eventTriggerPoint.Length; i++) {
    //        eventTriggerPoint[i].SetUpMissionTriggerPoint(this);
    //    }
    //    yield return null;
    //}


    ///// <summary>
    ///// �~�b�V�����̏���
    ///// </summary>
    ///// <param name="missionDuration"></param>
    ///// <param name="clearConditionsType"></param>
    ///// <param name="events"></param>
    ///// <param name="eventTrans"></param>
    //public void PreparateMission(int missionDuration, ClearConditionsType clearConditionsType, (EventType[] eventTypes, int[] eventNos) events, Transform[] eventTrans) {

    //    // �J�����̈ړ���~
    //    fieldAutoScroller.StopAndPlayMotion();

    //    // �~�b�V�����̎��Ԑݒ�
    //    currentMissionDuration = missionDuration;

    //    // �~�b�V�������̊e�C�x���g�̐���(�G�A�M�~�b�N�A�g���b�v�A�A�C�e���Ȃǂ𐶐�)
    //    eventGenerator.GenerateEvents(events, eventTrans);

    //    // �~�b�V�����J�n
    //    StartCoroutine(StartMission(clearConditionsType));
    //}
}