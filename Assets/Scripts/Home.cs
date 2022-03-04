using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    [SerializeField]
    private StageButton stageButtonPrefab;

    [SerializeField]
    private Transform stageButtonTran;

    [SerializeField]
    private List<StageButton> stageButtonList = new List<StageButton>();

    [SerializeField]
    private int[] debugStageNos;

    
    void Start()
    {
        //TransitionManager.instance.FadeNextScene(1.0f, SceneName.MainGame);

        // �f�o�b�O�p�B�N���A�����X�e�[�W�ԍ��̓o�^
        if (debugStageNos.Length > 0) {
            for (int i = 0; i < debugStageNos.Length; i++) {
                GameData.instance.AddClearStageNoList(debugStageNos[i]);
            }
        }

        // �X�e�[�W�{�^���̍쐬
        GenerateStageButtons();
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
}
