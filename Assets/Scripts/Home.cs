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

        if (debugStageNos.Length > 0) {
            for (int i = 0; i < debugStageNos.Length; i++) {
                GameData.instance.AddClearStageNoList(debugStageNos[i]);
            }
        } else {
            GameData.instance.AddClearStageNoList(0);
        }

        GenerateStageButtons();
    }

    /// <summary>
    /// クリアしているステージと次のステージの分のステージ選択ボタンの作成
    /// </summary>
    private void GenerateStageButtons() {
        for (int i = 0; i < GameData.instance.clearStageNoList.Count; i++) {
            StageButton stageButton = Instantiate(stageButtonPrefab, stageButtonTran, false);
            stageButton.SetUpStageButton(GameData.instance.clearStageNoList[i]);
            stageButtonList.Add(stageButton);
        }
    }
}
