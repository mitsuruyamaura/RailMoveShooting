using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    [SerializeField]
    private Button btnChoose;

    [SerializeField]
    private Text txtStageName;

    /// <summary>
    /// ボタンの設定
    /// </summary>
    /// <param name="stageNo"></param>
    /// <param name="name"></param>
    public void SetUpStageButton(int stageNo, string name = null) {

        btnChoose.onClick.AddListener(() => OnClickStageButton(stageNo));
        txtStageName.text = name == null ? "Stage " + (stageNo + 1) : name;
    }

    /// <summary>
    /// ボタンを押下した際の処理
    /// </summary>
    /// <param name="stageNo"></param>
    private void OnClickStageButton(int stageNo) {
        GameData.instance.chooseStageNo = stageNo;

        TransitionManager.instance.FadeNextScene(1.0f, SceneName.MainGame);
        //SceneStateManager.instance.PrepareChangeScene(SceneName.MainGame);
    }
}
