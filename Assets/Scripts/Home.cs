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

        // デバッグ用。クリアしたステージ番号の登録
        if (debugStageNos.Length > 0) {
            for (int i = 0; i < debugStageNos.Length; i++) {
                GameData.instance.AddClearStageNoList(debugStageNos[i]);
            }
        }

        // ステージボタンの作成
        GenerateStageButtons();
    }

    /// <summary>
    /// クリアしているステージと次のステージの分のステージ選択ボタンの作成
    /// </summary>
    private void GenerateStageButtons() {

        // １ステージ
        StageButton stageButton = Instantiate(stageButtonPrefab, stageButtonTran, false);
        stageButton.SetUpStageButton(0);
        stageButtonList.Add(stageButton);

        // 2ステージ以降
        for (int i = 0; i < GameData.instance.clearStageNoList.Count; i++) {
            stageButton = Instantiate(stageButtonPrefab, stageButtonTran, false);
            stageButton.SetUpStageButton(GameData.instance.clearStageNoList[i] + 1);
            stageButtonList.Add(stageButton);
        }
    }
}
