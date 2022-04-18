using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Title : MonoBehaviour
{
    [SerializeField]
    private StageButton stageButtonPrefab;

    [SerializeField]
    private Transform stageButtonTran;

    [SerializeField]
    private List<StageButton> stageButtonList = new List<StageButton>();

    [SerializeField]
    private int[] debugStageNos;

    [SerializeField]
    private bool isLoadData;


    void Start() {
        //TransitionManager.instance.FadeNextScene(1.0f, SceneName.MainGame);

        // �f�o�b�O�p�B�N���A�����X�e�[�W�ԍ��̓o�^
        if (debugStageNos.Length > 0) {
            for (int i = 0; i < debugStageNos.Length; i++) {
                GameData.instance.AddClearStageNoList(debugStageNos[i]);
            }
        }

        // �N���A�����X�e�[�W�̔ԍ��� List �����[�h
        if (isLoadData) {
            // �N���A�������X�g�̏�񂪂���ꍇ
            if (PlayerPrefsHelper.ExistsData(GameData.CLEAR_STAGES_KEY)) {
                // ���\�h
                GameData.instance.LoadClearStageList();
            }
        }

        // �X�e�[�W�{�^���̍쐬
        GenerateStageButtons();

        // �f�o�b�O
        this.UpdateAsObservable()
            .Subscribe(_ => {
                if (Input.GetKeyDown(KeyCode.A)) GameData.instance.SaveClearStageList();

                if (Input.GetKeyDown(KeyCode.B)) GameData.instance.SetSaveData();
                if (Input.GetKeyDown(KeyCode.C)) GameData.instance.GetSaveData();
            })
            .AddTo(gameObject);
    }

    /// <summary>
    /// �N���A���Ă���X�e�[�W�Ǝ��̃X�e�[�W�̕��̃X�e�[�W�I���{�^���̍쐬
    /// </summary>
    private void GenerateStageButtons() {

        // �P�X�e�[�W
        StageButton stageButton = Instantiate(stageButtonPrefab, stageButtonTran, false);
        stageButton.SetUpStageButton(0);
        stageButtonList.Add(stageButton);

        // 2�X�e�[�W�ȍ~
        for (int i = 0; i < GameData.instance.clearStageNoList.Count; i++) {
            stageButton = Instantiate(stageButtonPrefab, stageButtonTran, false);
            stageButton.SetUpStageButton(GameData.instance.clearStageNoList[i] + 1);
            stageButtonList.Add(stageButton);
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            TransitionManager.instance.FadeNextScene(1.0f, SceneName.MainGame);   //  <=  �V�[���J�ڂ������񋓎q��I�����܂�
        }
    }
}
